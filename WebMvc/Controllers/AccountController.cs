using Domain.Dtos;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebMvc.Controllers;

public class AccountController:Controller
{
   private readonly SignInManager<IdentityUser> _signInManager;
   private readonly IAccountService _accountService;

   public AccountController(SignInManager<IdentityUser> signInManager, IAccountService accountService)
   {
      _signInManager = signInManager;
      _accountService = accountService;
   }
   public IActionResult Index()
   {
      return View(); 
   }

   public IActionResult AccessDenied() => View();
  
   [HttpGet]
   public IActionResult Register()
   {
      return View(new UserRegisterDto());
   }
   
   [HttpPost]
   public async Task<IActionResult> Register(UserRegisterDto model)
   {
      if (!ModelState.IsValid)
      {
         return View(model); 
      } 
      var response=  await _accountService.RegisterAsync(model);
      
       if(response.StatusCode == System.Net.HttpStatusCode.OK)
       {
         return RedirectToAction("Login"); 
       }
       else
       {
          ModelState.AddModelError("Password",response.Errors.FirstOrDefault());
          return View(model); 
       }
     
   }
   [HttpGet]
   public IActionResult Login(string? returnUrl)
   {
      return View(new UserLoginDto()
      {
         ReturnUrl = returnUrl
      });
   }

   [HttpPost]
   public async Task<IActionResult> LoginAsync(UserLoginDto model)
   {
      if (!ModelState.IsValid)return View(model);  
         var response = await _accountService.LoginAsync(model);
      if (response.StatusCode == System.Net.HttpStatusCode.OK)
      {
         if (string.IsNullOrEmpty(model.ReturnUrl) == true)
         {
            return RedirectToAction("Index");
         }
         else
         {
            return Redirect(model.ReturnUrl);
         }
      }
      else
      {
         ModelState.AddModelError("ConfirmPassword",response.Errors.FirstOrDefault());
         return View(model);
      }
   }


   public async Task<IActionResult> LogOutAsync()
   {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index", "Home"); 
   }

}