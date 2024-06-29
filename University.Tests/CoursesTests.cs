using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Data;
using University.Interfaces;
using University.Models;
using University.ViewModels;
using static System.Reflection.Metadata.BlobBuilder;

namespace University.Tests
{
    [TestClass]
    public class CoursesTests
    {
        private UniversityContext _context;
        private DbContextOptions<UniversityContext> _options;
        private Mock<IDialogService> _dialogServiceMock;


        [TestInitialize()]
        public void Initialize()
        {
            _options = new DbContextOptionsBuilder<UniversityContext>()
                .UseInMemoryDatabase(databaseName: "UniversityTestDB")
                .Options;
            _context = new UniversityContext(_options);
            _dialogServiceMock = new Mock<IDialogService>();
            _dialogServiceMock.Setup(x => x.Show(It.IsAny<string>())).Returns(true);
            _context.Database.EnsureDeleted();
            List<Course> courses = new List<Course>
            {
                new Course { CourseCode = 1, Title = "Course 1", Instructor = "Insctuctor 1", Schedule = "Schedule 1", Description = "Description 1", Credits = 5, Department = "Department 1", Prerequisites = "Prerequisites 1"},
                new Course { CourseCode = 2, Title = "Course 2", Instructor = "Insctuctor 2", Schedule = "Schedule 2", Description = "Description 2", Credits = 6, Department = "Department 2", Prerequisites = "Prerequisites 2" }
            };
            _context.Courses.AddRange(courses);
            _context.SaveChanges();
        }
        [TestMethod]
        public void AddCourseViewModel_SavingData_AddCourseToDb()
        {
            var viewModel = new AddCourseViewModel(_context, _dialogServiceMock.Object)
            {
                CourseCode = 3,
                Title = "Coursik",
                Instructor = "Instructorr",
                Schedule = "Schedulee",
                Description = "Descriptionn",
                Credits = 5,
                Department = "Departmentt",
                Prerequisites = "Prerequisitess",
            };

            viewModel.Save.Execute(null);

            Assert.AreEqual(3, _context.Courses.Count());
            var newCourse = _context.Courses.Any(c => c.Title == "Coursik" && c.Instructor == "Instructorr" && c.Department == "Departmentt");
            Assert.IsTrue(newCourse);
        }
        [TestMethod]
        public void AddCourseViewModel_SaveData_WrongData_SaveChanges()
        {
            var viewModel = new AddCourseViewModel(_context, _dialogServiceMock.Object)
            {
                Title = "Coursssik"
            };
            viewModel.Save.Execute(null);

            Assert.AreEqual(2, _context.Courses.Count());
            var newCourse = _context.Courses.Any(c => c.Title == "Coursik" && c.Instructor == "Ins" && c.Credits == 5);
            Assert.IsFalse(newCourse);
        }

        [TestMethod]
        public void CoursesViewModel_RemoveCourse_IsValidCourseCode_DeletedCourse()
        {
            var viewModel = new CoursesViewModel(_context, _dialogServiceMock.Object);
            long courseCode = 2;
            viewModel.Remove.Execute(courseCode);
            Assert.AreEqual(1, viewModel.Courses.Count);
            Assert.IsFalse(viewModel.Courses.Any(c => c.CourseCode == courseCode));
        }

        [TestMethod]
        public void CoursesViewModel_RemoveCourse_IsValidCourseCode_StayedCourse()
        {
            var viewModel = new CoursesViewModel(_context, _dialogServiceMock.Object);
            long courseCode = 123;
            viewModel.Remove.Execute(courseCode);
            Assert.AreEqual(2, viewModel.Courses.Count);
        }

        [TestMethod]
        public void EditCourseViewModel_SaveData_IsValidDta_EdditingCourseData()
        {
            var course = _context.Courses.Find((long)1);
            var viewModel = new EditCourseViewModel(_context, _dialogServiceMock.Object)
            {
                CourseCode = course.CourseCode,
                Title = "New Title",
                Instructor = "Serhii",
                Schedule = "New Schedule",
                Description = "New description",
                Credits = 9,
                Department =  "New Department",
                Prerequisites = "New Prerequisites"
            };
            viewModel.Save.Execute(null);

            var edditedCourse = _context.Courses.Find((long)course.CourseCode);
            Assert.AreEqual("New Title", edditedCourse.Title);
            Assert.AreEqual("Serhii", edditedCourse.Instructor);
            Assert.AreEqual("New Schedule", edditedCourse.Schedule);
            Assert.AreEqual("New description", edditedCourse.Description);
            Assert.AreEqual(9, edditedCourse.Credits);
            Assert.AreEqual("New Department", edditedCourse.Department);
            Assert.AreEqual("New Prerequisites", edditedCourse.Prerequisites);
        }

        [TestMethod]
        public void EditCourseViewModel_SaveData_InvalidInput_DoesNotAddEditedBookToContext()
        {
            var course = _context.Courses.Find((long)1);
            var viewModel = new EditCourseViewModel(_context, _dialogServiceMock.Object)
            {
                CourseCode = course.CourseCode
            };

            viewModel.Save.Execute(null);

            var notEdditedCourse = _context.Courses.Find(course.CourseCode);
            Assert.AreEqual("Course 1", notEdditedCourse.Title);
            Assert.AreEqual("Insctuctor 1", notEdditedCourse.Instructor);
            Assert.AreEqual("Schedule 1", notEdditedCourse.Schedule);
            Assert.AreEqual("Description 1", notEdditedCourse.Description);
            Assert.AreEqual(5, notEdditedCourse.Credits);
            Assert.AreEqual("Department 1", notEdditedCourse.Department);
            Assert.AreEqual("Prerequisites 1", notEdditedCourse.Prerequisites);
        }

        [TestMethod]
        public void AddCourseViewModel_IsValidData_DepartmentNeeds()
        {
            // Arrange
            var viewModel = new AddCourseViewModel(_context, _dialogServiceMock.Object)
            {
                Title = "Programming",
                Credits = 10,
            };

            var noDepartment = viewModel[nameof(viewModel.Department)];

            Assert.AreEqual("Department is Required", noDepartment);
        }
        [TestMethod]
        public void AddCourseViewModel_IsValidData_InstructorNeeds()
        {
            // Arrange
            var viewModel = new AddCourseViewModel(_context, _dialogServiceMock.Object)
            {
               Title = "Programming",
               Credits = 10,
            };

            // Act
            var noInstructor = viewModel[nameof(viewModel.Instructor)];

            // Assert
            Assert.AreEqual("Instructor is Required", noInstructor);
        }
    }
}
