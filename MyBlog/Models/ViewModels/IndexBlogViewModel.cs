using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Models.ViewModels
{
    public class IndexBlogViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        
        public string Body { get; set; }

        public bool Published { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string MediaUrl { get; set; }
    }
}