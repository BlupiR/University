﻿using CommunityToolkit.Mvvm.Input;
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
    public class EditFacultyMemberViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;
        private FacultyMember? _facultyMember = new FacultyMember();

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "FacultyId":
                        if (FacultyId <= 0)
                            return "Faculty ID is Required";
                        break;
                    case "Name":
                        if (string.IsNullOrEmpty(Name))
                            return "Name is Required";
                        break;
                    case "Age":
                        if (Age <= 0)
                            return "Age must be greater than 0";
                        break;
                    case "Gender":
                        if (string.IsNullOrEmpty(Gender))
                            return "Gender is Required";
                        break;
                    case "Department":
                        if (string.IsNullOrEmpty(Department))
                            return "Department is Required";
                        break;
                    case "Position":
                        if (string.IsNullOrEmpty(Position))
                            return "Position is Required";
                        break;
                    case "Email":
                        if (string.IsNullOrEmpty(Email))
                            return "Email is Required";
                        break;
                    case "OfficeRoomNumber":
                        if (string.IsNullOrEmpty(OfficeRoomNumber))
                            return "Office Room Number is Required";
                        break;
                    default:
                        break;
                }
                return string.Empty;
            }
        }

        private long _facultyId = 0;
        public long FacultyId
        {
            get
            {
                return _facultyId;
            }
            set
            {
                _facultyId = value;
                OnPropertyChanged(nameof(FacultyId));
                LoadFacultyMemberData();
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private int _age = 0;
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        private string _gender = string.Empty;
        public string Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        private string _department = string.Empty;
        public string Department
        {
            get
            {
                return _department;
            }
            set
            {
                _department = value;
                OnPropertyChanged(nameof(Department));
            }
        }

        private string _position = string.Empty;
        public string Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        private string _email = string.Empty;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string _officeRoomNumber = string.Empty;
        public string OfficeRoomNumber
        {
            get
            {
                return _officeRoomNumber;
            }
            set
            {
                _officeRoomNumber = value;
                OnPropertyChanged(nameof(OfficeRoomNumber));
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
                instance.FacultyMembersSubView = new FacultyMembersViewModel(_context, _dialogService);
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

            if (_facultyMember is null)
            {
                return;
            }
            _facultyMember.Name = Name;
            _facultyMember.Age = Age;
            _facultyMember.Gender = Gender;
            _facultyMember.Department = Department;
            _facultyMember.Position = Position;
            _facultyMember.Email = Email;
            _facultyMember.OfficeRoomNumber = OfficeRoomNumber;

            _context.Entry(_facultyMember).State = EntityState.Modified;
            _context.SaveChanges();

            Response = "Data Saved";
        }

        public EditFacultyMemberViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;
        }

        private bool IsValid()
        {
            string[] properties = { "Name", "Age", "Gender", "Department", "Position", "Email", "OfficeRoomNumber" };
            foreach (string property in properties)
            {
                if (!string.IsNullOrEmpty(this[property]))
                {
                    return false;
                }
            }
            return true;
        }

        private void LoadFacultyMemberData()
        {
            _facultyMember = _context.FacultyMembers.Find(FacultyId);
            if (_facultyMember is null)
            {
                return;
            }
            this.Name = _facultyMember.Name;
            this.Age = _facultyMember.Age;
            this.Gender = _facultyMember.Gender;
            this.Department = _facultyMember.Department;
            this.Position = _facultyMember.Position;
            this.Email = _facultyMember.Email;
            this.OfficeRoomNumber = _facultyMember.OfficeRoomNumber;
        }
    }
}
