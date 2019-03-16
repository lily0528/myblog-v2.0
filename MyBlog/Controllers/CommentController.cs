using Microsoft.AspNet.Identity;
using MyBlog.Models;
using MyBlog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment

        private ApplicationDbContext DbContext;
        public CommentController()
        {
            DbContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CommentList(string slug)
        {
            var userId = User.Identity.GetUserId();
            var foundPost = DbContext.Blogs.FirstOrDefault(p => p.Slug == slug);

            var model = new ListCommentViewModel
            {
                Title = foundPost.Title,
                Slug = foundPost.Slug,
                //It is list from blog model
                Comments = foundPost.Comments.ToList()
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult CreatComment()
        {
              return View();
        }

        [HttpPost]
        public ActionResult CreatComment(string slug)
        {
            var userId = User.Identity.GetUserId();
            var blog = DbContext.Blogs.FirstOrDefault(
                p => p.Slug == slug && p.UserId == userId);
            if (blog == null)
            {
                return RedirectToAction(nameof(BlogController.DetailsByName));
            }
            var model = new CreateCommentViewModel();
            
            return View();
        }
    }
}