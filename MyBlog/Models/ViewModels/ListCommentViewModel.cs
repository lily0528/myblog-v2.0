using MyBlog.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBlog.Models.ViewModels
{
    public class ListCommentViewModel
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public List<Comment> Comments { get; set; }
    }
}