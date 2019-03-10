using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Models.Domain
{
    public class Blog
    {

        public virtual ApplicationUser User { get; set; }

        public string UserId { get; set; }

        public int Id { get; set; }
        public string Title { get; set; }

        [AllowHtml]
        public string Body { get; set; }

        public bool Published { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public string MediaUrl { get; set; }
  
    }
}