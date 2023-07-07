using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public class HomeController : BaseController
{
    public HomeController()
    {
        
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("SecretData")]
    public string SecretData()
    {
        return "this data is secret";
    }
    
    
}