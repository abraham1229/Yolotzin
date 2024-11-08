using Microsoft.EntityFrameworkCore;
using OrtizAbrahamSprint3.Data.Entities;
using System.Reflection.Metadata.Ecma335;

namespace OrtizAbrahamSprint3.Data
{
    public class MyApplicationDbContext : DbContext
    {   
        //DbContext creation, where I need to write the name of all my tables
        public MyApplicationDbContext(DbContextOptions<MyApplicationDbContext> options) : base(options)
        { }
        //Tables 
        public DbSet<ClassUser> ClassUser { get; set; }
        public DbSet<Parent> Parent { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<WeekDays> WeekDays { get; set; }
        public DbSet<Instructor> Instructor { get; set; }
        public DbSet<Style> Style { get; set; }
        public DbSet<Levels> Levels { get; set; }
        public DbSet<AgeRange> AgeRange { get; set; }

    }
}
