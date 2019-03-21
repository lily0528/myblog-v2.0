using Microsoft.AspNet.Identity;
using MyBlog.Models;
using MyBlog.Models.Domain;
using MyBlog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        [HttpGet]
        public ActionResult CommentList(string slug)
        {
            var foundBlog = DbContext.Blogs.FirstOrDefault(p => p.Slug == slug);
            if (foundBlog == null)
            {
                return HttpNotFound();
            }
            var model = new ListCommentViewModel()
            {
                Title = foundBlog.Title,
                Slug = foundBlog.Slug,
                Comments = foundBlog.Comments.ToList()
            };
            return View(model);
        }

        [HttpGet]
        [Authorize]
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
        public ActionResult CreateComment(string slug, CreateCommentViewModel formdata)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                RedirectToAction(nameof(HomeController.Index));
            }

            var userId = User.Identity.GetUserId();
            var blog = DbContext.Blogs.FirstOrDefault(
                p => p.Slug == slug);
            if (blog == null)
            {
                return HttpNotFound();
            }

            var comment = new Comment
            {
                BlogId = blog.Id,
                UserId = userId,
                Body = formdata.Body,
                DateCreated = formdata.DateCreated
            };
            if (!string.IsNullOrWhiteSpace(comment.Body))
            {
                DbContext.Comments.Add(comment);
                DbContext.SaveChanges();
                return RedirectToAction("Details", "Blog", new { slug = blog.Slug });
            }
            else
            {
                return View();
            }
        }

        [HttpGet]

        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult CommentEdit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comment = DbContext.Comments.FirstOrDefault(p => p.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            var model = new EditCommentViewModel();
            model.UpdateReason = comment.UpdateReason;
            model.Body = comment.Body;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult CommentEdit(int? id, EditCommentViewModel formdata)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comment = DbContext.Comments.FirstOrDefault(p => p.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            comment.Body = formdata.Body;

            if (!string.IsNullOrWhiteSpace(formdata.UpdateReason))
            {
                comment.UpdateReason = formdata.UpdateReason;
                comment.DateUpdated = DateTime.Now;
                DbContext.SaveChanges();
            }
            else
            {
                return View();
            }
            return RedirectToAction("Details", "Blog", new { slug = comment.Blog.Slug });
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public ActionResult CommentDel(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comment = DbContext.Comments.FirstOrDefault(p => p.Id == id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            string commentSlug = comment.Blog.Slug;
            DbContext.Comments.Remove(comment);
            DbContext.SaveChanges();

            return RedirectToAction("Details", "Blog", new { slug = commentSlug });
        }
    }
}