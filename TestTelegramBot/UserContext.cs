using Microsoft.EntityFrameworkCore;

namespace TestTelegramBot
{
    public class UserContext : DbContext 
    {
        public DbSet<User> Users { get; set; }

        public UserContext() 
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=HOME-PC\SQLEXPRESS;Database=TelegramUsers;Trusted_Connection=True;");
        }
    }
}
