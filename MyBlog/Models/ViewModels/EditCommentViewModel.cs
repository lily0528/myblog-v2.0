using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.Models.ViewModels
{
    public class EditCommentViewModel
    {
        public string UserId { get; set; }
        public int BlogId { get; set; }

        public string Body { get; set; }
        //public string slug { get; set; }
        public int Id { get; set; }

        [Required]
        public string UpdateReason { get; set; }
        public DateTime DateUpdated { get; set; }

        public EditCommentViewModel()
        {
            DateUpdated = DateTime.Now;
        }
    }
}