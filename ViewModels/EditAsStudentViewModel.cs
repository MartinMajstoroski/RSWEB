using ProektRS.Models;
using System.ComponentModel.DataAnnotations;


namespace ProektRS.ViewModels
{
    public class EditAsStudentViewModel
    {
        public Enrollment enrollment { get; set; }

        [Display(Name = "Seminal File")]
        public IFormFile? seminalUrlFile { get; set; }

        public string? seminalUrlName { get; set; }

    }
}
