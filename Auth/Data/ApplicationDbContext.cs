using Microsoft.EntityFrameworkCore;

namespace Auth.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAnswer>()
                .HasOne(u => u.Question)
                .WithMany(q => q.UserAnswers)
                .HasForeignKey(u => u.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<UserAnswer>()
                .HasOne(u => u.SelectedOption)
                .WithMany()
                .HasForeignKey(u => u.SelectedOptionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuestionOption>()
                .HasOne(o => o.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade); 
        }

        public DbSet<Track> Tracks { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<UserTrackRecommendation> UserTrackRecommendations { get; set; }
    }
}
