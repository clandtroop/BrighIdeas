using Microsoft.EntityFrameworkCore;

namespace CSharpExam1.Models
{
    public class context : DbContext
    {
        public context(DbContextOptions options) : base(options) { }

        public DbSet<Users> Users { get; set; }
        public DbSet<Messages> Messsages  { get; set; }
        public DbSet<Associations> Associations { get; set; }
        
    }
}