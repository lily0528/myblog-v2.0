using Microsoft.AspNet.Identity;
using MyBlog.Models;
using MyBlog.Models.Domain;
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
        public ActionResult CreateComment(string slug)
        {
            var blog = DbContext.Blogs
               .FirstOrDefault(p => p.Slug == slug);

            if (blog == null)
            {
                return HttpNotFound();
            }

            var model = new CreateCommentViewModel()
            // who dont use Comment Model?
            //InvalidOperationException: The model item passed into the dictionary is of type 'MyBlog.Models.Domain.Comment', but this dictionary requires a model item of type 'MyBlog.Models.ViewModels.CreateCommentViewModel'.
            //var model = new Comment()
            {
                BlogId = blog.Id
            };
            return PartialView("CreateComment", model);
        }

        [HttpPost]
        [Authorize]
        //Need get slug value form CreateComment.cshtml
        public ActionResult CreateComment(string slug, Comment formdata)
        {
            var userId = User.Identity.GetUserId();
            var blog = DbContext.Blogs.FirstOrDefault(
                p => p.Slug == "slug" && p.UserId == userId);
            if (userId == null)
            {
                return RedirectToAction(nameof(BlogController.DetailsByName));
            }
            var comment = new Comment();
            comment.BlogId = blog.Id;
            comment.UserId = userId;
            comment.Body = formdata.Body;
            comment.DateCreated = formdata.DateCreated;
            comment.DateUpdated = DateTime.Now;
            DbContext.Comments.Add(comment);
            DbContext.SaveChanges();
            //return RedirectToAction(nameof(BlogController.DetailsByName));
            return RedirectToAction("DetailsByname", "Blog", slug);
        }

        [HttpGet]
        public ActionResult CommentEdit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(BlogController.DetailsByName));
            }
            var comment= DbContext.Comments.FirstOrDefault(p => p.Id == id);
            return View();
        }
    }
}