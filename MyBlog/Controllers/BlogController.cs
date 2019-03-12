using Microsoft.AspNet.Identity;
using MyBlog.Models;
using MyBlog.Models.Domain;
using MyBlog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constants = MyBlog.Models.Constants;

namespace MyBlog.Controllers
{
    public class BlogController : Controller
    {
        // GET: Blog
        private ApplicationDbContext DbContext;
        public BlogController()
        {
            DbContext = new ApplicationDbContext();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();

        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BlogList()
        {
            var userId = User.Identity.GetUserId();
            var model = DbContext.Blogs
                .Where(p => p.UserId == userId)
                .Select(p => new IndexBlogViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Body = p.Body,
                    Published = p.Published,
                    DateCreated = p.DateCreated,
                    DateUpdated = p.DateUpdated
                }).ToList();
            return View(model);
        }


        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateBlog()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBlog(CreateBlogViewModel formData)
        {
            return SaveBlog(null, formData);
        }

        private ActionResult SaveBlog(int? id, CreateBlogViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var userId = User.Identity.GetUserId();

            if (DbContext.Blogs.Any(p => p.UserId == userId &&
            p.Title == formData.Title &&
            (!id.HasValue || p.Id != id.Value)))
            {
                ModelState.AddModelError(nameof(CreateBlogViewModel.Title),
                    "Blog Title should be unique");

                return RedirectToAction(nameof(BlogController.CreateBlog));
            }

            string fileExtension;
            if (formData.Media != null)
            {
                fileExtension = Path.GetExtension(formData.Media.FileName);
                if (!Constants.AllowedFileExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("", "File extension is not allowed.");
                    return View();
                }
            }

            Blog blog;
            if (!id.HasValue)
            {
                blog = new Blog();
                blog.UserId = userId;
                blog.DateCreated = DateTime.Now;
                DbContext.Blogs.Add(blog);
            }
            else
            {
                blog = DbContext.Blogs.FirstOrDefault(
               p => p.Id == id);
                if (blog == null)
                {
                    return RedirectToAction(nameof(BlogController.BlogList));
                }
            }

            blog.Title = formData.Title;
            blog.Body = formData.Body;
            blog.Published = formData.Published;
            blog.DateUpdated = DateTime.Now;


            //Handling file upload
            if (formData.Media != null)
            {
                //Set Constants Model(MyBlog.Models.Constants) 
                if (!Directory.Exists(Constants.MappedUploadFolder))
                {
                    Directory.CreateDirectory(Constants.MappedUploadFolder);
                }
                var fileName = formData.Media.FileName;
                var fullPathWithName = Constants.MappedUploadFolder + fileName;

                formData.Media.SaveAs(fullPathWithName);
                blog.MediaUrl = Constants.UploadFolder + fileName;
            }
            DbContext.SaveChanges();
            return RedirectToAction(nameof(BlogController.BlogList));
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(BlogController.BlogList));
            }
            var userId = User.Identity.GetUserId();

            var blog = DbContext.Blogs.FirstOrDefault(
                p => p.Id == id && p.UserId == userId);
            if (blog == null)
            {
                return RedirectToAction(nameof(BlogController.Index));
            }
            var model = new CreateBlogViewModel();
            model.Title = blog.Title;
            model.Body = blog.Body;
            model.Published = blog.Published;
            model.MediaUrl = blog.MediaUrl;
            model.DateUpdated = DateTime.Now;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, CreateBlogViewModel formData)
        {
            return SaveBlog(id, formData);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction(nameof(BlogController.BlogList));

            //var userId = User.Identity.GetUserId();
            var blog = DbContext.Blogs.FirstOrDefault(p =>
               p.Id == id.Value);
            //var blog = DbContext.Blogs.FirstOrDefault(p =>
            //p.Id == id.Value &&
            //p.UserId == userId);

            if (blog == null)
                return RedirectToAction(nameof(BlogController.BlogList));

            var model = new DetailBlogViewModel();
            model.Title = blog.Title;
            model.Body = blog.Body;
            model.pulished = blog.Published;
            model.MediaUrl = blog.MediaUrl;
            model.DateCreated = blog.DateCreated;
            model.DateUpdated = blog.DateUpdated;
            return View(model);
        }

        [HttpPost]
        //if need to identify user role,could use [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(BlogController.BlogList));
            }

            var userId = User.Identity.GetUserId();

            var blog = DbContext.Blogs.FirstOrDefault(p => p.Id == id && p.UserId == userId);

            if (blog != null)
            {
                DbContext.Blogs.Remove(blog);
                DbContext.SaveChanges();
            }

            return RedirectToAction(nameof(BlogController.BlogList));
        }

    }
}