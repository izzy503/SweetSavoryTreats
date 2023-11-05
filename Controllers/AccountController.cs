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
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, BakeryWithAuthContext dbContext)
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
    public async Task<ActionResult> RegisterUserAction(RegisterModel userModel)
    {
      if (!ModelState.IsValid)
      {
        return View(userModel);
      }
      else
      {
        User user = new User { UserName = userModel.Email };
        user.DisplayName = userModel.DisplayName;
        IdentityResult creationResult = await _userManager.CreateAsync(user, userModel.Password);
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
          return View(userModel);
        }
      }
    }

    public ActionResult LogInUser()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> LogInUserAction(LoginModel loginModel)
    {
      if (!ModelState.IsValid)
      {
        return View(loginModel);
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
          return View(loginModel);
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
