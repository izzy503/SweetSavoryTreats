using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SweetSavoryTreats.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using SweetSavoryTreats.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace SweetSavoryTreats.Controllers
{
  public class TasteController : Controller
  {
    private readonly SweetSavoryTreatsContext _dbContext;
    private readonly UserManager<User> _userManager;

    public TasteController(UserManager<User> userManager, SweetSavoryTreatsContext dbContext)
    {
      _userManager = userManager;
      _dbContext = dbContext;
    }

    public async Task<ActionResult> DisplayFlavors()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      User user = await _userManager.FindByIdAsync(userId);
      List<Taste> userTaste = _dbContext.Taste
          .Where(entry => entry.User.Id == user.Id)
          .ToList();
      return View(userTaste);
    }

    [Authorize]
    public ActionResult AddTasteForm()
    {
      return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateTaste(Taste taste)
    {
      if (!ModelState.IsValid)
      {
        ModelState.AddModelError("", "Please make corrections and try again.");
        return View(taste);
      }
      else
      {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        User user = await _userManager.FindByIdAsync(userId);
        taste.User = user;
        _dbContext.Taste.Add(taste);
        _dbContext.SaveChanges();
        return RedirectToAction("DisplayTaste");
      }
    }

    public ActionResult ShowTasteDetails(int id)
    {
      Taste selectedTaste = _dbContext.Taste
          .Include(taste => taste.JoinEntities)
          .ThenInclude(join => join.Treat)
          .FirstOrDefault(taste => taste.TasteId == id);
      return View(selectedTaste);
    }

    [Authorize]
    public ActionResult EditTasteForm(int id)
    {
      Taste tasteToEdit = _dbContext.Taste.FirstOrDefault(taste => taste.TasteId == id);
      return View(tasteToEdit);
    }

    [Authorize]
    [HttpPost]
    public ActionResult ModifyTaste(Taste modifiedTaste)
    {
      _dbContext.Taste.Update(modifiedTaste);
      _dbContext.SaveChanges();
      return RedirectToAction("ShowTasteDetails", new { id = modifiedTaste.TasteId });
    }

    [Authorize]
    public ActionResult RemoveTasteForm(int id)
    {
      Taste tasteToRemove = _dbContext.Taste.FirstOrDefault(taste => taste.TasteId == id);
      return View(tasteToRemove);
    }

    [Authorize]
    [HttpPost, ActionName("RemoveTasteForm")]
    public ActionResult DeleteTaste(int id)
    {
      Taste tasteToDelete = _dbContext.Taste.FirstOrDefault(taste => taste.TasteId == id);
      _dbContext.Taste.Remove(tasteToDelete);
      _dbContext.SaveChanges();
      return RedirectToAction("DisplayTaste");
    }
  }
}
