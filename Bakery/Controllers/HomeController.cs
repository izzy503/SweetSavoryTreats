using Microsoft.AspNetCore.Mvc;
using SweetSavoryTreats.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using SweetSavoryTreats;

namespace SweetSavoryTreats.Controllers
{
  public class HomeController : Controller
  {
    private readonly SweetSavoryTreatsContext _context;
    private readonly UserManager<User> _userManager;

    public HomeController(UserManager<User> userManager, SweetSavoryTreatsContext context)
    {
      _userManager = userManager;
      _context = context;
    }

    [HttpGet("/")]
    public async Task<ActionResult> Homepage()
    {
      var availableTreats = _context.Treats.ToArray();
      var availableTaste = _context.Taste.ToArray();
      var viewData = new Dictionary<string, object>();
      viewData.Add("treats", availableTreats);
      viewData.Add("taste", availableTaste);
      string currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      User currentUser = await _userManager.FindByIdAsync(currentUserId);
      return View(viewData);
    }
  }
}
