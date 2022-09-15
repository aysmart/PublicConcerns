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
using Microsoft.AspNetCore.Http;

namespace CourseWorkTwoNeyo.Controllers;

public class CausesController : Controller
{
    private readonly ILogger<CausesController> _logger;
    private readonly ApplicationDbContext _db;
    //private readonly databaseVM2 _db2;

    public CausesController(ILogger<CausesController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
       // _db2 = db2;
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

    public IActionResult updates(int? id)
    {
        var model = new databaseVM2();

        ViewBag.causeID = id;

        List<Updates> updatesList = _db.Updates.ToList();
        List<Causes> causeList = _db.Causes.ToList();
        List<Users> userList = _db.Users.ToList();
        List<Comments> commentList = _db.Comments.ToList();

        model.CausesUpdates = from i in updatesList
                               where i.CauseId == id
                               orderby i.PostDate descending
                               select new databaseVM2
                               {
                                   updatesList = i
                               };
        model.CausesStarted2 = (from i in commentList
                               join u in userList on i.UserId equals u.Id
                               where i.CauseId == id
                               orderby i.PostDate descending
                               select new databaseVM2
                               {
                                   commentList = i,
                                   userList = u
                               }).Take(3);
        ViewBag.updates = _db.Updates.Where(x => x.CauseId == id).ToList();

        return View(model);
    }

    //POST
    //Sign to cause
   // [HttpPost]
    //[ValidateAntiForgeryToken]
    public ActionResult signToCause(string signMsg, string CauseId)
    {
        var signMsg2 = signMsg;
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            //Redirrects to login page
            return Content("1");
        }

        if (signMsg2 != null)
        {
            Comments comments1 = new Comments
            {
                UserId = (int)HttpContext.Session.GetInt32("userID"),
                CauseId = Int32.Parse(CauseId),
                Content = signMsg2,
                LikeCount = 0
            };
            _db.Comments.Add(comments1);
        }

        Signatures signatures = new Signatures
        {
            UserId = (int)HttpContext.Session.GetInt32("userID"),
            CauseId = Int32.Parse(CauseId)
        };

        _db.Signatures.Add(signatures);
        var CauseUpdate = _db.Causes.FirstOrDefault(x => x.Id == Int32.Parse(CauseId));
        CauseUpdate.SigCount = CauseUpdate.SigCount+1;
        _db.SaveChanges();


        return Content("success");
    }


    public ActionResult commentToCause(string comMsg, string CauseId)
    {
        var comMsg2 = comMsg;
        if (HttpContext.Session.GetInt32("userID") == null)
        {
            //Redirrects to login page
            return Content("1");
        }

            Comments comments1 = new Comments
            {
                UserId = (int)HttpContext.Session.GetInt32("userID"),
                CauseId = Int32.Parse(CauseId),
                Content = comMsg2,
                LikeCount = 0
            };
            _db.Comments.Add(comments1);
        _db.SaveChanges();


        return Content("success");
    }
    //POST
    //Delete Comments Made By Self
    public IActionResult deleteComment(int? id)
    {
        if(id != null)
        {
            Comments comments = (from c in _db.Comments
                                 where c.Id == id
                                 select c).FirstOrDefault();
            if (comments != null)
            {
                _db.Comments.Remove(comments);
                _db.SaveChanges();
            }
        }
        var causeID = HttpContext.Session.GetInt32("causeID");
        return RedirectToAction("viewCause", new { id = causeID });
    }

    public IActionResult viewCause(int? id)
    {
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
        ViewBag.Signed = _db.Signatures.Where(x => x.UserId == HttpContext.Session.GetInt32("userID") && x.CauseId==id).ToList().Count();
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
