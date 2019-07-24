using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSharpExam1.Models
{
    public class Messages
    {
        [Key]
        public int MessageId { get; set; }
        [Required(ErrorMessage="Idea is required")]
        [MinLength(2, ErrorMessage = "Idea must be at least 2 characters")]
        [Display(Name="Idea")]
        
        public string Message { get; set; }
        public int UserId { get; set; }
        public Users Creator { get; set; }
        public List<Associations> Likes { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Updated_At { get; set; }

        public Messages()
        {
            Likes = new List<Associations>();
            Created_at = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }
    
}