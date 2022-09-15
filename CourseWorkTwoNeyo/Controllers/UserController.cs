using System;
using System.Data; 
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CourseWorkTwoNeyo.Models;
using Microsoft.AspNetCore.Identity;

namespace CourseWorkTwoNeyo.Controllers;
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly databaseVM2 _data;

    public UserController(ILogger<UserController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public IActionResult Index()
    {
        var model = new databaseVM2();
        var UserId = HttpContext.Session.GetInt32("userID");
    //redirrects to login page if user is not signed in
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            return RedirectToAction("login", "Home");
        }
        else
        {

            List<Causes> causeList = _db.Causes.ToList();
            List<Users> userList = _db.Users.ToList();
            List<Signatures> signatureList = _db.Signatures.ToList();

            model.CausesSigned2 = from i in signatureList
                                  join e in causeList on i.CauseId equals e.Id
                                  join d in userList on e.UserId equals d.Id
                                  where i.UserId==UserId
                                select new databaseVM2
                                {
                                    causeList = e,
                                    signatureList = i,
                                    userList = d
                                };

            model.CausesStarted2 = from e in causeList
                                   join d in userList on e.UserId equals d.Id
                                   where e.UserId == UserId
                                   select new databaseVM2
                                   {
                                       causeList = e,
                                       userList = d
                                   };

         //using raw sql to get datarows into model
            model.CausesStarted = _db.Causes.FromSqlRaw("SELECT * FROM Causes Where UserId='" + UserId + "'  Order By PostDate DESC").ToList();
            model.CausesSigned = _db.Causes.FromSqlRaw("SELECT c.Id, c.UserId, c.CauseBanner, c.Title, c.Content, c.SigCount, c.ViewCount, c.ShareCount, c.PostDate, c.Targeted, c.Focus, c.Category, c.PlainContent, c.SigGoal FROM Causes c inner join Signatures s on c.Id=s.CauseId where s.UserId='" + UserId + "'  Order By PostDate DESC").ToList();
            model.userList2 = _db.Users.FromSqlRaw("SELECT * FROM Users Where Id='" + UserId + "'").ToList();

            return View(model);
        }
       
    }
    
    //GET
    public IActionResult causeDashboard(int? id )
    {
        IEnumerable<Causes> objCauseDashboard = _db.Causes;

        if (HttpContext.Session.GetInt32("userID") == null)
        {
            return RedirectToAction("login", "Home");
        }

        if (id==null || id == 0)
        {
            return NotFound();
        }


        var InfoCauseDashboard = objCauseDashboard.First(x => x.Id==id );

        if (InfoCauseDashboard == null)
        {
            return NotFound();
        }

        ViewBag.causeId = id;

        var model = new databaseVM2();
        List<Signatures> signatureList = _db.Signatures.ToList();
        List<Users> userList = _db.Users.ToList();


        model.signatureList2 = (from i in signatureList
                                join u in userList on i.UserId equals u.Id
                                where i.CauseId == id
                                orderby i.SignDate descending
                                select new databaseVM2
                                {
                                    signatureList = i,
                                    userList = u
                                }).Take(3);

        ViewBag.signature = model.signatureList2;
        ViewBag.cause = InfoCauseDashboard;
        return View(InfoCauseDashboard);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult register(Users obj)
    {

        if (ModelState.IsValid)
        {
            var passwordCheck = obj.UserPassword;
            var useremail = _db.Users.Where(v => v.UserEmail == obj.UserEmail);
            var username = _db.Users.Where(v => v.UserName == obj.UserName);

            if (useremail.Count() > 0) //<--Uniqueness validation
            {
                ModelState.AddModelError("UserEmail", "Email already exist"); //<-- Add error to the ModelState, that would be displayed in view.
                return View();
            }

            if (username.Count() > 0) //<--Uniqueness validation
            {
                ModelState.AddModelError("UserName", "Username already exist"); //<-- Add error to the ModelState, that would be displayed in view.
                return View();
            }
            _db.Users.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("login");
        }
        else
        {
            return View();
        }
    }

    public IActionResult changePassword(int? id)
    {
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            return RedirectToAction("login", "Home");
        }

        if (id == null || id == 0)
        {
            return NotFound();
        }
        var userInfo = _db.Users.Find(id);
        if (userInfo == null)
        {
            return NotFound();
        }

        return View(userInfo);
    }


    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult changePassword(Users obj)
    {

        if (ModelState.IsValid)
        {
            _db.Users.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("settings", new { id = obj.Id });
        }
        else
        {
            return View();
        }
    }


    public IActionResult comments(int? id)
    {
        var model = new databaseVM2();

        ViewBag.causeID = id;

        List<Causes> causeList = _db.Causes.ToList();
        List<Users> userList = _db.Users.ToList();
        List<Signatures> signatureList = _db.Signatures.ToList();
        List<Comments> commentList = _db.Comments.ToList();

        model.CausesStarted2 = (from i in commentList
                                join u in userList on i.UserId equals u.Id
                                where i.CauseId == id
                                orderby i.PostDate descending
                                select new databaseVM2
                                {
                                    commentList = i,
                                    userList = u
                                }).Take(4);
        return View(model);
    }

    public IActionResult disableAccount(int? id)
    {
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            return RedirectToAction("login", "Home");
        }

        if (id == null || id == 0)
        {
            return NotFound();
        }
        var userInfo = _db.Users.Find(id);
        if (userInfo == null)
        {
            return NotFound();
        }

        return View(userInfo);
    }


    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult disableAccount(Users user)
    {
        var currentPerson = _db.Users.FirstOrDefault(p => p.Id == user.Id);
        if (currentPerson == null)
            return NotFound();
        currentPerson.Disabled = user.Disabled;
            _db.SaveChanges();
            return RedirectToAction("settings", new { id = user.Id });
    }
    //GET
    public IActionResult editCause(int? id)
    {
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            return RedirectToAction("login", "Home");
        }

        if (id == null || id == 0)
        {
            return NotFound();
        }
        var causeInfo = _db.Causes.Find(id);
        if (causeInfo == null)
        {
            return NotFound();
        }
        ViewBag.causeId = id;

        return View(causeInfo);
    }


    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult editCause(Causes obj)
    {
        //assign row to be eddited to a variable
        var causes = _db.Causes.Find(obj.Id);
        //edit the row with contents from the view
        causes.CauseBanner = obj.CauseBanner;
        causes.Title = obj.Title;
        causes.Content = obj.Content;
        causes.Targeted = obj.Targeted;
        causes.Focus = obj.Focus;
        //update the information
        _db.SaveChanges();
        return RedirectToAction("causeDashboard", new { id = obj.Id });
    }

    //logout
    public IActionResult logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("login", "Home");
    }

    public IActionResult newUpdate(int? id)
    {
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            return RedirectToAction("login", "Home");
        }
        ViewBag.causeId = id;
        return View();
    }


    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult newUpdate(Updates obj)
    {
        ///var c = obj.UserPhoto;

        if (ModelState.IsValid)
        {
            Updates updates = new Updates
            {
                CauseId = obj.CauseId,
                Content = obj.Content,
                Headline = obj.Headline
            };

            _db.Updates.Add(updates);
            _db.SaveChanges();
            return RedirectToAction("causeDashboard");
        }
        else
        {
            return View();
        }
    }

    //GET
    public IActionResult profileEdit(int? id)
    {
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            return RedirectToAction("login", "Home");
        }

        if (id==null || id==0)
        {
            return NotFound();
        }
       var userInfo = _db.Users.Find(id);
        if (userInfo == null)
        {
            return NotFound();
        }

        return View(userInfo);
    }


    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult profileEdit(Users obj)
    {

        if (ModelState.IsValid)
        {
            _db.Users.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("settings", new {id=obj.Id});
        }
        else
        {
            return View();
        }
    }


    //GET
    public IActionResult settings(int? id)
    {
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            return RedirectToAction("login", "Home");
        }
        IEnumerable<Users> objUserSettings = _db.Users;

        if (id == null || id == 0)
        {
            return NotFound();
        }

        var InfoUserSettings = objUserSettings.First(x => x.Id == id);

        if (InfoUserSettings == null)
        {
            return NotFound();
        }

        return View(InfoUserSettings);
    }

    //GET
    public IActionResult victory(int? id)
    {
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            return RedirectToAction("login", "Home");
        }

        var model = new databaseVM2();

        List<Causes> causeList = _db.Causes.ToList();
        List<Comments> commentList = _db.Comments.ToList();

        model.CausesSigned2 = from i in causeList
                              where i.Id == id
                              select new databaseVM2
                              {
                                  causeList = i
                              };

        ViewBag.cause = model.CausesSigned2.ToList();
        ViewBag.comCount = _db.Comments.Where(y => y.CauseId == id).Count();
        ViewBag.causeId = id;
        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult declareVictory(int? id)
    {
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            return RedirectToAction("login", "Home");
        }
        var cause = _db.Causes.FirstOrDefault(p => p.Id == id);
        if (cause == null)
            return NotFound();
        cause.Category = "Victory";
        _db.SaveChanges();
        return RedirectToAction("causeDashboard", new { id = id });
    }


    //GET
    public IActionResult viewCause(int? id)
    {
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            return RedirectToAction("login", "Home");
        }
        IEnumerable<Causes> objCauseDashboard = _db.Causes;
        var model = new databaseVM2();

        if (id == null || id == 0)
        {
            return NotFound();
        }

        var CauseView = _db.Causes.FirstOrDefault(x => x.Id == id);
        CauseView.ViewCount = CauseView.ViewCount + 1;
        _db.SaveChanges();
        var InfoCauseDashboard = objCauseDashboard.First(x => x.Id == id);

        if (InfoCauseDashboard == null)
        {
            return NotFound();
        }

        List<Causes> causeList = _db.Causes.ToList();
        List<Users> userList = _db.Users.ToList();
        List<Signatures> signatureList = _db.Signatures.ToList();
        List<Comments> commentList = _db.Comments.ToList();

        model.CausesSigned2 = from i in causeList
                              join d in userList on i.UserId equals d.Id
                              where i.Id == id
                              select new databaseVM2
                              {
                                  causeList = i,
                                  userList = d
                              };

        model.CausesStarted2 = from i in commentList
                               join u in userList on i.UserId equals u.Id
                               where i.CauseId == id
                               orderby i.PostDate descending
                               select new databaseVM2
                               {
                                   commentList = i,
                                   userList = u
                               };

        model.signatureList2 = (from i in signatureList
                                join u in userList on i.UserId equals u.Id
                                where i.CauseId == id
                                orderby i.SignDate descending
                                select new databaseVM2
                                {
                                    signatureList = i,
                                    userList = u
                                }).Take(3);

        ViewBag.userID = HttpContext.Session.GetInt32("userID");
        ViewBag.causeID = id;
        ViewBag.Signed = _db.Signatures.Where(x => x.UserId == HttpContext.Session.GetInt32("userID") && x.CauseId == id).ToList().Count();
        //model.CausesStarted.Skip(30).Take(40);
        HttpContext.Session.SetInt32("causeID", (int)id);

        //Console.WriteLine(model.userID);
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
