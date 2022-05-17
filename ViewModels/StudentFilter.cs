using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using ProektRS.Models;
namespace ProektRS.ViewModels
{
    public class StudentFilter
    {
        public IList<Student> students { get; set; }

        public string fullName { get; set; }

        public int studentId { get; set; }
    }
}
