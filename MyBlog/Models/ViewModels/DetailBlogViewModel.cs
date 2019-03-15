using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Models.ViewModels
{
    public class DetailBlogViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public bool pulished { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime DateCreated { get; set; }
        public string MediaUrl { get; set; }
        public string Slug { get; set; }
    }
}