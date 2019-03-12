using Microsoft.AspNet.Identity;
using MyBlog.Models;
using MyBlog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext DbContext;
        public HomeController()
        {
            DbContext = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            //var userId = User.Identity.GetUserId();
            var model = DbContext.Blogs
                //.Where(p => p.UserId == userId)
                .Select(p => new HomeBlogViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Body = p.Body,
                    Published = p.Published,
                    MediaUrl = p.MediaUrl,
                    DateCreated = p.DateCreated,
                    DateUpdated = p.DateUpdated
                }).ToList();
            return View(model);
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}