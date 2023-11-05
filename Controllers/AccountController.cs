using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SweetSavoryTreats.Models;
using System.Threading.Tasks;
using SweetSavoryTreats.ViewModels;

namespace SweetSavoryTreats.Controllers
{
  public class AccountController : Controller
  {
    private readonly SweetSavoryTreatsContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, BakeryWithAuthContext dbContext)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _dbContext = dbContext;
    }

    public ActionResult Home()
    {
      return View();
    }

    public IActionResult RegisterUser()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> RegisterUserAction(RegisterViewModel userViewModel)
    {
      if (!ModelState.IsValid)
      {
        return View(userViewModel);
      }
      else
      {
        ApplicationUser user = new ApplicationUser { UserName = userViewModel.Email };
        user.DisplayName = userViewModel.DisplayName;
        IdentityResult creationResult = await _userManager.CreateAsync(user, userViewModel.Password);
        if (creationResult.Succeeded)
        {
          return RedirectToAction("Home");
        }
        else
        {
          foreach (IdentityError error in creationResult.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
          return View(userViewModel);
        }
      }
    }

    public ActionResult LogInUser()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> LogInUserAction(LoginViewModel loginViewModel)
    {
      if (!ModelState.IsValid)
      {
        return View(loginViewModel);
      }
      else
      {
        Microsoft.AspNetCore.Identity.SignInResult loginResult = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, isPersistent: true, lockoutOnFailure: false);
        if (loginResult.Succeeded)
        {
          return RedirectToAction("Home");
        }
        else
        {
          ModelState.AddModelError("", "Your email or username appears to be incorrect. Please try again.");
          return View(loginViewModel);
        }
      }
    }

    [HttpPost]
    public async Task<ActionResult> LogOutUser()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Home");
    }
  }
}
