using System;
using University.Interfaces;
using University.Data;

namespace University.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly UniversityContext _context;
    private readonly IDialogService _dialogService;

    private int _selectedTab;
    public int SelectedTab
    {
        get
        {
            return _selectedTab;
        }
        set
        {
            _selectedTab = value;
            OnPropertyChanged(nameof(SelectedTab));
        }
    }

    private object? _studentsSubView = null;
    public object? StudentsSubView
    {
        get
        {
            return _studentsSubView;
        }
        set
        {
            _studentsSubView = value;
            OnPropertyChanged(nameof(StudentsSubView));
        }
    }

    private object? _subjectsSubView = null;
    public object? SubjectsSubView
    {
        get
        {
            return _subjectsSubView;
        }
        set
        {
            _subjectsSubView = value;
            OnPropertyChanged(nameof(SubjectsSubView));
        }
    }

    private object? _classroomsSubView = null;
    public object? ClassroomsSubView
    {
        get
        {
            return _classroomsSubView;
        }
        set
        {
            _classroomsSubView = value;
            OnPropertyChanged(nameof(ClassroomsSubView));
        }
    }


    private object? _searchSubView = null;
    public object? SearchSubView
    {
        get
        {
            return _searchSubView;
        }
        set
        {
            _searchSubView = value;
            OnPropertyChanged(nameof(SearchSubView));
        }
    }

    private object? _сoursesSubView = null;
    public object? CoursesSubView
    {
        get
        {
            return _сoursesSubView;
        }
        set
        {
            _сoursesSubView = value;
            OnPropertyChanged(nameof(CoursesSubView));
        }
    }

    private object? _facultyMembersSubView = null;
    public object? FacultyMembersSubView
    {
        get
        {
            return _facultyMembersSubView;
        }
        set
        {
            _facultyMembersSubView = value;
            OnPropertyChanged(nameof(FacultyMembersSubView));
        }
    }

    private static MainWindowViewModel? _instance = null;
    public static MainWindowViewModel? Instance()
    {
        return _instance;
    }

    public MainWindowViewModel(UniversityContext context, IDialogService dialogService)
    {
        _context = context;
        _dialogService = dialogService;

        if (_instance is null)
        {
            _instance = this;
        }

        StudentsSubView = new StudentsViewModel(_context, _dialogService);
        SubjectsSubView = new SubjectsViewModel(_context, _dialogService);
        SearchSubView = new SearchViewModel(_context, _dialogService);
        CoursesSubView = new CoursesViewModel(_context, _dialogService);
        ClassroomsSubView = new ClassroomsViewModel(_context, _dialogService);
        FacultyMembersSubView = new FacultyMembersViewModel(_context, _dialogService);
    }
}
