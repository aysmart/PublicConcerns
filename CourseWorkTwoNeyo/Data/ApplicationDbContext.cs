using CourseWorkTwoNeyo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CourseWorkTwoNeyo.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Admins> Admins { get; set; }
        public DbSet<Causes> Causes { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<CommentsLike> CommentsLikes { get; set; }
        public DbSet<Updates> Updates { get; set; }
        public DbSet<Signatures> Signatures { get; set; }
        public List<ApplicationDbContext> CausesStarted { get; set; }
        public List<Causes> CausesWithUsers { get; set; }

        public List<ApplicationDbContext> CausesSigned { get; set; }
        public Causes causeList { get; set; }
        public Users userList { get; set; }
        public Comments commentList { get; set; }

        public Signatures signatureList { get; set; }
    }
}



