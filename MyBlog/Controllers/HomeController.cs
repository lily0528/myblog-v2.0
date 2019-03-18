using Facebook;
using Microsoft.AspNet.Identity;
using MyBlog.Models;
using MyBlog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext DbContext;
        public HomeController()
        {
            DbContext = new ApplicationDbContext();
        }
        public ActionResult Index(string search)
        {
            var isAdmin = User.IsInRole("Admin");
            //if (!string.IsNullOrWhiteSpace(search))
            //{
                var model = DbContext.Blogs
                    .Where(p => isAdmin ? true : p.Published && (p.Title.Contains(search) ||
                                p.Body.Contains(search) ||
                                p.Slug.Contains(search)))
                    .Select(p => new HomeBlogViewModel
                    {
                        Id = p.Id,
                        Title = p.Title,
                        Body = p.Body,
                        Published = p.Published,
                        MediaUrl = p.MediaUrl,
                        DateCreated = p.DateCreated,
                        DateUpdated = p.DateUpdated,
                        Slug = p.Slug
                    }).ToList();
                return View(model);
            //}
            //else
            //{
            //    return View();
            //}
        }

        //public ActionResult Search()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Search(string search)
        //{
        //    if (!string.IsNullOrWhiteSpace(search))
        //    {
        //        var model = DbContext.Blogs
        //            .Where(p => p.Title.Contains(search) ||
        //                        p.Body.Contains(search) ||
        //                        p.Slug.Contains(search))
        //            .Select(p => new HomeBlogViewModel
        //            {
        //                Id = p.Id,
        //                Title = p.Title,
        //                Body = p.Body,
        //                Published = p.Published,
        //                MediaUrl = p.MediaUrl,
        //                DateCreated = p.DateCreated,
        //                DateUpdated = p.DateUpdated,
        //                Slug = p.Slug
        //            }).ToList();
        //        return View(model);
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}

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