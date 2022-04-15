using N01521496_C1_w22.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        //******************* C2 **************************************

        //[Route("/TeacherDisplay/DeleteConfirm/{teacherId}")]
        public ActionResult DeleteConfirm(int id)
        {
            // accessing data layer
            TeacherDataController controller = new TeacherDataController();
            // getting single teacher
            TeacherObject currentTeacher = controller.FindOne(id);
            // returing currentTeacher to Show View
            return View(currentTeacher);
        }

        // after confirming delete we will delete teacher
        // after deleting we will redirect to all teacher list
        [HttpPost]
        public ActionResult Delete(int id)
        {
            // accessing data layer
            TeacherDataController controller = new TeacherDataController();
            // deleting single teacher
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }


        //user clicking add teacher button on list page will get handled using New controller
        // GET: /TeacherDisplay/New
        [HttpGet]
        public ActionResult New()
        {
            return View();
        }


        //POST: /TeacherDisplay/Create
        // when user click add teacher button it will be post method and will be handled by Create method
        [HttpPost]

        public ActionResult Create(string f_name, string l_name, string empNum, double salary)
        {
            Debug.WriteLine("fname is :" + f_name);
            TeacherObject newTeacher = new TeacherObject();
            newTeacher.TeacherFname = f_name;
            newTeacher.TeacherLname = l_name;
            newTeacher.EmployeeNumber = empNum;
            newTeacher.Salary = salary;

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(newTeacher);
            return RedirectToAction("List");
        }

        // GET: TeacherDisplay/Edit
        // this will show edit page to user with teacher of selected id
        /// <summary>
        /// This will return a page where user can edit exsisting teacher data
        /// </summary>
        /// <param name="id">teacher id on which update is needed to be applied</param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            // accessing data layer
            TeacherDataController controller = new TeacherDataController();
            // getting single teacher
            TeacherObject currentTeacher = controller.FindOne(id);
            return View(currentTeacher);
        }

        // POST: /TeacherDisplay/Update/{id}
        /// <summary>
        /// this will be called when actual edit button is clicked to save edited info
        /// it will redirect to "Show"
        /// </summary>
        /// <param name="id"></param>
        /// <param name="f_name"></param>
        /// <param name="l_name"></param>
        /// <param name="empNum"></param>
        /// <param name="salary"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(int id, string f_name, string l_name, string empNum, double salary)
        {
            // storing new incoming data to teacher object
            TeacherObject teacherInfo = new TeacherObject();
            teacherInfo.TeacherFname = f_name;
            teacherInfo.TeacherLname = l_name;
            teacherInfo.EmployeeNumber = empNum;
            teacherInfo.Salary = salary;

            // sending data and processing it using TeacherDataController
            TeacherDataController controller = new TeacherDataController();
            controller.UpdateTeacher(id, teacherInfo);
            return RedirectToAction("Show/" + id);
        }
    }
}