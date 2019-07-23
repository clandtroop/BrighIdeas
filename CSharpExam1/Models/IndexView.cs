using System.Collections.Generic;

namespace CSharpExam1.Models
{
    public class IndexView
    {
        public Messages NewMessage { get; set; }
        public List<Messages> AllMessages { get; set; }

        public Users NewUser { get; set; }
        public List<Users> AllUsers { get; set; }

        public Associations NewAssociations { get; set; }

    }
}