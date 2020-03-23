using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NowaAplikacja.Controllers
{
    public class ExampleController : Controller
    {
 // GET: Example
        public ActionResult Index()
        {
            ViewBag.StatusMessage = "To jest strona dostępna dla wszystkich.";
            return View();
        }
        [Authorize(Roles = "Teacher")]
        public ActionResult OnlyTeachers()
        {
            ViewBag.StatusMessage = "To jest strona dostępna tylko dla nauczycieli.";
        return View();
        }
        [Authorize(Roles = "Student")]
        public ActionResult OnlyStudents()
        {
            ViewBag.StatusMessage = "To jest strona dostępna tylko dla studentów.";
        return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult OnlyAdmin()
        {
            ViewBag.StatusMessage = "To jest strona dostępna tylko dla administratora.";
        return View();
        }
        [Authorize(Roles = "Student,Teacher")]
        public ActionResult StudentsAndTeachers()
        {
            ViewBag.StatusMessage = "To jest strona dostępna dla studentów i nauczycieli.";
        return View();
        }
    }
}