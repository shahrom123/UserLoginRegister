using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class HomeController:BaseController
{
    public HomeController()
    {
        
    }
    
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
    [HttpGet("SecretData")]
    // Method for testing
    public IActionResult SecretData()
    {
        return Ok("Secret Data");  
    } 
}
