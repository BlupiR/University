using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace University.Models
{
    public class Course
    {
        [Key]
        public long CourseCode { get; set; } = 0;
        public string Title { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; } = 0;
        public string Department { get; set; } = string.Empty;
        public string Prerequisites { get; set; } = string.Empty;
    }
}
