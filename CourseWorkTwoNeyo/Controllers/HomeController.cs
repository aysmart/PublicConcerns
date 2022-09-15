    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using CourseWorkTwoNeyo.Models;
    using CourseWorkTwoNeyo.Data;
    namespace CourseWorkTwoNeyo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }


    //GET
    public IActionResult Index()
    {
        IEnumerable<Causes> objLogin = _db.Causes;
        var model = new databaseVM2();

        List<Causes> causeList = _db.Causes.ToList();
        List<Users> userList = _db.Users.ToList();
        List<Signatures> signatureList = _db.Signatures.ToList();
        List<Comments> commentList = _db.Comments.ToList();

        //Selecting Causes that have not gotten victory
        model.CausesSigned2 = from i in causeList
                              join d in userList on i.UserId equals d.Id
                              where i.Category == "Trending" || i.Category == "Recent"
                              orderby i.PostDate descending
                              select new databaseVM2
                              {
                                  causeList = i,
                                  userList = d
                              };

        model.CausesStarted2 = (from i in causeList
                                join d in userList on i.UserId equals d.Id
                                where i.Category == "Victory"
                                orderby i.PostDate descending
                                select new databaseVM2
                                {
                                    causeList = i,
                                    userList = d
                                }).Take(9);

        ViewBag.victory = model.CausesStarted2.ToList();
        ViewBag.causes = model.CausesSigned2.ToList();
        return View(objLogin);
    }

/*    [HttpPost]
    public JsonResult AjaxMethod(int pageIndex)
    {
       *//*
        var model = new databaseVM2();

        model.PageIndex = pageIndex;
        model.PageSize = 10;
        model.RecordCount = _db.Causes.Count();
        int startIndex = (pageIndex - 1) * model.PageSize;
        model.Customers = (from customer in entities.Customers
                           select customer)
                         .OrderBy(customer => customer.CustomerID)
                         .Skip(startIndex)
                         .Take(model.PageSize).ToList();
        return Json(model);*//*
    }*/

    //GET
    public IActionResult causes()
    {
        var model = new databaseVM2();

        List<Causes> causeList = _db.Causes.ToList();
        List<Users> userList = _db.Users.ToList();
        List<Signatures> signatureList = _db.Signatures.ToList();
        List<Comments> commentList = _db.Comments.ToList();

        //Selecting Causes that are trending
        model.CausesSigned2 = from i in causeList
                              join d in userList on i.UserId equals d.Id
                              where i.Category == "Trending"
                              orderby i.PostDate descending
                              select new databaseVM2
                              {
                                  causeList = i,
                                  userList = d
                              };
        //Selecting Causes that are recent

        model.signatureList2 = from i in causeList
                              join d in userList on i.UserId equals d.Id
                              where i.Category == "Recent"
                              orderby i.PostDate descending
                              select new databaseVM2
                              {
                                  causeList = i,
                                  userList = d
                              };
        //Selecting Causes that are victorious
        model.CausesStarted2 = (from i in causeList
                                join d in userList on i.UserId equals d.Id
                                where i.Category == "Victory"
                                orderby i.PostDate descending
                                select new databaseVM2
                                {
                                    causeList = i,
                                    userList = d
                                });

        ViewBag.victory = model.CausesStarted2.ToList();
        ViewBag.trending = model.CausesSigned2.ToList();
        ViewBag.recent = model.signatureList2.ToList();

        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult causes(Causes obj)
    {
        if (ModelState.IsValid)
        {
        _db.Causes.Add(obj);
        _db.SaveChanges();
         return View();
        }
        else
        {
            return View();
        }
    }

    //GET
     public IActionResult login()
    {
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            return View();
            
        }
        else
        {
            return RedirectToAction("Index", "User");
        }
    }

    //Used a Video to get clarity
    //
    //POST -> Login

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult login(Users obj)
    {
            var passwordCheck = obj.UserPassword;
            var authenticate = _db.Users.Where(v => v.UserEmail == obj.UserEmail && v.UserPassword == obj.UserPassword).FirstOrDefault();
        //var username = _db.Users.Where(v => v.UserName == obj.UserName);

        if (authenticate == null) //<--Uniqueness validation
        {
            ModelState.AddModelError("UserEmail", "Your Email or Password is incorrect"); //<-- Add error to the ModelState, that would be displayed in view.
            return View();
        }
        else
        {
            HttpContext.Session.SetInt32("userID", authenticate.Id);
            HttpContext.Session.SetString("userName", authenticate.Name);
            var UserID = ViewBag.userID = HttpContext.Session.GetInt32("userID");
           
            //HttpContext.Session.SetString("userName", obj.Name);
            return RedirectToAction("Index", "User", obj.Id);
        }
      
        
    }




    //GET
    public IActionResult register()
    {
        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult register(Users obj)
    {
        ///var c = obj.UserPhoto;

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

    //GET
    public IActionResult search()
    {
        var model = new databaseVM2();

        List<Causes> causeList = _db.Causes.ToList();
        List<Users> userList = _db.Users.ToList();
        List<Signatures> signatureList = _db.Signatures.ToList();
        List<Comments> commentList = _db.Comments.ToList();

        //Selecting Causes that are trending
        model.CausesSigned2 = from i in causeList
                              join d in userList on i.UserId equals d.Id
                              orderby i.PostDate descending
                              select new databaseVM2
                              {
                                  causeList = i,
                                  userList = d
                              };
  
        ViewBag.searchResult = model.CausesSigned2.ToList();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult search(Causes obj)
    {
        var model = new databaseVM2();

        List<Causes> causeList = _db.Causes.ToList();
        List<Users> userList = _db.Users.ToList();
        List<Signatures> signatureList = _db.Signatures.ToList();
        List<Comments> commentList = _db.Comments.ToList();

        //Selecting Causes that are trending
        model.CausesSigned2 = from i in causeList
                              join d in userList on i.UserId equals d.Id
                              orderby i.PostDate descending
                              select new databaseVM2
                              {
                                  causeList = i,
                                  userList = d
                              };
        string contain;

        if (obj.Title==null)
        {
            contain = " ";
        }
        else
        {
            contain = obj.Title;
        }
        ViewBag.searchResult = model.CausesSigned2
            .Where(e => e.causeList.Title.Contains(contain))
            .ToList();

        return View();
    }

     public IActionResult startACause()
    {
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            return RedirectToAction("login", "Home");
        }
        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult startACause(Causes obj)
    {

        if (ModelState.IsValid)
        {
            _db.Causes.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("causedashboard", "User");
        }
        else
        {
            return View();
        }       
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
