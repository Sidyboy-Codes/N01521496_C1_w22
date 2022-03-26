using N01521496_C1_w22.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace N01521496_C1_w22.Controllers
{
    public class TeacherDisplayController : Controller
    {
        // GET: TeacherDisplay
        public ActionResult Index()
        {
            return View();
        }

        // GET: TeacherDisplay/List/{searchKey?}
        [HttpGet]
        [Route("TeacherDisplay/List/{searchKey?")]
        public ActionResult List(string searchKey=null)
        {
            // accessing Data layer
            TeacherDataController controller = new TeacherDataController();

            // getting list of teachers using ListTeachers methon
            List<TeacherObject> allTeachers = controller.ListTeachers(searchKey);

            //returning list data to view
            return View(allTeachers);
        }

        //[HttpGet]
        //[Route("/TeacherDisplay/Show/{teacherId}")]

        // I wanted to know why dont "Route" method works here, i faced same issue like u did so using default routing
        public ActionResult Show(int id)
        {
            // accessing data layer
            TeacherDataController controller = new TeacherDataController();
            // getting single teacher
            TeacherObject currentTeacher = controller.FindOne(id);
            // returing currentTeacher to Show View
            return View(currentTeacher);
        }
    }
}