#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProektRS.Data;
using ProektRS.Models;
using ProektRS.ViewModels;

namespace ProektRS.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ProektRSContext _context;

        public CoursesController(ProektRSContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string title, int semester, string programme)
        {
            IQueryable<Course> coursesQuery = _context.Course.AsQueryable();
            IQueryable<int> semestersQuery = _context.Course.OrderBy(m => m.semester).Select(m => m.semester).Distinct();
            IQueryable<string> programmesQuery = _context.Course.OrderBy(m => m.programme).Select(m => m.programme).Distinct();
            if (!string.IsNullOrEmpty(title))
            {
                coursesQuery = coursesQuery.Where(x => x.title.Contains(title));
            }
            if (semester != null && semester != 0)
            {
                coursesQuery = coursesQuery.Where(s => s.semester == semester);
            }
            if (!string.IsNullOrEmpty(programme))
            {
                coursesQuery = coursesQuery.Where(p => p.programme == programme);
            }
            var CoursefilterVM = new CourseFilter
            {
                courses = await coursesQuery.Include(c => c.firstTeacher).Include(c => c.secondTeacher).ToListAsync(),
                programmes = new SelectList(await programmesQuery.ToListAsync()),
                semesters = new SelectList(await semestersQuery.ToListAsync())
            };

            return View(CoursefilterVM);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.firstTeacher)
                .Include(c => c.secondTeacher)
                .FirstOrDefaultAsync(m => m.courseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["firstTeacherId"] = new SelectList(_context.Set<Teacher>(), "teacherId", "firstName");
            ViewData["secondTeacherId"] = new SelectList(_context.Set<Teacher>(), "teacherId", "firstName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("courseId,title,credits,semester,programme,educationLevel,firstTeacherId,secondTeacherId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["firstTeacherId"] = new SelectList(_context.Set<Teacher>(), "teacherId", "firstName", course.firstTeacherId);
            ViewData["secondTeacherId"] = new SelectList(_context.Set<Teacher>(), "teacherId", "firstName", course.secondTeacherId);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["firstTeacherId"] = new SelectList(_context.Set<Teacher>(), "teacherId", "firstName", course.firstTeacherId);
            ViewData["secondTeacherId"] = new SelectList(_context.Set<Teacher>(), "teacherId", "firstName", course.secondTeacherId);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("courseId,title,credits,semester,programme,educationLevel,firstTeacherId,secondTeacherId")] Course course)
        {
            if (id != course.courseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.courseId))
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
            ViewData["firstTeacherId"] = new SelectList(_context.Set<Teacher>(), "teacherId", "firstName", course.firstTeacherId);
            ViewData["secondTeacherId"] = new SelectList(_context.Set<Teacher>(), "teacherId", "firstName", course.secondTeacherId);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.firstTeacher)
                .Include(c => c.secondTeacher)
                .FirstOrDefaultAsync(m => m.courseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.courseId == id);
        }

        // GET: Teachers/TeacherCourse
        public async Task<IActionResult> TeacherCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.teacherId == id);
            /*var courses = await _context.Course
                .FirstOrDefaultAsync(m => m.firstTeacherId == id || m.secondTeacherId == id);*/
            IQueryable<Course> coursesQuery = _context.Course.Where(m => m.firstTeacherId == id || m.secondTeacherId == id);
            await _context.SaveChangesAsync();
            if (teacher == null)
            {
                return NotFound();
            }
            var CourseTitleVM = new CourseFilter
            {
                courses = await coursesQuery.ToListAsync(),
            };

            return View(CourseTitleVM);
        }
            
        }
    }

