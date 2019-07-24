using System;
using System.Collections.Generic;
using CSharpExam1.Models;
using System.ComponentModel.DataAnnotations;

namespace CSharpExam1.Models
{
    public class Associations
    {
        [Key]
        public int AssociationId { get; set; }
        public int MessageId { get; set; }
        public int UserId { get; set; }
        public Messages Messages { get; set; }
        public Users Users { get; set; }

    }

}