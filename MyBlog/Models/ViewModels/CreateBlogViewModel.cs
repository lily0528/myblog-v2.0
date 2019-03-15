using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Models.ViewModels
{
    public class CreateBlogViewModel
    {
        [Required]
        public string Title { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        
        public bool Published { get; set; }

     
        public DateTime DateCreated { get; set; }


        public DateTime DateUpdated { get; set; }

        public HttpPostedFileBase Media { get; set; }

        public string MediaUrl { get; set; }
        public string Slug { get; set; }

        //public CreateBlogViewModel()
        //{
        //    DateCreated = DateTime.Now;
        //    DateUpdated = DateTime.Now;
        //}
    }
}