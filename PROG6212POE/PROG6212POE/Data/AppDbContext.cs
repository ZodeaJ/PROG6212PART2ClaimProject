using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PROG6212POE.Models;

namespace PROG6212POE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Coordinator> Coordinators { get; set; }
        public DbSet<Manager> Managers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //populate lecturers
            modelBuilder.Entity<Lecturer>().HasData(
                new Lecturer { LecturerId = 1, Name = "Emma Watson", Email = "emma.watson@example.com", PhoneNumber = "0712345678", Department = "Computer Science" },
                new Lecturer { LecturerId = 2, Name = "Chris Hemsworth", Email = "chris.hemsworth@example.com", PhoneNumber = "0723456789", Department = "Mathematics" },
                new Lecturer { LecturerId = 3, Name = "Scarlett Johansson", Email = "scarlett.johansson@example.com", PhoneNumber = "0734567890", Department = "Physics" },
                new Lecturer { LecturerId = 4, Name = "Leonardo DiCaprio", Email = "leonardo.dicaprio@example.com", PhoneNumber = "0745678901", Department = "Chemistry" },
                new Lecturer { LecturerId = 5, Name = "Jennifer Lawrence", Email = "jennifer.lawrence@example.com", PhoneNumber = "0756789012", Department = "Biology" }
            );
        }
    }
}