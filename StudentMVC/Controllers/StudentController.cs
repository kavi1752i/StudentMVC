using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer;
using StudentMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentMVC.Controllers
{
    public class StudentController : Controller
    {
        IStudentRepository obj;
        public StudentController(IStudentRepository ins)
        {
            obj = ins;
        }
        // GET: StudentController
        public ActionResult Index()
        {
            try
            {

                var data = obj.Getstudentd();
                return View("AllStudent", data);  
            }
            catch (Exception ex)
            {
                var err = new ErrorViewModel();
                err.Message = ex.Message;
                return View("error", err);
            }
        }

       

        // GET: StudentController/Create
        public ActionResult Create()
        {
            var statelist = obj.GetState();
            ViewBag.states = statelist.Select(s => new SelectListItem
            {
                Text = s.StateName,
                Value = s.StateId.ToString()
            })
                        .ToList();
           
            return View("Addstudent",new StudentDetail());
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addstudent(StudentDetail p )
        {
            try
            {
                if(obj.isduplicateEmailorMobile(p.Email,p.Mobilenumber))
                {
                    ModelState.AddModelError("", "This email or mobile is already exits");
                }

                if (ModelState.IsValid)
                {
                    ViewBag.states = obj.GetState().Select(s => new SelectListItem
                    {
                        Text = s.StateName,
                        Value = s.StateId.ToString()
                    }).ToList();
                    obj.Addstudent(p);
                    return RedirectToAction(nameof(Index));
                }


                return View("Addstudent", p);
                
               
            }
            catch(Exception ex)
            {
                throw;
            }
           
        }



        public IActionResult StudentList()
        {
            var students = obj.Getstudent();
            return View("Index", students);
        }

        public IActionResult CreateEditStudent(int? id)
        {
            var student = id.HasValue ? obj.Getstudentbyid(id.Value) : new StudentDetail();
            ViewBag.states = obj.GetState()
        .Select(s => new SelectListItem
        {
            Text = s.StateName,
            Value = s.StateId.ToString()
        }).ToList();

            return View("_CreateEditStudentPartial", student);
        }

        public JsonResult SaveStudent(StudentDetail p)
        {
            if (obj.isduplicateEmailorMobileforedit(p.Id, p.Email, p.Mobilenumber))
            {
                return Json(new { success = false, message = "Email or Mobile already exists." });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Validation failed." });
            }

            if (p.Id == 0)
                obj.Addstudent(p);        // CREATE
            else
                obj.Updatestudent(p);     // UPDATE

            return Json(new { success = true });
        }

        // AJAX DELETE
        [HttpGet]
        public IActionResult Deletee(int id)
        {
            var student = obj.Getstudentbyid(id);
            return View("_Delete", student);

        }
        [HttpPost]
        public JsonResult DeleteStudents(int id)
        {
            try
            {
                obj.DeleteStudents(id);
                return Json(new { success = true });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: StudentController/Details/5
        public ActionResult Detailsstudent(int id)
        {

            var data = obj.Getstudentbyid(id);

            return View("_Details", data);
        }

        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {

            var patientdata = obj.Getstudentbyid(id);

            return View("Detailstudent", patientdata);
        }


        // GET: StudentController/Edit/5
        public ActionResult Edit(int id)
        {
            var data = obj.Getstudentbyid(id);
            ViewBag.states = obj.GetState()
           .Select(s => new SelectListItem
            {
                Text = s.StateName,
                Value = s.StateId.ToString(),
              Selected =(s.StateId==data.StateId)
            })
                        .ToList();


            return View("Editstudent",data);
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editstudent(StudentDetail p)
        {
            try
            {
                if (obj.isduplicateEmailorMobileforedit(p.Id, p.Email, p.Mobilenumber))
                {
                    ModelState.AddModelError("", "This email or mobile is already exits");
                }
                if (!ModelState.IsValid)
                {
                    ViewBag.states = obj.GetState().Select(s => new SelectListItem
                    {
                        Text = s.StateName,
                        Value = s.StateId.ToString(),
                        Selected = (s.StateId == p.StateId)
                    }).ToList();
                    return View("Editstudent", p);
                }

                    obj.Updatestudent(p);
                    return RedirectToAction(nameof(Index));
                
            }
            catch
            {
                ViewBag.states = obj.GetState().Select(s => new SelectListItem
                {
                    Text = s.StateName,
                    Value = s.StateId.ToString(),
                    Selected = (s.StateId == p.StateId)
                }).ToList();
                ModelState.AddModelError("", "an error occured");
                return View("Editstudent", p);
            }
        }

        // GET: StudentController/Delete/5
        public ActionResult Delete(int id)
        {
            var studentdata = obj.Getstudentbyid(id);
            return View("Deletestudent",studentdata);
        }

        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deletestudent(StudentDetail student)
        {
            try
            {
               
                {

                    obj.DeleteStudents(student.Id);

                    return RedirectToAction(nameof(Index));
                }
               
            }
            catch
            {
                return View(student);
            }
        }
    }
}