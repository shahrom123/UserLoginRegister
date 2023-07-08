using Domain.Dtos;
using Domain.Wrapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class AccountController:BaseController
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("Login")]
    public async Task<Response<string>> Login([FromBody]JWTLoginDto login)
    {
        return await _accountService.JWTLogin(login); 
    }

    [HttpPost("Register")]
    public async Task<Response<IdentityResult>> Register([FromBody]UserRegisterDto model)
    {
        return await _accountService.RegisterAsync(model); 
    }
    
}