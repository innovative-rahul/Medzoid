using MedzoidSite.Models;
using MedzoidSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedzoidSite.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Dashboard()
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];

            if (userSession == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            using (var db = new MedzoidEntities())
            {
                var doctors = db.DoctorMasters.Where(a => a.IsDeleted == false).ToList();

                AdminDashboardViewModel dashboard = new AdminDashboardViewModel()
                {
                    Doctors = doctors.Where(a => a.IsDeleted == false).Count(),
                    Users = db.UserMasters.Where(a => a.IsDeleted == false).Count(),
                    Orders = db.PharmacyPrescriptions.Where(a => a.PrescriptionStatus == "Accepted").Count(),
                    Prescriptions = db.Prescriptions.Where(a => a.IsApproved == "1").Count()
                };
                return View(dashboard);
            }
        }

        public ActionResult unclaimeddoctor()
        {

             AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];
            if (userSession == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" , session = "0"});
            }

            DoctorRegistrationViewModel viewModel = new DoctorRegistrationViewModel();
            if (TempData["ID"] != null)
            {
                int docId = Convert.ToInt32(TempData["ID"]);
                using (var db = new MedzoidEntities())
                {
                    var data = db.UserMasters.Join(db.DoctorMasters, a=>a.id, b=>b.docId, (a,b) => new { a,b} ).Where(a => a.b.id == docId).FirstOrDefault();



                    var doc = new DoctorRegistrationViewModelInput()
                    {
                        id = data.b.id,
                        mci_No = data.b.mci_no,
                        AboutUs = data.b.AboutUs,
                        OnLineAppointment = data.b.online_appointment,
                        deptId = data.b.dept_id,
                        experience = data.b.experience,
                        Fee = data.b.Fee,
                        HosptalAffiliation = data.b.HospitalAffiliation,
                        Membership = data.b.Membership,
                        name = data.a.name,
                        Locality = data.a.address != null ? data.a.address.Split('$')[0] : "",
                        address = data.a.address != null ? data.a.address.Split('$')[1] : ""

                    };
                    viewModel.Doctor = doc;
                }
            }

          
            using (var db = new MedzoidEntities())
            {
                viewModel.UnclaimeDoctors = db.DoctorMasters.Where(a => a.Isclaimed == false  && a.IsDeleted == false).ToList();
                return View(viewModel);
            }
        }

        public ActionResult EditDoctor(int id)
        {
            TempData["ID"] = id;
            return RedirectToAction("unclaimeddoctor");
        }

        public ActionResult RegisterUnClaimedDoctors(DoctorRegistrationViewModelInput model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MedzoidEntities())
                {
                    using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var userCount = db.UserMasters.Count();
                            userCount += 1;

                            UserMaster users = new UserMaster();

                            users.name = model.name;
                            users.address = model.Locality + "$" + model.address;
                            users.blood_donate = model.blood_donate;
                            users.blood_group = model.blood_group;
                            users.date_of_birth = model.date_of_birth;
                            users.email = userCount.ToString();
                            users.gcmkey = "";
                            users.gender = model.gender;
                            users.mobile = userCount.ToString();
                            users.user_type = "D";
                            users.used_ref = "";
                            users.ref_code = "";
                            users.password = "";
                            users.latlong = model.latlong;
                            users.creation_date = DateTime.Now;
                            users.IsDeleted = false;
                            users.IsActive = true;
                            
                            db.UserMasters.Add(users);
                            db.SaveChanges();

                            DoctorMaster doctors = new DoctorMaster()
                            {
                                docId = users.id,
                                experience = model.experience,
                                dept_id = model.deptId,
                                AboutUs = model.AboutUs,
                                Fee = model.Fee,
                                HospitalAffiliation = model.HosptalAffiliation,
                                mci_no = model.mci_No,
                                Membership = model.Membership,
                                name = model.name,
                                creation_date = DateTime.Now,
                                IsActive = true,
                                IsApproved = false,
                                Isclaimed = false,
                                IsDeleted = false,
                                online_appointment = model.OnLineAppointment
                            };

                            db.DoctorMasters.Add(doctors);
                            db.SaveChanges();

                            if (model.deptId != null)
                            {
                                string[] dept = model.deptId.TrimEnd(',').Split(',');

                                for (int i = 0; i < dept.Length; i++)
                                {
                                    DoctorDeptMaster doctorDept = new DoctorDeptMaster()
                                    {
                                        dept_id = Convert.ToInt32(dept[i]),
                                        doc_id = users.id,
                                        IsActive = true,
                                        IsDeleted = false,
                                        CreationDate = DateTime.Now
                                    };
                                    db.DoctorDeptMasters.Add(doctorDept);
                                    db.SaveChanges();
                                }
                            }
                            dbTran.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTran.Rollback();
                            tblException exception = new tblException()
                            {
                                Error = "Add UnClaimed " + ex.InnerException,
                                creationDate = DateTime.Now
                            };
                            db.tblExceptions.Add(exception);
                            db.SaveChanges();
                        }
                    }
                }
               

              
            }
            return RedirectToAction("unclaimeddoctor");
        }

        public ActionResult ClinicDetails(int id)
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];
            if (userSession == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            ClinicDetailsViewModel model = new ClinicDetailsViewModel();
            if (TempData["id"] != null)
            {
                int clinicId = Convert.ToInt32(TempData["id"]);
                using (var db = new MedzoidEntities())
                {
                    var data = db.ClinicDetails.Where(a => a.id == clinicId).FirstOrDefault();
                    model.Clinic = data;
                }
            }

            if (id == 0)
            {
                return RedirectToAction("unclaimeddoctor");
            }

            ViewBag.id = id;
            
            using (var db = new MedzoidEntities())
            {
                var clinic = db.ClinicDetails.Where(a => a.doc_id == id).ToList();
                model.ClinicList = clinic;
               
            }
            return View(model);
        }

        public ActionResult EditClinic(int id,int docId)
        {
            TempData["id"] = id;
            return RedirectToAction("ClinicDetails", new { id = docId });
        }

        public ActionResult ClinicSchedule(int? id)
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];
            if (userSession == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            if (id == 0)
            {
                return RedirectToAction("ClinicDetails");
            }

            ViewBag.clinicid = id;
            ClinicScheduleViewModel model = new ClinicScheduleViewModel();
            using (var db = new MedzoidEntities())
            {
                var clinic = db.ClinicDetails.Where(a => a.id == id).FirstOrDefault();
                ViewBag.docId = clinic.doc_id;

                var schedules = db.clinicSchedules.Where(a => a.clinicId == id && a.Isdeleted == false).ToList();
                model.scheduleList = schedules;
            }
            return View(model);
        }

        public ActionResult AddClinic(ClinicDetail model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new MedzoidEntities())
                    {
                        model.CreationDate = DateTime.Now;
                        model.IsDeleted = false;
                        model.IsActive = true;
                        db.ClinicDetails.Add(model);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    using (var db = new MedzoidEntities())
                    {
                        tblException exception = new tblException()
                        {
                            Error = "Add Clinic " + ex.InnerException,
                            creationDate = DateTime.Now
                        };
                        db.tblExceptions.Add(exception);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("ClinicDetails", new { id = model.doc_id });
        }

        public ActionResult AddSchedule(clinicSchedule model)
        {
            if (ModelState.IsValid)
            {
               
                try
                {
                    using (var db = new MedzoidEntities())
                    {
                        model.Isdeleted = false;
                        model.IsActive = true;
                        model.CreationDate = DateTime.Now;
                        db.clinicSchedules.Add(model);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    using (var db = new MedzoidEntities())
                    {
                        tblException exception = new tblException()
                        {
                            Error = "Add Schedule " + ex.InnerException,
                            creationDate = DateTime.Now
                        };
                        db.tblExceptions.Add(exception);
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("ClinicSchedule", new { id = model.clinicId });
            
        }

        public ActionResult SignOut()
        {
            Session["Details"] = null;
            return Json(new { Status = true },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SearchAddress(string searchTerm)
        {
            string predictions = CommonFunctions.SearchAddress(searchTerm);

            return Json( new { data = predictions} , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetLatLong(string placeId)
        {
            string predictions = CommonFunctions.GetLatLong(placeId);

            return Json(new { data = predictions }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Blog()
        {
            BlogViewModel blogModel = new BlogViewModel();
            dynamic mymodel = new ExpandoObject();

            using (var db = new MedzoidEntities())
            {
                try
                {
                    var blog = db.BlogNews.Where(a => a.STATUS == true).OrderByDescending(a => a.id).ToList();
                    if (blog.Count > 0)
                    {
                         blogModel.BlogList = blog;
                    }

                    return View(blogModel);


                }
                catch (Exception ex)
                {
                    tblException exception = new tblException()
                    {
                        Error = "Blogs :" + ex.InnerException,
                        creationDate = DateTime.Now
                    };
                    db.tblExceptions.Add(exception);
                    db.SaveChanges();
                }

                return View(blogModel);
            }
        }

        [ValidateInput(false)]
        public ActionResult AddBlog(BlogNew model, HttpPostedFileBase file)
        {
            using (var db = new MedzoidEntities())
            {
               
                try
                {
                    model.date = DateTime.Now;
                    model.sub_title = model.title;
                    model.auth_image = "";
                    model.url = model.title.Replace(' ', '-');
                    model.image = model.title.Replace(' ', '_') + ".jpg";
                    model.views = 1;
                    model.likes = 1;
                    model.STATUS = true;
                    db.BlogNews.Add(model);
                    db.SaveChanges();


                    var allowedExtensions = new[] { ".Jpg", ".png", ".jpg", "jpeg" };

                    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                    var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string myfile = model.sub_title.Replace(' ', '_') + ".jpg";
                        var path = Path.Combine(Server.MapPath("~/content/images/Blogs/"), myfile);
                        file.SaveAs(path);
                    }
                    else
                    {
                        ViewBag.message = "Please choose only Image file";
                    }


                }
                catch (Exception ex)
                {
                    tblException exception = new tblException()
                    {
                        creationDate = DateTime.Now,
                        Error = ex.InnerException.ToString()
                    };

                    db.tblExceptions.Add(exception);
                    db.SaveChanges();
                    
                }
            }

            return RedirectToAction("Blog");
        }

        public ActionResult HealthTips()
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];

            if (userSession == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            using (var db = new MedzoidEntities())
            {
                var healthTips = db.HealthTips.Where(a => a.IsDeleted == false).ToList();

                HealthTipsViewModel healthTipsViewModel = new HealthTipsViewModel()
                {
                    HealthTipsList = healthTips
                };
                return View(healthTipsViewModel);
            }
        }

        public ActionResult Faq()
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];

            if (userSession == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            FaqViewModel faqModel = new FaqViewModel();
            dynamic mymodel = new ExpandoObject();

            using (var db = new MedzoidEntities())
            {
                try
                {
                    var faq = db.FAqs.Where(a => a.IsDeleted == false).OrderByDescending(a => a.id).ToList();
                    if (faq.Count > 0)
                    {
                        faqModel.FaqList = faq;
                    }

                    return View(faqModel);


                }
                catch (Exception ex)
                {
                    tblException exception = new tblException()
                    {
                        Error = "Faq :" + ex.InnerException,
                        creationDate = DateTime.Now
                    };
                    db.tblExceptions.Add(exception);
                    db.SaveChanges();
                }

                return View(faqModel);
            }
        }


        [ValidateInput(false)]
        public ActionResult AddFaq(FAq model)
        {
            using (var db = new MedzoidEntities())
            {

                try
                {
                    model.CreatedDate = DateTime.Now;
                    model.IsDeleted = false;
                    model.IsUpdated = false;
                    db.FAqs.Add(model);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    tblException exception = new tblException()
                    {
                        creationDate = DateTime.Now,
                        Error = ex.InnerException.ToString()
                    };

                    db.tblExceptions.Add(exception);
                    db.SaveChanges();

                }
            }

            return RedirectToAction("Faq");
        }


        public ActionResult CheckPrescriptionManually()
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];

            if (userSession == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }



            PrescriptionViewModel model = new PrescriptionViewModel();
            using (var db = new MedzoidEntities())
            {
                var presc = db.Prescriptions.Join(db.UserMasters, a=>a.userId , b=>b.id , (a,b) => new { a,b })
                                            .Where(a => a.a.IsDeleted == false)
                                            .Select(a=> new Presc
                                                    { 
                                                      Id = a.a.id,
                                                      UserName = a.b.name,
                                                      Description = a.a.Description,
                                                      DeliveryType = a.a.DeliveryType,
                                                      Createddate = a.a.CreationDate,
                                                     }       
                                                    ).ToList();
                model.PrescList = presc;
                return View(model);
            }
                
        }

    }
}