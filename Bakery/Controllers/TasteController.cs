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

    public async Task<ActionResult> Index()
    {
      string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      User user = await _userManager.FindByIdAsync(userId);
      List<Taste> userTaste = _dbContext.Tastes
          .Where(entry => entry.User.Id == user.Id)
          .ToList();
      return View(userTaste);
    }

    [Authorize]
    public ActionResult Create()
    {
      return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Create(Taste taste)
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
        _dbContext.Tastes.Add(taste);
        _dbContext.SaveChanges();
        return RedirectToAction("Index");
      }
    }

    public ActionResult Details(int id)
    {
      Taste selectedTaste = _dbContext.Tastes
          .Include(taste => taste.JoinEntities)
          .ThenInclude(join => join.Treat)
          .FirstOrDefault(taste => taste.TasteId == id);
      return View(selectedTaste);
    }

    [Authorize]
    public ActionResult Edit(int id)
    {
      Taste tasteToEdit = _dbContext.Tastes.FirstOrDefault(taste => taste.TasteId == id);
      return View(tasteToEdit);
    }

    [Authorize]
    [HttpPost]
    public ActionResult Edit(Taste modifiedTaste)
    {
      _dbContext.Tastes.Update(modifiedTaste);
      _dbContext.SaveChanges();
      return RedirectToAction("Details", new { id = modifiedTaste.TasteId });
    }

    [Authorize]
    public ActionResult Delete(int id)
    {
      Taste tasteToRemove = _dbContext.Tastes.FirstOrDefault(taste => taste.TasteId == id);
      return View(tasteToRemove);
    }

    [Authorize]
    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Taste tasteToDelete = _dbContext.Tastes.FirstOrDefault(taste => taste.TasteId == id);
      _dbContext.Tastes.Remove(tasteToDelete);
      _dbContext.SaveChanges();
      return RedirectToAction("Index");
    }
    [Authorize]
    public ActionResult AddTreat(int id)
    {
      Taste toAdd = _dbContext.Tastes.FirstOrDefault(taste  => taste.TasteId == id);
      ViewBag.TreatId = new SelectList(_dbContext.Tastes, "TasteId", "Type");
      return View(toAdd);
    }

    [Authorize]
    [HttpPost]
    public ActionResult AddTreat(Taste taste, int treatId)
    {
      #nullable enable
      TasteTreat? joinEntity = _dbContext.TasteTreat.FirstOrDefault(join => join.TasteId == treatId && join.TasteId == taste.TasteId);
      #nullable disable
      if (taste.TasteId != 0 && joinEntity == null)
      {
        _dbContext.TasteTreat.Add(new TasteTreat() { TreatId = treatId, TasteId = taste.TasteId });
        _dbContext.SaveChanges();
      }
      return RedirectToAction("Details", new { id = treatId });
    }

    [HttpPost]
    public ActionResult RemoveJoin(int joinId)
    {
      if (!User.Identity.IsAuthenticated)
      {
        ErrorModel error = new ErrorModel();
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
        return RedirectToAction("Details", new { id = joinEntry.TreatId });
      }
    }
  }
}
