using Microsoft.AspNetCore.Mvc;
using SweetSavoryTreats.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace SweetSavoryTreats.Controllers
{
  public class HomeController : Controller
  {
    private readonly SweetSavoryTreatsontext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(UserManager<ApplicationUser> userManager, BakeryWithAuthContext context)
    {
      _userManager = userManager;
      _context = context;
    }

    [HttpGet("/")]
    public async Task<ActionResult> Homepage()
    {
      var availableTreats = _context.Treats.ToArray();
      var availableFlavors = _context.Taste.ToArray();
      var viewData = new Dictionary<string, object>();
      viewData.Add("treats", availableTreats);
      viewData.Add("Taste", availableTaste);
      string currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser currentUser = await _userManager.FindByIdAsync(currentUserId);
      return View(viewData);
    }
  }
}
