using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolAppCoreMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.NETCore;
namespace SchoolAppCoreMVC.Controllers
{
    public class ReportController : Controller
    {
        private readonly SchoolContext _context;

        public ReportController(SchoolContext context)
        {
            _context = context;
        }

        // GET: /Report
        public IActionResult Index()
        {
            var model = new ReportViewModel
            {
                Courses = _context.Courses.Select(c => new SelectListItem
                {
                    Value = c.CourseID.ToString(),
                    Text = c.Title
                }).ToList()
            };
            return View(model);
        }

        // POST: /Report/Generate
        [HttpPost]
        public IActionResult Generate(int SelectedCourseID, string action)
        {
            var students = (from s in _context.Students
                            join e in _context.Enrollments on s.StudentID equals e.StudentID
                            where e.CourseID == SelectedCourseID
                            select new
                            {
                                s.StudentID,
                                s.FirstName,
                                s.LastName,
                                e.Grade
                            }).ToList();

            var report = new LocalReport();
            string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "StudentReport.rdlc");
            report.ReportPath = reportPath;

            report.DataSources.Add(new ReportDataSource("StudentDataSet", students));

            byte[] pdfBytes = report.Render("PDF");

            if (action == "preview")
            {
                Response.Headers.Append("Content-Disposition", "inline; filename=StudentReport.pdf");
                return File(pdfBytes, "application/pdf");
            }
            else
            {
                return File(pdfBytes, "application/pdf", "StudentReport.pdf");
            }
        }
    }
}