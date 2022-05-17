#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProektRS.Data;
using ProektRS.Models;
using ProektRS.ViewModels;


namespace ProektRS.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ProektRSContext _context;

        public EnrollmentsController(ProektRSContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var proektRSContext = _context.Enrollment.Include(e => e.Course).Include(e => e.Student);
            return View(await proektRSContext.ToListAsync());
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // GET: Enrollments/Create

        public IActionResult Create()
        {
            ViewData["courseId"] = new SelectList(_context.Course, "courseId", "title");
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "fullname");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("ID,courseId,StudentID,semester,year,grade,seminalUrl,projectUrl,examPoints,seminalPoints,projectPoints,additionalPoints,finishDate,student,course")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["courseId"] = new SelectList(_context.Course, "courseId", "title", enrollment.courseId);
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "StudentID", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewData["courseId"] = new SelectList(_context.Course, "courseId", "title", enrollment.courseId);
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "FirstName", enrollment.StudentID);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,courseId,StudentID,semester,year,grade,seminalUrl,projectUrl,examPoints,seminalPoints,projectPoints,additionalPoints,finishDate")] Enrollment enrollment)
        {
            if (id != enrollment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["courseId"] = new SelectList(_context.Course, "courseId", "title", enrollment.courseId);
            ViewData["StudentID"] = new SelectList(_context.Student, "ID", "FirstName", enrollment.StudentID);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment
                .Include(e => e.Course)
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollment = await _context.Enrollment.FindAsync(id);
            _context.Enrollment.Remove(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentExists(int id)
        {
            return _context.Enrollment.Any(e => e.ID == id);
        }

        // GET: Enrollments/StudentsEnrolledAtCourse/5/MarijaStefanoska
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> StudentsEnrolledAtCourse(int? id, string teacher, int year)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.courseId == id);

            string[] names = teacher.Split(" ");
            var teacherModel = await _context.Teacher.FirstOrDefaultAsync(m => m.firstName == names[0] && m.lastName == names[1]);
            ViewBag.teacher = teacher;
            ViewBag.course = course.title;
            var enrollment = _context.Enrollment.Where(x => x.courseId == id && (x.Course.firstTeacherId == teacherModel.teacherId || x.Course.secondTeacherId == teacherModel.teacherId))
            .Include(e => e.Course)
            .Include(e => e.Student);
            await _context.SaveChangesAsync();
            IQueryable<int?> yearsQuery = _context.Enrollment.OrderBy(m => m.year).Select(m => m.year).Distinct();
            IQueryable<Enrollment> enrollmentQuery = enrollment.AsQueryable();
            if (year != null && year != 0)
            {
                enrollmentQuery = enrollmentQuery.Where(x => x.year == year);
            }
            else
            {
                enrollmentQuery = enrollmentQuery.Where(x => x.year == yearsQuery.Max());
            }

            if (enrollment == null)
            {
                return NotFound();
            }
           
            EnrollmentFilter viewmodel = new EnrollmentFilter
            {
                enrollments = await enrollmentQuery.ToListAsync(),
                yearsList = new SelectList(await yearsQuery.ToListAsync())
            };
          
            return View(viewmodel);
        }

        //THIS IS FOR TEACHER ROLE
        // GET: Enrollments/EditAsTeacher/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> EditAsTeacher(long? id, string teacher)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = await _context.Enrollment.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }
            ViewBag.teacher = teacher;
            ViewData["courseId"] = new SelectList(_context.Course, "courseId", "title", enrollment.courseId);
            ViewData["studentId"] = new SelectList(_context.Student, "Id", "firstName", enrollment.StudentID);
            return View(enrollment);
        }

        //THIS IS FOR TEACHER ROLE
        // POST: Enrollments/EditAsTeacher/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> EditAsTeacher(long id, string teacher, [Bind("enrollmentId,courseId,studentId,semester,year,grade,seminalUrl,projectUrl,examPoints,seminalPoints,projectPoints,additionalPoints,finishDate")] Enrollment enrollment)
        {
            if (id != enrollment.ID)
            {
                return NotFound();
            }
            string temp = teacher;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(enrollment.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("StudentsEnrolledAtCourse", new { id = enrollment.courseId, teacher = temp, year = enrollment.year });
            }
            ViewData["courseId"] = new SelectList(_context.Course, "courseId", "title", enrollment.courseId);
            ViewData["studentId"] = new SelectList(_context.Student, "Id", "firstName", enrollment.StudentID);
            return View(enrollment);
        }

        //THIS IS FOR STUDENT ROLE
        // GET: Enrollments/StudentCourses/5
        [Authorize(Roles = "Admin,Student")]
        public async Task<IActionResult> StudentCourses(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.ID == id);

            ViewBag.student = student.fullName;

            IQueryable<Enrollment> enrollment = _context.Enrollment.Where(x => x.StudentID == id)
            .Include(e => e.Course)
            .Include(e => e.Student);
            await _context.SaveChangesAsync();

            if (enrollment == null)
            {
                return NotFound();
            }

            return View(await enrollment.ToListAsync());
        }

        //THIS IS FOR STUDENT ROLE
        // GET: Enrollments/EditAsTeacher/5
        public async Task<IActionResult> EditAsStudent(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollment = _context.Enrollment.Where(m => m.ID == id).Include(x => x.Student).Include(x => x.Course).First();
            IQueryable<Enrollment> enrollmentQuery = _context.Enrollment.AsQueryable();
            enrollmentQuery = enrollmentQuery.Where(m => m.ID == id);
            if (enrollment == null)
            {
                return NotFound();
            }

            EditAsStudentViewModel viewmodel = new EditAsStudentViewModel
            {
                enrollment = await enrollmentQuery.Include(x => x.Student).Include(x => x.Course).FirstAsync(),
                seminalUrlName = enrollment.seminalUrl
            };
            ViewData["courseId"] = new SelectList(_context.Course, "courseId", "title", enrollment.courseId);
            ViewData["studentId"] = new SelectList(_context.Student, "Id", "firstName", enrollment.StudentID);
            return View(viewmodel);
        }

        //THIS IS FOR STUDENT ROLE
        // POST: Enrollments/EditAsTeacher/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsStudent(long id, EditAsStudentViewModel viewmodel)
        {
            if (id != viewmodel.enrollment.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (viewmodel.seminalUrlFile != null)
                    {
                        string uniqueFileName = UploadedFile(viewmodel);
                        viewmodel.enrollment.seminalUrl = uniqueFileName;
                    }
                    else
                    {
                        viewmodel.enrollment.seminalUrl = viewmodel.seminalUrlName;
                    }

                    _context.Update(viewmodel.enrollment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentExists(viewmodel.enrollment.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("StudentCourses", new { id = viewmodel.enrollment.StudentID });
            }

            ViewData["courseId"] = new SelectList(_context.Course, "courseId", "title", viewmodel.enrollment.courseId);
            ViewData["studentId"] = new SelectList(_context.Student, "Id", "firstName", viewmodel.enrollment.StudentID);
            return View(viewmodel);
        }

        private string UploadedFile(EditAsStudentViewModel viewmodel)
        {
            string uniqueFileName = null;

            if (viewmodel.seminalUrlFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/seminals");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(viewmodel.seminalUrlFile.FileName);
                string fileNameWithPath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    viewmodel.seminalUrlFile.CopyTo(stream);
                }
            }
            return uniqueFileName;
        }

    }
}
