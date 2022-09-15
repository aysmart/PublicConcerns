using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CourseWorkTwoNeyo.Models;

namespace CourseWorkTwoNeyo.Controllers;

public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly ApplicationDbContext _db;

    public AdminController(ILogger<AdminController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult dashboard()
    {
        if (HttpContext.Session.GetInt32("adminID") == null)
        {
            return RedirectToAction("login", "Admin");

        }
        var model = new databaseVM2();

        List<Causes> causeList = _db.Causes.ToList();
        List<Users> userList = _db.Users.ToList();
        List<Signatures> signatureList = _db.Signatures.ToList();
        List<Comments> commentList = _db.Comments.ToList();

        model.CausesStarted2 = (from i in userList
                                join u in causeList on i.Id equals u.UserId
                                orderby u.PostDate descending
                                select new databaseVM2
                                {
                                    userList = i,
                                    causeList = u
                                });

        ViewBag.causeCount = causeList.Count();
        ViewBag.userCount = userList.Count();
        ViewBag.victoryCount = causeList.Where(x=> x.Category=="Victory").Count();
        ViewBag.disabledCount = userList.Where(x => x.Disabled == 1).Count();
        ViewBag.table = model.CausesStarted2;
        return View();
    }

    //Delete Causes By Admin
    /*[HttpPost]
    [ValidateAntiForgeryToken]*/
    public IActionResult deleteCause(string CauseId)
    {
        if (HttpContext.Session.GetInt32("adminID") == null)
        {
            //Redirrects to login page
            return Content("1");
        }
        var obj = _db.Causes.Find(Int32.Parse(CauseId));
        if (obj==null)
        {
            return NotFound();
        }

        _db.Causes.Remove(obj);
        _db.SaveChanges();
        return Content("success");
    }

    public IActionResult login()
    {
        if (HttpContext.Session.GetInt32("adminID") == null)
        {
            return View();

        }
        else
        {
            return RedirectToAction("dashboard", "Admin");
        }
    }

    //POST -> Login
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult login(Admins obj)
    {
        var passwordCheck = obj.AdminPassword;
        var authenticate = _db.Admins.Where(v => v.AdminEmail == obj.AdminEmail && v.AdminPassword == obj.AdminPassword).FirstOrDefault();

        if (authenticate == null) //<--Uniqueness validation
        {
            ModelState.AddModelError("AdminEmail", "Your Email or Password is incorrect"); //<-- Add error to the ModelState, that would be displayed in view.
            return View();
        }
        else
        {
            HttpContext.Session.SetInt32("adminID", authenticate.Id);
            HttpContext.Session.SetString("adminName", authenticate.Name);
            var AdminID = ViewBag.adminID = HttpContext.Session.GetInt32("adminID");

            //HttpContext.Session.SetString("userName", obj.Name);
            return RedirectToAction("dashboard", "Admin", obj.Id);
        }


    }
    public IActionResult userAccounts()
    {
        if (HttpContext.Session.GetInt32("adminID") == null)
        {
            return RedirectToAction("login", "Admin");

        }
        var model = new databaseVM2();

        List<Causes> causeList = _db.Causes.ToList();
        List<Users> userList = _db.Users.ToList();
        List<Signatures> signatureList = _db.Signatures.ToList();
        List<Comments> commentList = _db.Comments.ToList();

        model.CausesStarted2 = (from i in userList
                                orderby i.Name ascending
                                select new databaseVM2
                                {
                                    userList = i
                                    
                                });

        ViewBag.causeCount = causeList.Count();
        ViewBag.userCount = userList.Count();
        ViewBag.victoryCount = causeList.Where(x => x.Category == "Victory").Count();
        ViewBag.disabledCount = userList.Where(x => x.Disabled == 1).Count();
        ViewBag.table = model.CausesStarted2;
        return View();
    }

    //For deleting and disabling user accounts
    public IActionResult ddAccount(string? actions, int? userId)
    {
        if (HttpContext.Session.GetInt32("adminID") == null)
        {
            //Redirrects to login page
            return Content("1");
        }

        if (actions=="disable")
        {
            var currentPerson = _db.Users.FirstOrDefault(p => p.Id == userId);
            if (currentPerson == null)
                return NotFound();
            currentPerson.Disabled = 1;
            _db.SaveChanges();
            return Content("2");
        }

        if(actions=="delete")
        {
            var obj = _db.Users.Find(userId);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Users.Remove(obj);
            _db.SaveChanges();
            return Content("3");
        }
        return View();
    }

    //logout
    public IActionResult logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
