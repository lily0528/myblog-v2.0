using Microsoft.AspNet.Identity;
using MyBlog.Models;
using MyBlog.Models.Domain;
using MyBlog.Models.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        [Authorize(Roles = "Admin")]
        public ActionResult BlogList()
        {
            var model = DbContext.Blogs
                .Select(p => new IndexBlogViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Body = p.Body,
                    Published = p.Published,
                    DateCreated = p.DateCreated,
                    //DateUpdated = p.DateUpdated,
                    MediaUrl = p.MediaUrl,
                    Slug = p.Slug
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

            var titleOfFriendly = formData.Title;

            var slug = SlugFriendly(titleOfFriendly);

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
                    return HttpNotFound();
                }
                blog.DateUpdated = DateTime.Now;
            }
            blog.Slug = slug;
            blog.Title = formData.Title;
            blog.Body = formData.Body;
            blog.Published = formData.Published;

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

            var blog = DbContext.Blogs.FirstOrDefault(
                p => p.Id == id);
            if (blog == null)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
            var model = new CreateBlogViewModel
            {
                Title = blog.Title,
                Body = blog.Body,
                Published = blog.Published,
                MediaUrl = blog.MediaUrl,
                DateUpdated = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string title, CreateBlogViewModel formData)
        {
            if (title == null)
            {
                return RedirectToAction(nameof(BlogController.BlogList));
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            return SaveBlog(null, formData);
        }

        [HttpGet]
        [Route("Myblog/{slug}")]
        public ActionResult Details(string slug)
        {
            if (slug == null)
                return RedirectToAction(nameof(BlogController.BlogList));

            var blog = DbContext.Blogs.FirstOrDefault(p =>
               p.Slug == slug);

            if (blog == null)
                return HttpNotFound();

            var model = new DetailBlogViewModel
            {
                Slug = blog.Slug,
                Title = blog.Title,
                Body = blog.Body,
                pulished = blog.Published,
                MediaUrl = blog.MediaUrl,
                DateCreated = blog.DateCreated,
                DateUpdated = blog.DateUpdated
            };
            return View("Details", model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(BlogController.BlogList));
            }

            var blog = DbContext.Blogs.FirstOrDefault(p => p.Id == id);
            if (blog != null)
            {
                DbContext.Blogs.Remove(blog);
                DbContext.SaveChanges();
            }
            return RedirectToAction(nameof(BlogController.BlogList));
        }

        private string SlugFriendly(string titleOfFriendly)
        {
            string str = titleOfFriendly.ToLower();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"[\s-]+", " ").Trim();
            str = str.Substring(0, str.Length <= 100 ? str.Length : 100).Trim();
            str = Regex.Replace(str, @"\s", "-");

            while (DbContext.Blogs.Any(p => p.Slug == str))
            {
                Random rand = new Random();
                str = str + rand.Next(1, 100).ToString();
            }
            return str;
        }
    }
}