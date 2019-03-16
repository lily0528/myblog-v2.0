using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.Models.Domain
{
    public class Comment
    {
        public virtual ApplicationUser User { get; set; }
        public virtual Blog Blog { get; set; }

        public string UserId { get; set; }
        public int BlogId { get; set; }

        public int Id { get; set; }
     

        [Required]
        public string Body { get; set; }
        public string UpdateReason { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        

        public Comment()
        {
            DateCreated = DateTime.Now;
        }
    }
}