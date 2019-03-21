using MyBlog.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyBlog.Models.ViewModels
{
    public class CreateCommentViewModel
    {
        public string UserId { get; set; }
        public int BlogId { get; set; }
        
        public string Slug { get; set; }
        //public int Id { get; set; }

        [Required]
        public string Body { get; set; }
        public DateTime DateCreated { get; set; }

        public CreateCommentViewModel()
        {
            DateCreated = DateTime.Now;
        }
    
    }
}