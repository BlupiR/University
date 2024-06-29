using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using University.Data;
using University.Interfaces;
using University.Models;

namespace University.ViewModels
{
    public class CoursesViewModel : ViewModelBase
    {
        private readonly UniversityContext _context;
        private readonly IDialogService _dialogService;

        private bool? _dialogResult = null;
        public bool? DialogResult
        {
            get
            {
                return _dialogResult;
            }
            set
            {
                _dialogResult = value;
            }
        }

        private ObservableCollection<Course>? _courses = null;
        public ObservableCollection<Course>? Courses
        {
            get
            {
                if (_courses is null)
                {
                    _courses = new ObservableCollection<Course>();
                    return _courses;
                }
                return _courses;
            }
            set
            {
                _courses = value;
                OnPropertyChanged(nameof(Courses));
            }
        }

        private ICommand? _add = null;
        public ICommand? Add
        {
            get
            {
                if (_add is null)
                {
                    _add = new RelayCommand<object>(AddNewCourse);
                }
                return _add;
            }
        }

        private void AddNewCourse(object? obj)
        {
            var instance = MainWindowViewModel.Instance();
            if (instance is not null)
            {
                instance.CoursesSubView = new AddCourseViewModel(_context, _dialogService);
            }
        }

        private ICommand? _edit = null;
        public ICommand? Edit
        {
            get
            {
                if (_edit is null)
                {
                    _edit = new RelayCommand<object>(EditCourse);
                }
                return _edit;
            }
        }

        private void EditCourse(object? obj)
        {
            if (obj is not null)
            {
                long courseCode = (long)obj;
                EditCourseViewModel editCourseViewModel = new EditCourseViewModel(_context, _dialogService)
                {
                    CourseCode = courseCode
                };
                var instance = MainWindowViewModel.Instance();
                if (instance is not null)
                {
                    instance.CoursesSubView = editCourseViewModel;
                }
            }
        }

        private ICommand? _remove = null;
        public ICommand? Remove
        {
            get
            {
                if (_remove is null)
                {
                    _remove = new RelayCommand<object>(RemoveCourse);
                }
                return _remove;
            }
        }

        private void RemoveCourse(object? obj)
        {
            if (obj is not null)
            {
                long courseCode = (long)obj;
                Course? course = _context.Courses.Find(courseCode);
                if (course is not null)
                {
                    DialogResult = _dialogService.Show(course.Title);
                    if (DialogResult == false)
                    {
                        return;
                    }

                    _context.Courses.Remove(course);
                    _context.SaveChanges();
                }
            }
        }

        public CoursesViewModel(UniversityContext context, IDialogService dialogService)
        {
            _context = context;
            _dialogService = dialogService;

            _context.Database.EnsureCreated();
            _context.Courses.Load();
            Courses = _context.Courses.Local.ToObservableCollection();
        }
    }

}
