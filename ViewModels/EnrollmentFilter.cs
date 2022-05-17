using Microsoft.AspNetCore.Mvc.Rendering;
using ProektRS.Models;

namespace ProektRS.ViewModels
{
    public class EnrollmentFilter
    {
        public IList<Enrollment> enrollments { get; set; }

        public SelectList yearsList { get; set; }
        public int year { get; set; }

    }
}
