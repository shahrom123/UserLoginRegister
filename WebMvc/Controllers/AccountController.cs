using Domain.Dtos;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebMvc.Controllers;

public class AccountController:Controller
{
   private readonly UserManager<IdentityUser> _userManager;
   private readonly SignInManager<IdentityUser> _signInManager;
   private readonly IAccountService _accountService;

   public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IAccountService accountService)
   {
      _userManager = userManager;
      _signInManager = signInManager;
      _accountService = accountService;
   }
   public IActionResult Index()
   {
      return View(); 
   }
   [HttpGet]
   public IActionResult Register()
   {
      return View(new UserRegisterDto());
   }

   public IActionResult Login(string? returnUrl)
   {
      return View(new UserLoginDto()
      {
         ReturnUrl = returnUrl
      });
   }

   [HttpGet]
   public async Task<IActionResult> LoginAsync(UserLoginDto model)
   {
      if (!ModelState.IsValid)
      {
         return View(model);
      }

      if (ModelState.IsValid)
      {
         await _accountService.LoginAsync(model);
         return RedirectToAction("Index", "Home");
      }

      if (string.IsNullOrEmpty(model.ReturnUrl) == true)
      {
         return RedirectToAction("Index");
      }
      return View(model);
   }
   [HttpPost]
   public async Task<IActionResult> Register(UserRegisterDto model)
   {
      if (!ModelState.IsValid)
      {
         return View(model); 
      } 
      if(ModelState.IsValid)
      {
         await _accountService.RegisterAsync(model);
         return RedirectToAction("Login"); 
      }
      return View(model); 
   }
   
   public async Task<IActionResult> LogOutAsync()
   {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index", "Home"); 
   }

}