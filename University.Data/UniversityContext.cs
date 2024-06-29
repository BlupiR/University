using University.Models;
using Microsoft.EntityFrameworkCore;

namespace University.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext()
        {
        }

        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<FacultyMember> FacultyMembers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("UniversityDb");
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>().Ignore(s => s.IsSelected);

            modelBuilder.Entity<Student>().HasData(
                new Student { StudentId = 1, Name = "Wieńczysław", LastName = "Nowakowicz", PESEL = "PESEL1", BirthDate = new DateTime(1987, 05, 22) },
                new Student { StudentId = 2, Name = "Stanisław", LastName = "Nowakowicz", PESEL = "PESEL2", BirthDate = new DateTime(2019, 06, 25) },
                new Student { StudentId = 3, Name = "Eugenia", LastName = "Nowakowicz", PESEL = "PESEL3", BirthDate = new DateTime(2021, 06, 08) });

            modelBuilder.Entity<Subject>().HasData(
                new Subject { SubjectId = 1, Name = "Matematyka", Semester = "1", Lecturer = "Michalina Warszawa" },
                new Subject { SubjectId = 2, Name = "Biologia", Semester = "2", Lecturer = "Halina Katowice" },
                new Subject { SubjectId = 3, Name = "Chemia", Semester = "3", Lecturer = "Jan Nowak" }
            );

            modelBuilder.Entity<Course>().HasData(
                new Course {CourseCode = 33, Title = "New Course", Instructor = "Me", Schedule = "Not set" , Description = "No set", Credits = 10, Department = "Any", Prerequisites = "IdkWIS"}
            );

            modelBuilder.Entity<Classroom>().HasData(
                 new Classroom {ClassroomId = "312", Location = "University", Capacity = 34, AvailableSeats = 11, Projector = true, Whiteboard = true, Microphone = false, Description = "It's a classroom" }
                );

            modelBuilder.Entity<FacultyMember>().HasData(
                 new FacultyMember { FacultyId = 1, Name = "Artom", Age = 22, Gender = "Im mechanic", Department = "World of 12", Position = "Mid", Email = "yatrahall@ukr.net", OfficeRoomNumber = "44" }
                );
        }
    }
}
