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

            var model = new ListCommentViewModel()
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
                p => p.Slug == slug && p.UserId == userId);
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
            //next one have problem
            //return RedirectToAction("DetailsByname", "Blog", slug); 
            return RedirectToAction("DetailsByname", "Blog", new { slug = blog.Slug });
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult CommentEdit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(BlogController.DetailsByName));
            }
            var comment = DbContext.Comments.FirstOrDefault(p => p.Id == id);
            var model = new EditCommentViewModel();
            model.UpdateReason = comment.UpdateReason;
            model.Body = comment.Body;

            return View(model);
        }

        [HttpPost]
        public ActionResult CommentEdit(int? id, EditCommentViewModel formdata)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(BlogController.DetailsByName));
            }
            Comment comment = new Comment();
            comment = DbContext.Comments.FirstOrDefault(p => p.Id == id);
            comment.Body = formdata.Body;
            comment.UpdateReason = formdata.UpdateReason;
            DbContext.Comments.Add(comment);
            DbContext.SaveChanges();
            return View();
         }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult CommentDel(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(BlogController.DetailsByName));
            }
            Comment comment = new Comment();
            comment = DbContext.Comments.FirstOrDefault(p => p.Id == id);
            string commentSlug = comment.Blog.Slug;
            DbContext.Comments.Remove(comment);
            DbContext.SaveChanges();

            return RedirectToAction("DetailsByname", "Blog", new { slug = commentSlug });
        }
    }


}