using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CSharpExam1.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSharpExam1.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Enter your Name")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must be letters only")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter your first name")]
        [MinLength(2, ErrorMessage = "Alias must be at least 2 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Alias must be letters only")]
        [Display(Name = "Alias")]
        public string Alias { get; set; }

        [Required(ErrorMessage = "Enter your email")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter a password")]
        [MinLength(8, ErrorMessage = "Must be at least 8 characters")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password:")]
        public string Check { get; set; }
        public List<Messages> Messages { get; set; }
        public List<Associations> Likes { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }

        public Users()
        {
            Likes = new List<Associations>();
            Messages = new List<Messages>();
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }
    }

    public class login
    {
        [Required(ErrorMessage = "Enter your email")]
        [Display(Name="Email")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string emaill { get; set; }

        [Required(ErrorMessage = "Enter a password")]
        [Display(Name="Password")]
        [MinLength(8, ErrorMessage = "Must be at least 8 characters")]
        public string passwordl { get; set; }

        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
    }
}