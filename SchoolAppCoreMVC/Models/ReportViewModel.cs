using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolAppCoreMVC.Models
{
    public class ReportViewModel
    {
        public int SelectedCourseID { get; set; }
        public List<SelectListItem> Courses { get; set; }
    }
}
