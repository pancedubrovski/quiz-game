using Microsoft.EntityFrameworkCore;
using quiz_game.Database.Entites;
using quiz_game.Models;

namespace quiz_game.Database
{
    public class QuizGameContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public QuizGameContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>().HasIndex(u => u.Username).IsUnique();

        }
    }
}
