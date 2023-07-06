using Domain.Dtos;
using Domain.Wrapper;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public interface IAccountService
{
    Task<Response<IdentityResult>> RegisterAsync(UserRegisterDto model);
    Task<Response<IdentityUser>> LoginAsync(UserLoginDto model);
    
}