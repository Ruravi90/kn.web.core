using Microsoft.EntityFrameworkCore;

namespace kn.web.core.Models
{
    public class EFKNContext : DbContext
    {
        private string url = "";
        public DbSet<User> Users { get; set; }  
        public DbSet<Event> Events { get; set; }

        public EFKNContext(string _url) {
            url = _url;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseMySql(url, ServerVersion.AutoDetect(url));

        protected override void OnModelCreating(ModelBuilder modelBuilder)  
        {  
            // Use Fluent API to configure  
  
            // Map entities to tables  
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Event>().ToTable("Events");  

             // Configure Primary Keys  
            modelBuilder.Entity<User>().HasKey(u => u.Id).HasName("PK_Users"); 
            modelBuilder.Entity<Event>().HasKey(ug => ug.Id).HasName("PK_Events");  
        }
    }
}
 