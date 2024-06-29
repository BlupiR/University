using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class EditClassroomViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;
        private Classroom? _classroom = new Classroom();

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "ClassroomId":
                        if (string.IsNullOrEmpty(ClassroomId))
                        {
                            return "Classroom ID is Required";
                        }
                        break;
                    case "Location":
                        if (string.IsNullOrEmpty(Location))
                        {
                            return "Location is Required";
                        }
                        break;
                    case "Capacity":
                        if (Capacity <= 0)
                        {
                            return "Capacity must be greater than zero";
                        }
                        break;
                }
                return string.Empty;
            }
        }

        private string _classroomId = string.Empty;
        public string ClassroomId
        {
            get
            {
                return _classroomId;
            }
            set
            {
                _classroomId = value;
                OnPropertyChanged(nameof(ClassroomId));
                LoadClassroomData();
            }
        }

        private string _location = string.Empty;
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private int _capacity = 0;
        public int Capacity
        {
            get
            {
                return _capacity;
            }
            set
            {
                _capacity = value;
                OnPropertyChanged(nameof(Capacity));
            }
        }

        private int _availableSeats = 0;
        public int AvailableSeats
        {
            get
            {
                return _availableSeats;
            }
            set
            {
                _availableSeats = value;
                OnPropertyChanged(nameof(AvailableSeats));
            }
        }

        private bool _projector = false;
        public bool Projector
        {
            get
            {
                return _projector;
            }
            set
            {
                _projector = value;
                OnPropertyChanged(nameof(Projector));
            }
        }

        private bool _whiteboard = false;
        public bool Whiteboard
        {
            get
            {
                return _whiteboard;
            }
            set
            {
                _whiteboard = value;
                OnPropertyChanged(nameof(Whiteboard));
            }
        }

        private bool _microphone = false;
        public bool Microphone
        {
            get
            {
                return _microphone;
            }
            set
            {
                _microphone = value;
                OnPropertyChanged(nameof(Microphone));
            }
        }

        private string _description = string.Empty;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _response = string.Empty;
        public string Response
        {
            get
            {
                return _response;
            }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(Response));
            }
        }

        private ICommand? _back = null;
        public ICommand Back
        {
            get
            {
                if (_back is null)
                {
                    _back = new RelayCommand<object>(NavigateBack);
                }
                return _back;
            }
        }

        private void NavigateBack(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.ClassroomsSubView = new ClassroomsViewModel(_context, _dialogService);
            }
        }

        private ICommand? _save = null;
        public ICommand Save
        {
            get
            {
                if (_save is null)
                {
                    _save = new RelayCommand<object>(SaveData);
                }
                return _save;
            }
        }

        private void SaveData(object? obj)
        {
            if (!IsValid())
            {
                Response = "Please complete all required fields";
                return;
            }

            if (_classroom is null)
            {
                return;
            }

            _classroom.Location = Location;
            _classroom.Capacity = Capacity;
            _classroom.AvailableSeats = AvailableSeats;
            _classroom.Projector = Projector;
            _classroom.Whiteboard = Whiteboard;
            _classroom.Microphone = Microphone;
            _classroom.Description = Description;

            _context.Entry(_classroom).State = EntityState.Modified;
            _context.SaveChanges();

            Response = "Data Saved";
        }

        public EditClassroomViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private bool IsValid()
        {
            string[] properties = { "ClassroomId", "Location", "Capacity" };
            foreach (string property in properties)
            {
                if (!string.IsNullOrEmpty(this[property]))
                {
                    return false;
                }
            }
            return true;
        }

        private void LoadClassroomData()
        {
            var classrooms = _context.Classrooms;
            if (classrooms is not null)
            {
                _classroom = classrooms.Find(ClassroomId);
                if (_classroom is null)
                {
                    return;
                }
                Location = _classroom.Location;
                Capacity = _classroom.Capacity;
                AvailableSeats = _classroom.AvailableSeats;
                Projector = _classroom.Projector;
                Whiteboard = _classroom.Whiteboard;
                Microphone = _classroom.Microphone;
                Description = _classroom.Description;
            }
        }
    }

}
