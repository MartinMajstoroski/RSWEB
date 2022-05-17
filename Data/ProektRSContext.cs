#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProektRS.Models;

namespace ProektRS.Data
{
    public class ProektRSContext : DbContext
    {
        public ProektRSContext(DbContextOptions<ProektRSContext> options)
            : base(options)
        {
        }

        public DbSet<ProektRS.Models.Course> Course { get; set; }

        public DbSet<ProektRS.Models.Student> Student { get; set; }

        public DbSet<ProektRS.Models.Teacher> Teacher { get; set; }

        public DbSet<ProektRS.Models.Enrollment> Enrollment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
            .HasOne<Teacher>(p => p.firstTeacher)
            .WithMany(p => p.coursesOne)
            .HasForeignKey(p => p.firstTeacherId);
            modelBuilder.Entity<Course>()
                .HasOne<Teacher>(p => p.secondTeacher)
                .WithMany(p => p.coursesTwo)
                .HasForeignKey(p => p.secondTeacherId);



            modelBuilder.Entity<Enrollment>()
                 .HasOne<Student>(p => p.Student)
                 .WithMany(p => p.Courses)
                 .HasForeignKey(p => p.StudentID);
            //.HasPrincipalKey(p => p.Id);
            modelBuilder.Entity<Enrollment>()
                .HasOne<Course>(p => p.Course)
                .WithMany(p => p.enrollments)
                .HasForeignKey(p => p.courseId);
            //.HasPrincipalKey(p => p.Id);*/
        }
    }
}
