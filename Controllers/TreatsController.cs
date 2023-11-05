using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SweetSavoryTreats.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using System;
using SweetSavoryTreats.ViewModels;

namespace Treats.Controllers
{
  public class TreatsController : Controller
  {
    private readonly SweetSavoryTreatsContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public TreatsController(UserManager<ApplicationUser> userManager, SweetSavoryTreatsContext dbContext)
    {
      _userManager = userManager;
      _dbContext = dbContext;
    }

    public async Task<ActionResult> ViewAllTreats()
    {
      string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      ApplicationUser loggedInUser = await _userManager.FindByIdAsync(currentUserId);
      List<Treat> userTreats = _dbContext.Treats
          .Where(entry => entry.User.Id == loggedInUser.Id)
          .ToList();
      return View(userTreats);
    }

    [Authorize]
    public ActionResult CreateTreat()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> CreateTreatEntry(Treat newTreat)
    {
      if (!ModelState.IsValid)
      {
        ModelState.AddModelError("", "Please fix the errors and try again.");
        return View(newTreat);
      }
      else
      {
        string currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(currentUserId);
        newTreat.User = currentUser;
        _dbContext.Treats.Add(newTreat);
        _dbContext.SaveChanges();
        return RedirectToAction("ViewAllTreats");
      }
    }

    public ActionResult DisplayTreatDetails(int id)
    {
      Treat selectedTreat = _dbContext.Treats
          .Include(treat => treat.JoinEntities)
          .ThenInclude(join => join.Flavor)
          .FirstOrDefault(treat => treat.TreatId == id);
      return View(selectedTreat);
    }

    [Authorize]
    public ActionResult AddTasteToTreat(int id)
    {
      Treat selectedTreat = _dbContext.Treats.FirstOrDefault(treat => treat.TreatId == id);
      ViewBag.TasteId = new SelectList(_dbContext.Taste, "TasteId", "Type");
      return View(selectedTreat);
    }

    [Authorize]
    [HttpPost]
    public ActionResult AttachTaste(Treat treat, int tasteId)
    {
#nullable enable
      TasteTreat? joinEntity = _dbContext.TasteTreat.FirstOrDefault(join => join.TasteId == treat.TreatId && join.TasteId == TasteId);
#nullable disable
      if (tasteId != 0 && joinEntity == null)
      {
        _dbContext.TasteTreat.Add(new TasteTreat() { TreatId = treat.TreatId, TasteId = TasteId });
        _dbContext.SaveChanges();
      }
      return RedirectToAction("DisplayTreatDetails", new { id = treat.TreatId });
    }

    [HttpPost]
    public ActionResult RemoveJoin(int joinId)
    {
      if (!User.Identity.IsAuthenticated)
      {
        ErrorViewModel error = new ErrorViewModel();
        error.ErrorMessage = "You need to be logged in to do that.";
        TasteTreat joinEntry = _dbContext.TasteTreat.FirstOrDefault(entry => entry.TasteTreatId == joinId);
        int treatId = joinEntry.TreatId;
        Dictionary<string, object> model = new Dictionary<string, object>();
        model.Add("error", error);
        model.Add("treatId", treatId);
        return View("Error", model);
      }
      else
      {
        TasteTreat joinEntry = _dbContext.TasteTreat.FirstOrDefault(entry => entry.TasteTreatId == joinId);
        _dbContext.TasteTreat.Remove(joinEntry);
        _dbContext.SaveChanges();
        return RedirectToAction("DisplayTreatDetails", new { id = joinEntry.TreatId });
      }
    }

    [Authorize]
    public ActionResult EditTreat(int id)
    {
      Treat treatToModify = _dbContext.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(treatToModify);
    }

    [Authorize]
    [HttpPost]
    public ActionResult ModifyTreat(Treat modifiedTreat)
    {
      _dbContext.Treats.Update(modifiedTreat);
      _dbContext.SaveChanges();
      return RedirectToAction("DisplayTreatDetails", new { id = modifiedTreat.TreatId });
    }

    [Authorize]
    public ActionResult DeleteTreat(int id)
    {
      Treat treatToDelete = _dbContext.Treats.FirstOrDefault(treat => treat.TreatId == id);
      return View(treatToDelete);
    }

    [Authorize]
    [HttpPost, ActionName("DeleteTreat")]
    public ActionResult ConfirmDeleteTreat(int id)
    {
      Treat treatToRemove = _dbContext.Treats.FirstOrDefault(treat => treat.TreatId == id);
      _dbContext.Treats.Remove(treatToRemove);
      _dbContext.SaveChanges();
      return RedirectToAction("ViewAllTreats");
    }
  }
}
