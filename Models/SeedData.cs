using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using ProektRS.Data;
using ProektRS.Models;
using System;
using System.Linq;

namespace ProektRS.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ProektRSContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ProektRSContext>>()))
            {
                if (context.Course.Any() || context.Student.Any() || context.Teacher.Any())
                {
                    return;
                }

                context.Student.AddRange(
                    new Student
                    {
                        StudentID = "1392019",
                        FirstName = "Stojan",
                        LastName = "Stojanoski",
                        enrollmentDate = DateTime.Parse("2019-4-20"),
                        acquiredCredits = 30,
                        currentSemester = 2,
                        educationLevel = "Bachelor's Degree"
                    },
                    new Student
                    {
                        StudentID = "552019",
                        FirstName = "Dimitar",
                        LastName = "Dimitrijoski",
                        enrollmentDate = DateTime.Parse("2019-7-11"),
                        acquiredCredits = 14,
                        currentSemester = 2,
                        educationLevel = "Bachelor's Degree"
                    },
                    new Student
                    {
                        StudentID = "022016",
                        FirstName = "Petar",
                        LastName = "Petreski",
                        enrollmentDate = DateTime.Parse("2020-2-4"),
                        acquiredCredits = 142,
                        currentSemester = 5,
                        educationLevel = "Bachelor's Degree"
                    },
                    new Student
                    {
                        StudentID = "242020",
                        FirstName = "Marija",
                        LastName = "Mitreska",
                        enrollmentDate = DateTime.Parse("2022-10-23"),
                        acquiredCredits = 210,
                        currentSemester = 8,
                        educationLevel = "Bachelor's Degree"
                    },
                    new Student
                    {
                        StudentID = "202019",
                        FirstName = "Ilina",
                        LastName = "Naumoska",
                        enrollmentDate = DateTime.Parse("2021-1-20"),
                        acquiredCredits = 78,
                        currentSemester = 3,
                        educationLevel = "Bachelor's Degree"
                    }
                );
                context.Teacher.AddRange(
                    new Teacher
                    {
                        firstName = "Simeon",
                        lastName = "Milenkoski",
                        degree = "Ph.D.",
                        academicRank = "Full Professor",
                        officeNumber = "223",
                        hireDate = DateTime.Parse("2002-3-20")
                    },
                    new Teacher
                    {
                        firstName = "Katerina",
                        lastName = "Stojkoska",
                        degree = "Master's degree",
                        academicRank = "Assistant Professor",
                        officeNumber = "211",
                        hireDate = DateTime.Parse("2010-2-11")
                    },
                    new Teacher
                    {
                        firstName = "Bojan",
                        lastName = "Nedelkoski",
                        degree = "Ph.D.",
                        academicRank = "Full Professor",
                        officeNumber = "102",
                        hireDate = DateTime.Parse("1998-1-11")
                    },
                    new Teacher
                    {
                        firstName = "Nebojsha",
                        lastName = "Ilijoski",
                        degree = "Master's degree",
                        academicRank = "Assistant Professor",
                        officeNumber = "204A",
                        hireDate = DateTime.Parse("2016-4-8")
                    }
                );
                context.SaveChanges();

                context.Course.AddRange(
                    new Course
                    {
                        title = "Matematika 1",
                        credits = 7,
                        semester = 1,
                        programme = "KTI",
                        educationLevel = "Bachelor's degree",
                        firstTeacherId = context.Teacher.Single(d => d.firstName == "Katerina" && d.lastName == "Stojkoska").teacherId,
                        firstTeacher = context.Teacher.Single(d => d.firstName == "Katerina" && d.lastName == "Stojkoska"),
                        secondTeacherId = context.Teacher.Single(d => d.firstName == "Bojan" && d.lastName == "Nedelkoski").teacherId,
                        secondTeacher = context.Teacher.Single(d => d.firstName == "Bojan" && d.lastName == "Nedelkoski")
                    },
                    new Course
                    {
                        title = "RSWEB",
                        credits = 6,
                        semester = 6,
                        programme = "KTI",
                        educationLevel = "Bachelor's degree",
                        firstTeacherId = context.Teacher.Single(d => d.firstName == "Katerina" && d.lastName == "Stojkoska").teacherId,
                        firstTeacher = context.Teacher.Single(d => d.firstName == "Katerina" && d.lastName == "Stojkoska"),
                        secondTeacherId = context.Teacher.Single(d => d.firstName == "Nebojsha" && d.lastName == "Ilijoski").teacherId,
                        secondTeacher = context.Teacher.Single(d => d.firstName == "Nebojsha" && d.lastName == "Ilijoski")
                    },
                    new Course
                    {
                        title = "Android programming",
                        credits = 6,
                        semester = 7,
                        programme = "KTI",
                        educationLevel = "Bachelor's degree",
                        firstTeacherId = context.Teacher.Single(d => d.firstName == "Simeon" && d.lastName == "Milenkoski").teacherId,
                        firstTeacher = context.Teacher.Single(d => d.firstName == "Simeon" && d.lastName == "Milenkoski"),
                        secondTeacherId = context.Teacher.Single(d => d.firstName == "Nebojsha" && d.lastName == "Ilijoski").teacherId,
                        secondTeacher = context.Teacher.Single(d => d.firstName == "Nebojsha" && d.lastName == "Ilijoski")
                    },
                    new Course
                    {
                        title = "OWEB",
                        credits = 6,
                        semester = 5,
                        programme = "KTI",
                        educationLevel = "Bachelor's degree",
                        firstTeacherId = context.Teacher.Single(d => d.firstName == "Simeon" && d.lastName == "Milenkoski").teacherId,
                        firstTeacher = context.Teacher.Single(d => d.firstName == "Simeon" && d.lastName == "Milenkoski"),
                        secondTeacherId = context.Teacher.Single(d => d.firstName == "Katerina" && d.lastName == "Stojkoska").teacherId,
                        secondTeacher = context.Teacher.Single(d => d.firstName == "Katerina" && d.lastName == "Stojkoska")
                    }
                );
                context.SaveChanges();

                context.Enrollment.AddRange(
                    new Enrollment
                    {
                        courseId = context.Course.Single(d => d.title == "OWEB").courseId,
                        StudentID = context.Student.Single(d => d.FirstName == "Dimitar" && d.LastName == "Dimitrijoski").ID,
                        semester = "1",
                        year = 2021,
                        grade = 8,
                        seminalUrl = "github",
                        projectUrl = "github",
                        examPoints = 40,
                        seminalPoints = 20,
                        projectPoints = 20,
                        additionalPoints = 0,
                        finishDate = DateTime.Parse("2022-10-6")
                    },
                    new Enrollment
                    {
                        courseId = context.Course.Single(d => d.title == "OWEB").courseId,
                        StudentID = context.Student.Single(d => d.FirstName == "Petar" && d.LastName == "Petreski").ID,
                        semester = "1",
                        year = 2021,
                        grade = 6,
                        seminalUrl = "github",
                        projectUrl = "github",
                        examPoints = 20,
                        seminalPoints = 10,
                        projectPoints = 10,
                        additionalPoints = 10,
                        finishDate = DateTime.Parse("2022-10-6")
                    },
                    new Enrollment
                    {
                        courseId = context.Course.Single(d => d.title == "RSWEB").courseId,
                        StudentID = context.Student.Single(d => d.FirstName == "Stojan" && d.LastName == "Stojanoski").ID,
                        semester ="6",
                        year = 2021,
                        grade = 10,
                        seminalUrl = "github",
                        projectUrl = "github",
                        examPoints = 100,
                        seminalPoints = 20,
                        projectPoints = 20,
                        additionalPoints = 0,
                        finishDate = DateTime.Parse("2022-10-6")
                    },
                    new Enrollment
                    {
                        courseId = context.Course.Single(d => d.title == "OWEB").courseId,
                        StudentID = context.Student.Single(d => d.FirstName == "Ilina" && d.LastName == "Naumoska").ID,
                        semester = "7",
                        year = 2021,
                        grade = 9,
                        seminalUrl = "github",
                        projectUrl = "github",
                        examPoints = 60,
                        seminalPoints = 20,
                        projectPoints = 20,
                        additionalPoints = 5,
                        finishDate = DateTime.Parse("2022-10-6")
                    },
                    new Enrollment
                    {
                        courseId = context.Course.Single(d => d.title == "Matematika 1").courseId,
                        StudentID = context.Student.Single(d => d.FirstName == "Marija" && d.LastName == "Mitreska").ID,
                        semester = "7",
                        year = 2021,
                        grade = 5,
                        seminalUrl = "github",
                        projectUrl = "github",
                        examPoints = 10,
                        seminalPoints = 0,
                        projectPoints = 0,
                        additionalPoints = 0,
                        finishDate = DateTime.Parse("2022-10-6")
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
