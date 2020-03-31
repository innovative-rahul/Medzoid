using MedzoidSite.Models;
using MedzoidSite.ViewModel;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MedzoidSite.Areas.Doctors.Controllers
{
    public class DoctorController : Controller
    {
        private MedzoidEntities db = new MedzoidEntities();


        public ActionResult DoctorDashboard()
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];

            if (userSession == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            var orderCount = db.Prescriptions.Join(db.PharmacyPrescriptions, a => a.id, b => b.prescriptionId, (a, b) => new { a, b }).Where(a => a.a.userId == userSession.userDetails.Id && a.b.PrescriptionStatus == "Accepted").Count();

            string uid = userSession.userDetails.Id.ToString();
            var appointment = new int();
            if (userSession.userDetails.UserType == "D")
            {
                appointment = db.make_appointment.Where(a => a.doctor_id == userSession.userDetails.Id).Count();
            }
            else
            {
                appointment = db.make_appointment.Where(a => a.user_id == uid).Count();
            }


            var wallet = db.wallets.Where(a => a.userid == userSession.userDetails.Id).Select(a =>
                           new { userid = a.userid, amount = a.amount ?? 0, transfertype = a.transtype == 1 ? "Credited" : "Debit", transdate = a.transdate ?? "", message = a.message ?? "" }).ToList();

            var totalDebited = wallet.Where(a => a.transfertype == "Debit").Sum(a => a.amount);
            var totalCredited = wallet.Where(a => a.transfertype == "Credited").Sum(a => a.amount);

            var availableAmount = Convert.ToInt32(totalCredited) + Convert.ToInt32(totalDebited);

            var favCount = db.FavouriteDoctors.Where(a => a.UserID == userSession.userDetails.Id).Count();

            DashboardViewModel dashboard = new DashboardViewModel()
            {
                Appointment = appointment,
                FavDoctors = favCount,
                Orders = orderCount,
                WalletAmount = availableAmount
            };
            return View(dashboard);
        }


        public ActionResult DoctorProfile()
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];
            if (userSession == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            return View();
        }


        [HttpGet]
        public ActionResult Appointment()
        {

            AfterLoginViewModel user = (AfterLoginViewModel)Session["Details"];
            if (user == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            else
            {

                List<AppointmentViewModel> listUserAppointmentViewModel = new List<AppointmentViewModel>();

                try
                {
                    
                    var appointments = db.make_appointment.Where(a => a.doctor_id == user.userDetails.Id)
                        .Select(a => new
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


        public ActionResult MyWallet()
        {
            AfterLoginViewModel user = (AfterLoginViewModel)Session["Details"];

            if (user == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            WalletViewModel wvm = new WalletViewModel();
            try
            {
                var wallet = db.wallets.Where(a => a.userid == user.userDetails.Id)
                           .Select(a =>
                           new Wallet
                           {
                               userId = a.userid,
                               amount = a.amount ?? 0,
                               TransferType = a.transtype == 1 ? "Credited" : "Debit",
                               TransferDate = a.transdate ?? "",
                               Message = a.message ?? ""

                           }).ToList();


                decimal totalDebited = wallet.Where(a => a.TransferType == "Debit").Sum(a => a.amount);
                decimal totalCredited = wallet.Where(a => a.TransferType == "Credited").Sum(a => a.amount);

                decimal availableAmount = Convert.ToInt32(totalCredited) + Convert.ToInt32(totalDebited);

                wvm.walletDetails = wallet;
                wvm.TotalAvailableAmount = availableAmount;
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

            return View(wvm);
        }


        [HttpPost]
        public ActionResult AddAmount(WalletViewModel model)
        {
            AfterLoginViewModel user = (AfterLoginViewModel)Session["Details"];
            if (user == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            try
            {
                int amount = model.Amount;
                int TotalAmount = Convert.ToInt32(amount * 100);

                Dictionary<string, object> input = new Dictionary<string, object>();
                input.Add("amount", TotalAmount); // this amount should be same as transaction amount
                input.Add("currency", "INR");
                input.Add("receipt", "1220");
                input.Add("payment_capture", 1);

                string key = "rzp_live_Q8Tdjuv7d3Wvt9";
                string secret = "Y1PNVuiEEPSuO0cqhYJn5c0v";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                RazorpayClient client = new RazorpayClient(key, secret);

                Order order = client.Order.Create(input);
                string orderId = order["id"].ToString();


                string outHTML = "<form action = 'https://medzoid.com/Doctors/Doctor/GatewayResponse?uid=" + user.userDetails.Id + "' method = 'post'>";
                outHTML += "<script src = 'https://checkout.razorpay.com/v1/checkout.js'";
                outHTML += " data-key = 'rzp_live_Q8Tdjuv7d3Wvt9'";
                outHTML += " data-amount = '" + TotalAmount + "'";
                outHTML += " data-name = 'Razorpay'";
                outHTML += " data-description = 'Amount added to Wallet'";
                outHTML += " data-order_id ='" + orderId + "'";
                outHTML += " data-image = 'https://razorpay.com/favicon.png'";
                outHTML += " data-prefill.name = '" + user.userDetails.Name + "'";
                outHTML += " data-prefill.email = '" + user.userDetails.Email + "'";
                outHTML += " data-prefill.contact = '" + user.userDetails.Mobile + "'";
                outHTML += " data-theme.color = '#F37254'></script>";
                outHTML += "<input type = 'hidden' value = 'Hidden Element' name = 'hidden' >";
                outHTML += "</form>";


                ViewBag.htmlData = outHTML;
                ViewBag.amount = amount;
                ViewBag.orderId = order["id"].ToString();
            }
            catch (Exception ex)
            {
                tblException exception = new tblException()
                {
                    creationDate = DateTime.Now,
                    Error = ex.InnerException.ToString()
                };
                using (var db = new MedzoidEntities())
                {
                    db.tblExceptions.Add(exception);
                    db.SaveChanges();
                }

            }
            return View("PaymentPage");

            // return Json(new { Success = true, msg = "Done", orderId = ViewBag.orderId, TotalAmount = TotalAmount }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GatewayResponse()
        {
            AfterLoginViewModel model = new AfterLoginViewModel();
            int uid = Convert.ToInt32(Request.QueryString["uid"]);
            var user = db.UserMasters.Where(a => a.id == uid).FirstOrDefault();
            if (user != null)
            {
                UserMasterVM userVM = new UserMasterVM()
                {
                    Id = user.id,
                    Mobile = user.mobile,
                    Name = user.name,
                    Password = user.password,
                    UserType = user.user_type,
                    Email = user.email,
                    Gender = user.gender,
                    BloodDonor = user.blood_donate,
                    BloodGroup = user.blood_group,
                    DOB = user.date_of_birth,
                    Address = user.address
                };

                model.userDetails = userVM;

                Session["Details"] = model;

                string paymentId = Request.Form["razorpay_payment_id"];

                Dictionary<string, object> input = new Dictionary<string, object>();
                input.Add("amount", 500); // this amount should be same as transaction amount

                string key = "rzp_live_Q8Tdjuv7d3Wvt9";
                string secret = "Y1PNVuiEEPSuO0cqhYJn5c0v";

                RazorpayClient client = new RazorpayClient(key, secret);

                Dictionary<string, string> attributes = new Dictionary<string, string>();

                attributes.Add("razorpay_payment_id", paymentId);
                attributes.Add("razorpay_order_id", Request.Form["razorpay_order_id"]);
                attributes.Add("razorpay_signature", Request.Form["razorpay_signature"]);

                Utils.verifyPaymentSignature(attributes);

                var payment = client.Payment.Fetch(paymentId);
                var paymentAmount = payment["amount"] / 100;

                wallet wallet = new wallet()
                {
                    userid = uid,
                    amount = paymentAmount,
                    message = "Your wallet is credited with Rs " + paymentAmount + " by add wallet",
                    transtype = 1,
                    transdate = DateTime.Now.ToString(),
                    STATUS = true
                };
                db.wallets.Add(wallet);
                db.SaveChanges();
            }

            //Refund refund = new Razorpay.Api.Payment((string)paymentId).Refund();
            //Console.WriteLine(refund["id"]);

            return RedirectToAction("MyWallet");
        }


        // GET: Doctor
        public ActionResult MyPractise()
        {
            return View();
        }

        //public ActionResult GetDoctorPractiseDetails()
        //{
        //    AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];
        //    if (userSession == null)
        //    {
        //        return RedirectToAction("Login","Home");
        //    }

        //    DoctorPractiseViewModel model = new DoctorPractiseViewModel();


        //    Appointment appointment = new Appointment();

        //    DoctorPractiseParam param = new DoctorPractiseParam();
        //    param.Id = userSession.userDetails.Id;

        //    DoctorPractiseViewModel app = appointment.GetAppointmentCount(param);
        //    model.TotalAppointments = app.TotalAppointments;
        //    model.TotalOffline = app.TotalOffline;
        //    model.TotalOnline = app.TotalOnline;

        //    return Json(new { Success = true , data = model }, JsonRequestBehavior.AllowGet);
        //}


        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(UserMaster userMaster)
        {
            AfterLoginViewModel user = (AfterLoginViewModel)Session["Details"];

            if (user == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

            var userObj = db.UserMasters.Where(a => a.id == user.userDetails.Id).FirstOrDefault();
            if (userObj != null)
            {
                userObj.password = userMaster.password;
                db.Entry(userObj).State = EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAppointmentDetails(DoctorPractiseParam param)
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];
            if (userSession == null)
            {
                return RedirectToAction("Login", "Home");
            }


            DoctorPractiseViewModel model = new DoctorPractiseViewModel();

            var clinic = db.ClinicDetails.Where(a => a.doc_id == userSession.userDetails.Id && a.IsDeleted == false).Select(a => new ClinicsDetails { Id = a.id, Name = a.hospital_name }).ToList();
            if (clinic.Count > 0)
            {
                model.clinics = clinic;
            }

            var clinicSchedules = db.clinicSchedules.Where(a => a.docId == userSession.userDetails.Id && a.clinicId == param.ClinicId && a.Isdeleted == false)
                                                    .Select(a => new TimeSchedule { Id = a.id, Time = a.StartTime + "-" + a.EndTime }).ToList();

            if (clinicSchedules.Count > 0)
            {
                model.timeschedule = clinicSchedules;
            }

            param.Id = userSession.userDetails.Id;
            Appointment appointment = new Appointment();
            DoctorPractiseViewModel app = appointment.GetAppointmentCount(param);
            model.TotalAppointments = app.TotalAppointments;
            model.TotalOffline = app.TotalOffline;
            model.TotalOnline = app.TotalOnline;

            return Json(new { Success = true, data = model }, JsonRequestBehavior.AllowGet);
        }
    }
}