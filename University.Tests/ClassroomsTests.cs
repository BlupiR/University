using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using University.Data;
using University.Interfaces;
using University.Models;
using University.ViewModels;

namespace University.Tests
{
    [TestClass]
    public class ClassroomsTests
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
            List<Classroom> classrooms = new List<Classroom>
            {
                new Classroom { ClassroomId = "1", Location = "Building A", Capacity = 30, AvailableSeats = 20, Projector = true, Whiteboard = true, Microphone = false, Description = "Standard classroom" },
                new Classroom { ClassroomId = "2", Location = "Building B", Capacity = 50, AvailableSeats = 40, Projector = false, Whiteboard = true, Microphone = true, Description = "Large lecture hall" }
            };
            _context.Classrooms.AddRange(classrooms);
            _context.SaveChanges();
        }

        [TestMethod]
        public void AddClassroomViewModel_SavingData_AddClassroomToDb()
        {
            var viewModel = new AddClassroomViewModel(_context, _dialogServiceMock.Object)
            {
                ClassroomId = "3",
                Location = "Building C",
                Capacity = 40,
                AvailableSeats = 35,
                Projector = true,
                Whiteboard = false,
                Microphone = true,
                Description = "New classroom"
            };

            viewModel.Save.Execute(null);

            Assert.AreEqual(3, _context.Classrooms.Count());
            var newClassroom = _context.Classrooms.Any(c => c.ClassroomId == "3" && c.Location == "Building C" && c.Capacity == 40);
            Assert.IsTrue(newClassroom);
        }

        [TestMethod]
        public void AddClassroomViewModel_SaveData_WrongData_SaveChanges()
        {
            var viewModel = new AddClassroomViewModel(_context, _dialogServiceMock.Object)
            {
                ClassroomId = "4",
                Location = string.Empty // Invalid Location
            };
            viewModel.Save.Execute(null);

            Assert.AreEqual(2, _context.Classrooms.Count());
            var newClassroom = _context.Classrooms.Any(c => c.ClassroomId == "4");
            Assert.IsFalse(newClassroom);
        }

        [TestMethod]
        public void ClassroomsViewModel_RemoveClassroom_IsValidClassroomId_DeletedClassroom()
        {
            var viewModel = new ClassroomsViewModel(_context, _dialogServiceMock.Object);
            string classroomId = "2";
            viewModel.Remove.Execute(classroomId);
            Assert.AreEqual(1, viewModel.Classrooms.Count);
            Assert.IsFalse(viewModel.Classrooms.Any(c => c.ClassroomId == classroomId));
        }

        [TestMethod]
        public void ClassroomsViewModel_RemoveClassroom_IsInvalidClassroomId_StayedClassroom()
        {
            var viewModel = new ClassroomsViewModel(_context, _dialogServiceMock.Object);
            string classroomId = "123";
            viewModel.Remove.Execute(classroomId);
            Assert.AreEqual(2, viewModel.Classrooms.Count);
        }

        [TestMethod]
        public void EditClassroomViewModel_SaveData_IsValidData_EditsClassroomData()
        {
            var classroom = _context.Classrooms.Find("1");
            var viewModel = new EditClassroomViewModel(_context, _dialogServiceMock.Object)
            {
                ClassroomId = classroom.ClassroomId,
                Location = "Building D",
                Capacity = 35,
                AvailableSeats = 30,
                Projector = false,
                Whiteboard = true,
                Microphone = false,
                Description = "Updated classroom"
            };
            viewModel.Save.Execute(null);

            var editedClassroom = _context.Classrooms.Find("1");
            Assert.AreEqual("Building D", editedClassroom.Location);
            Assert.AreEqual(35, editedClassroom.Capacity);
            Assert.AreEqual(30, editedClassroom.AvailableSeats);
            Assert.IsFalse(editedClassroom.Projector);
            Assert.IsTrue(editedClassroom.Whiteboard);
            Assert.IsFalse(editedClassroom.Microphone);
            Assert.AreEqual("Updated classroom", editedClassroom.Description);
        }

        [TestMethod]
        public void EditClassroomViewModel_SaveData_InvalidInput_DoesNotEditClassroomData()
        {
            var classroom = _context.Classrooms.Find("1");
            var viewModel = new EditClassroomViewModel(_context, _dialogServiceMock.Object)
            {
                ClassroomId = classroom.ClassroomId,
                Location = string.Empty // Invalid Location
            };
            viewModel.Save.Execute(null);

            var notEditedClassroom = _context.Classrooms.Find("1");
            Assert.AreEqual("Building A", notEditedClassroom.Location);
            Assert.AreEqual(30, notEditedClassroom.Capacity);
            Assert.AreEqual(20, notEditedClassroom.AvailableSeats);
            Assert.IsTrue(notEditedClassroom.Projector);
            Assert.IsTrue(notEditedClassroom.Whiteboard);
            Assert.IsFalse(notEditedClassroom.Microphone);
            Assert.AreEqual("Standard classroom", notEditedClassroom.Description);
        }

        [TestMethod]
        public void AddClassroomViewModel_IsValidData_LocationNeeds()
        {
            var viewModel = new AddClassroomViewModel(_context, _dialogServiceMock.Object)
            {
                Capacity = 20
            };

            var noLocation = viewModel[nameof(viewModel.Location)];

            Assert.AreEqual("Location is Required", noLocation);
        }

        [TestMethod]
        public void AddClassroomViewModel_IsValidData_CapacityNeeds()
        {
            var viewModel = new AddClassroomViewModel(_context, _dialogServiceMock.Object)
            {
                Location = "Building E"
            };

            var noCapacity = viewModel[nameof(viewModel.Capacity)];

            Assert.AreEqual("Capacity must be greater than zero", noCapacity);
        }
    }
}
