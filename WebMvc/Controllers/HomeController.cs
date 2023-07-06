using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMvc.Models;

namespace WebMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    } 
   

    [Authorize(Roles = "Admin")]
    public IActionResult Secret()
    {
        return View();
    }

    [Authorize(Roles = "User")]
    public IActionResult OnlyManager()
    {
        return View();
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
 
}
