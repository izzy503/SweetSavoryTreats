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

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, SweetSavoryTreatsContext dbContext)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _dbContext = dbContext;
    }

    public ActionResult Index()
    {
      return View();
    }

    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register(RegisterModels userModel)
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
          return RedirectToAction("Index");
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

    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginModel loginModel)
    {
      if (!ModelState.IsValid)
      {
        return View(loginModel);
      }
      else
      {
        Microsoft.AspNetCore.Identity.SignInResult loginResult = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, isPersistent: true, lockoutOnFailure: false);
        if (loginResult.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          ModelState.AddModelError("", "Your email or username appears to be incorrect. Please try again.");
          return View(loginModel);
        }
      }
    }

    [HttpPost]
    public async Task<ActionResult> LogOut()
    {
      await _signInManager.SignOutAsync();
      return RedirectToAction("Index");
    }
  }
}
