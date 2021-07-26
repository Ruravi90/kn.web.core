using Microsoft.EntityFrameworkCore;

namespace kn.web.core.Models
{
    public class EFContext : DbContext
    {
        //private string url = "47.89.209.96;User ID=alkkon;Password=Alkk0n*;Database=gigaDB";
        public DbSet<User> Users { get; set; }  
        public DbSet<Event> Events { get; set; }  

        public EFContext(DbContextOptions<EFContext> options) : base(options)  
        {   
        }

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
 