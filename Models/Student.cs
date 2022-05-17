using System.ComponentModel.DataAnnotations;
namespace ProektRS.Models
{
    
    public class Student
    {
        public int ID { get; set; }
        [StringLength(10, MinimumLength = 3)]
        [Required]
        [Display(Name = "Student ID")]
        public string StudentID { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required]
        [Display(Name = "Last Name")]

        public string LastName { get; set; }
        [StringLength(50, MinimumLength = 3)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Enrollment Date")]
        [DataType(DataType.Date)]
        public DateTime? enrollmentDate { get; set; }

        [Display(Name = "Acquired Credits")]
        public int? acquiredCredits { get; set; }

        [Display(Name = "Current Semester")]
        public int? currentSemester { get; set; }

        [StringLength(25, MinimumLength = 3)]
        [Display(Name = "Education Level")]
        public string? educationLevel { get; set; }

        public ICollection<Enrollment>? enrollments { get; set; }

        public ICollection<Enrollment>? Courses { get; set; }

        public string fullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }
    }
}