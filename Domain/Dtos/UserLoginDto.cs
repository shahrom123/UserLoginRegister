using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class UserLoginDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }  
    
}