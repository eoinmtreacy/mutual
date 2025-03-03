using Microsoft.EntityFrameworkCore;
using Share.Model;

namespace View.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

   private const string DbPath = "app.db";
   
   public DbSet<Message> Messages { get; set; }
   
   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
      => optionsBuilder.UseSqlite($"Data Source={DbPath}");

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      modelBuilder.Entity<Message>()
         .HasKey(m => m.Id);
      
      modelBuilder.Entity<Message>()
         .HasIndex(m => m.Timestamp);
   }
   
}