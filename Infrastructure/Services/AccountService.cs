using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.Dtos;
using Domain.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AccountService(UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<Response<IdentityResult>> RegisterAsync(UserRegisterDto model)
    {
        try
        {
            var user = new IdentityUser()
            {
                UserName = model.Username,
                Email = model.Email,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // await _userManager.AddToRoleAsync(user, "Manager");
                return new Response<IdentityResult>(result);
            }
            else
            {
                return new Response<IdentityResult>(HttpStatusCode.BadRequest,
                    result.Errors.Select(e => e.Description).ToList());
            }
        }
        catch (Exception ex)
        {
            return new Response<IdentityResult>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<IdentityUser>> LoginAsync(UserLoginDto model)
    {
        var existing = await _userManager.FindByNameAsync(model.Username);
        if (existing == null)
            return new Response<IdentityUser>(HttpStatusCode.BadRequest, "Login or Password  is incorrect");
        var checkPassword = await _userManager.CheckPasswordAsync(existing, model.Password);
        if (checkPassword == true)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, existing.UserName),
                new Claim(ClaimTypes.Email, existing.Email),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.DateOfBirth, "2005.02.02"),
                new Claim("Tax", "2345654"),
            };
            await _signInManager.SignInWithClaimsAsync(existing, model.RememberMe, claims);
            return new Response<IdentityUser>(existing); //
        }
        else
        {
            return new Response<IdentityUser>(HttpStatusCode.BadRequest, "Login or Password  is incorrect");
        }
    }

    public async Task<Response<string>> JWTLogin(JWTLoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null)
        {
            var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (checkPassword == false)
                return new Response<string>(HttpStatusCode.BadRequest, "Login or Password  is incorrect");

            var token = await GenerateJwtToken(user);
            return new Response<string>(token);
        }

        return new Response<string>(HttpStatusCode.BadRequest, "Login or Password  is incorrect");
    }

    //Method to generate The Token
    private async Task<string> GenerateJwtToken(IdentityUser user)
    {
        // key
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        // claims
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
        };

        //add roles
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        // token from all Informations
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token); //
    }
}