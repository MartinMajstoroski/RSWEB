using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProektRS.Models;

namespace ProektRS.ViewModels
{
    public class EnrollStudentsAtCourseEdit
    {
        public Course course { get; set; }

        public IEnumerable<long>? selectedStudents { get; set; }

        public IEnumerable<SelectListItem>? studentsEnrolledList { get; set; }

        public int? year { get; set; }

    }
}
