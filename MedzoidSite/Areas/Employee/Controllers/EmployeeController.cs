using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedzoidSite.Models;
using MedzoidSite.ViewModel;

namespace MedzoidSite.Areas.Employee.Controllers
{
    public class EmployeeController : Controller
    {

        private MedzoidEntities db = new MedzoidEntities();
        // GET: Employee/Employee
        public ActionResult Dashboard()
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];

            if (userSession == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            using (var db = new MedzoidEntities())
            {
                var employee = db.doc_employee_master.Where(a => a.id == userSession.userDetails.Id).FirstOrDefault();
                EmployeeDashboardViewModel dashboard = new EmployeeDashboardViewModel();
                if (employee != null)
                {
                    dashboard.Appointment = db.make_appointment.Where(a => a.clinic_id == employee.ClinicId && a.doctor_id == employee.docId).Count();
                    dashboard.Orders = db.PharmacyPrescriptions.Where(a => a.PrescriptionStatus == "Accepted").Count();
                }


                return View(dashboard);
            }
        }




        public ActionResult EmpProfile()
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];

            if (userSession == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            return View();
        }

        public ActionResult EmpAppointment()
        {
            AfterLoginViewModel user = (AfterLoginViewModel)Session["Details"];
            if (user == null)
            {
                return RedirectToAction("Login","Home");
            }
            else
            {

                List<AppointmentViewModel> listUserAppointmentViewModel = new List<AppointmentViewModel>();

                try
                {
                    string uid = user.userDetails.Id.ToString();

                    var employee = db.doc_employee_master.Where(a => a.id == user.userDetails.Id).FirstOrDefault();

                    if(employee !=null)
                    {
                        var appointments = db.make_appointment.Where(a => a.clinic_id == employee.ClinicId  && a.doctor_id == employee.docId).Select(a => new
                                                                                            {
                                                                                                appointmentStatus = a.AppointmentStatus,
                                                                                                Date = a.date,
                                                                                                PatientAge = a.PatientAge ?? 0,
                                                                                                PatientName = a.PatientName ?? "",
                                                                                                PatientSex = a.PatientSex ?? "",
                                                                                                QueueNo = a.QueueNo,
                                                                                                ClinicId = a.clinic_id,
                                                                                                docId = a.doctor_id,
                                                                                                Id = a.id,
                                                                                                UserId = a.user_id,
                                                                                                AppointmentType = a.AppointmentType ?? "",
                                                                                                scheduleId = a.scheduleId ?? 0
                                                                                            }).ToList().OrderByDescending(a => a.Date);

                        int? uId = Convert.ToInt32(user.userDetails.Id);
                        var reviews = db.ReviewBanks.Where(a => a.reviewby == uId).ToList();

                        if (appointments != null)
                        {
                            foreach (var appointment in appointments)
                            {

                                int appointmentNo = 0;
                                var app = appointments.Where(a => a.Date == appointment.Date).ToList();

                                var result = app.AsEnumerable().Select((x, index) => new { index, x.Id, x.UserId });
                                if (app.Count > 0)
                                {
                                    var res = result.Where(a => a.Id == appointment.Id).FirstOrDefault();

                                    appointmentNo = res.index + 1;
                                }




                                bool IsReviewMade = false;
                                if (reviews.Count > 0)
                                {
                                    int? doctId = Convert.ToInt32(appointment.docId);
                                    var reviewforDoc = reviews.Where(a => a.reviewfor == doctId).FirstOrDefault();
                                    if (reviewforDoc != null)
                                    {
                                        IsReviewMade = true;
                                    }
                                }

                                DateTime dat3 = Convert.ToDateTime(appointment.Date);

                                AppointmentViewModel userAppointmentViewModel = new AppointmentViewModel();
                                userAppointmentViewModel.AppointmentStatus = appointment.appointmentStatus;
                                userAppointmentViewModel.Date = dat3.ToString("dd/MM/yyyy");
                                userAppointmentViewModel.PatientAge = appointment.PatientAge;
                                userAppointmentViewModel.PatientName = appointment.PatientName ?? "";
                                userAppointmentViewModel.PatientSex = appointment.PatientSex ?? "";
                                userAppointmentViewModel.QueueNo = appointment.QueueNo;
                                userAppointmentViewModel.ClinicId = appointment.ClinicId;
                                userAppointmentViewModel.docId = appointment.docId;
                                userAppointmentViewModel.Id = appointment.Id;
                                userAppointmentViewModel.UserId = appointment.UserId;
                                userAppointmentViewModel.AppointmentType = appointment.AppointmentType ?? "";
                                userAppointmentViewModel.ReviewMade = IsReviewMade;
                                userAppointmentViewModel.AppointmentNo = appointmentNo.ToString();

                                int docId = Convert.ToInt32(appointment.docId);
                                var docName = db.DoctorMasters.Where(a => a.docId == docId).FirstOrDefault();
                                if (docName != null)
                                {
                                    userAppointmentViewModel.DocName = docName.name;
                                }
                                else
                                {
                                    userAppointmentViewModel.DocName = "";
                                }

                                var clinicDetails = db.ClinicDetails.Where(a => a.doc_id == docId).FirstOrDefault();
                                if (clinicDetails != null)
                                {
                                    userAppointmentViewModel.ClinicName = clinicDetails.hospital_name;
                                    userAppointmentViewModel.ClinicAddress = clinicDetails.hospital_address;
                                }
                                else
                                {
                                    userAppointmentViewModel.ClinicName = "";
                                    userAppointmentViewModel.ClinicAddress = "";
                                }

                                var clinic = db.clinicSchedules.Where(a => a.id == appointment.scheduleId).FirstOrDefault();
                                if (clinic != null)
                                {
                                    userAppointmentViewModel.AppointmentSchedule = clinic.StartTime + "-" + clinic.EndTime;
                                }
                                else
                                {
                                    userAppointmentViewModel.AppointmentSchedule = "";
                                }

                                listUserAppointmentViewModel.Add(userAppointmentViewModel);
                            }
                        }
                    }


                }

                catch (Exception ex)
                {
                    tblException exception = new tblException()
                    {
                        Error = ex.Message,
                        creationDate = DateTime.Now
                    };
                    db.tblExceptions.Add(exception);
                    db.SaveChanges();
                }
                return View(listUserAppointmentViewModel);
            }
        }

        public ActionResult EmpChangePassword()
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];

            if (userSession == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }


            return View();
        }


    }
}