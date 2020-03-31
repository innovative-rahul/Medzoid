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


namespace MedzoidSite.Areas.User.Controllers
{
    public class UserController : Controller
    {
        private MedzoidEntities db = new MedzoidEntities();

        public ActionResult Dashboard()
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];

            if (userSession == null)
            {
                return RedirectToAction("Login","Home", new { area = "" });
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

      
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SignUp(UserMaster userMaster)
        {

            try
            {
                #region Save User
                var lastid = db.UserMasters.Max(a => a.id);

                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var random = new Random();
                var result = new string(
                Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
                userMaster.ref_code = "REF" + result.ToString() + lastid.ToString();
                userMaster.used_ref = "0";

                Random gen = new Random();
                var OTP = gen.Next(1001, 9999);

                userMaster.mobile = userMaster.mobile;
                userMaster.email = userMaster.mobile;
                userMaster.OTP = OTP.ToString();
                db.UserMasters.Add(userMaster);
                db.SaveChanges();
                #endregion

                #region Send OTP
                Sms sms_obj = new Sms();
                sms_obj.mobileno = userMaster.mobile;
                sms_obj.txt = OTP.ToString();
                // sms_obj.SMS_SEND("Normal");
                #endregion


                //#region check if userappointment Exist
                //var appointments = db.make_appointment.Where(a => a.user_id == userMaster.mobile).ToList();
                //if (appointments != null)
                //{
                //    foreach (var appointment in appointments)
                //    {
                //        string userid = userMaster.id.ToString();
                //        appointment.user_id = userid;
                //    }
                //}

                //#endregion

                return Json(new { Success = true, OTP = OTP.ToString(), id = userMaster.id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                if (ex.InnerException.InnerException.ToString().Contains("Violation of UNIQUE KEY constraint 'UserMaster_Mobile"))
                {
                    return Json(new { Success = false, msg = "Mobile number already in Use" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Success = false, msg = "Some error occurred. Please try again Later. " }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult VerifyOTP(OTPInputModel model)
        {
            OTPViewModel otp = new OTPViewModel();
            try
            {
                UserMaster user_master_obj = db.UserMasters.Where(a => a.OTP == model.otp && a.id == model.Id).FirstOrDefault();
                if (user_master_obj != null)
                {
                    user_master_obj.id = model.Id;
                    user_master_obj.IsActive = true;
                    int update = db.SaveChanges();


                    string retMSG = "";

                    Decimal? refamount = 0;
                    if (model.usedrefcode != "0")
                    {
                        var isvalidref = db.UserMasters.Where(a => a.ref_code == model.usedrefcode).FirstOrDefault();
                        if (isvalidref != null)
                        {
                            user_master_obj.used_ref = model.usedrefcode;
                            db.SaveChanges();

                            var offerPrice = db.refprice_master.Where(a => a.STATUS == true).OrderByDescending(a => a.id).FirstOrDefault().amount;

                            wallet walletobj = new wallet();
                            walletobj.userid = Convert.ToInt32(isvalidref.id);
                            walletobj.amount = Convert.ToDecimal(offerPrice);
                            walletobj.transtype = 1;
                            walletobj.message = "Your wallet is credited with Rs " + offerPrice.ToString() + " for user registeration using your referal code";
                            walletobj.transdate = DateTime.Now.ToString();
                            walletobj.STATUS = true;

                            var outputwal = db.wallets.Add(walletobj);
                            db.SaveChanges();

                            wallet wallet_obj = new wallet();
                            wallet_obj.userid = Convert.ToInt32(model.Id);
                            wallet_obj.amount = Convert.ToDecimal(offerPrice);
                            wallet_obj.transtype = 1;
                            wallet_obj.message = "Your wallet is credited with Rs " + offerPrice.ToString() + " for registration with referal code";
                            wallet_obj.transdate = DateTime.Now.ToString();
                            wallet_obj.STATUS = true;
                            var outputwal1 = db.wallets.Add(wallet_obj);
                            db.SaveChanges();

                            if (outputwal != null)
                            {
                                refamount = offerPrice;
                                retMSG = "Congratulations! Your referal code is applied successfully'";
                            }
                        }
                    }


                    var total_credited = db.wallets.Where(a => a.transtype == 1 && a.userid == user_master_obj.id).Sum(a => a.amount);
                    var total_debited = db.wallets.Where(a => a.transtype == 0 && a.userid == user_master_obj.id).Sum(a => a.amount);

                    decimal avail_bal = Convert.ToDecimal(total_credited) - Convert.ToDecimal(total_debited);


                    otp.Id = model.Id;
                    otp.referal_amount = refamount.ToString();
                    otp.wallet_amount = avail_bal.ToString();
                    otp.Msg = "OTP Verify Successfully";
                    otp.Ref_Msg = retMSG;

                }
                else
                {
                    return Json(new { Success = false, msg = "Incorrect OTP", OTP = otp }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // viewModel.Status = false;
                // viewModel.Message = ex.Message;
                // viewModel.Data = new object();
            }

            return Json(new { Success = true, msg = "Done", OTP = otp }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateUserProfileType(string userType)
        {
            try
            {
                AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];
                if (userSession == null)
                {
                  return RedirectToAction("Login","Home", new { area = "" });
                }


                var user = db.UserMasters.Where(a => a.id == userSession.userDetails.Id).FirstOrDefault();
                if (user == null)
                {
                    return Json(new { Success = true, Msg = "User does not Exist" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    user.user_type = userType;
                    db.SaveChanges();

                    DoctorMaster doctor = new DoctorMaster();
                    doctor.docId = user.id;
                    doctor.name = user.name;
                    doctor.mci_no = user.id.ToString();
                    doctor.IsActive = true;
                    doctor.IsDeleted = false;
                    db.DoctorMasters.Add(doctor);
                    db.SaveChanges();

                    return Json(new { Success = true, Msg = "Updated" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //viewModel.Status = false;
                //viewModel.Message = "Some error occurred. Please try again later";
                //viewModel.Data = new object();


                tblException exception = new tblException()
                {
                    Error = "UpdateUserProfileType " + ex.InnerException,
                    creationDate = DateTime.Now
                };
                db.tblExceptions.Add(exception);
                db.SaveChanges();
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SignOut()
        {
          Session["Details"] = null;
          return RedirectToAction("Login","Home", new { area = "" });
        }

        public ActionResult Userprofile()
        {
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];
            if (userSession == null)
            {
               return RedirectToAction("Login","Home", new { area = "" });
            }

            return View();
        }

        [HttpPost]
        public ActionResult UserUpdateProfile(UserMaster userMaster)
        {

            var user = db.UserMasters.Where(a => a.id == userMaster.id).FirstOrDefault<UserMaster>();
            if (user != null)
            {

                //string date = DateTime.Now.ToString("dd/MM/yyyy");

                user.id = user.id;
                user.address = (userMaster.address == "" || userMaster.address == null) ? user.address : userMaster.address;
                user.blood_donate = (userMaster.blood_donate == "" || userMaster.blood_donate == null) ? user.blood_donate : userMaster.blood_donate;
                user.blood_group = (userMaster.blood_group == "" || userMaster.blood_group == null) ? user.blood_group : userMaster.blood_group;
                user.date_of_birth = (userMaster.date_of_birth == "" || userMaster.date_of_birth == null) ? user.date_of_birth : userMaster.date_of_birth;
                // user.email = userMaster.email == "" ? user.email : userMaster.email;
                user.gcmkey = (userMaster.gcmkey == "" || userMaster.gcmkey == null) ? user.gcmkey : userMaster.gcmkey;
                user.gender = (userMaster.gender == "" || userMaster.gender == null) ? user.gender : userMaster.gender;
                user.latlong = (userMaster.latlong == "" || userMaster.latlong == null) ? user.latlong : userMaster.latlong;
                //  user.mobile = userMaster.mobile == "" ? user.mobile : userMaster.mobile;
                user.name = (userMaster.name == "" || userMaster.name == null) ? user.name : userMaster.name;
                user.OTP = (userMaster.OTP == "" || userMaster.OTP == null) ? user.OTP ?? "" : userMaster.OTP ?? "";
                user.password = (userMaster.password == "" || userMaster.password == null) ? user.password : userMaster.password;
                user.photo = (userMaster.photo == "" || userMaster.photo == null) ? user.photo : userMaster.photo;
                user.ref_code = (userMaster.ref_code == "" || userMaster.ref_code == null) ? user.ref_code : userMaster.ref_code;
                user.used_ref = (userMaster.used_ref == "" || userMaster.used_ref == null) ? user.used_ref : userMaster.used_ref;
                user.user_type = (userMaster.user_type == "" || userMaster.user_type == null) ? user.user_type : userMaster.user_type;
                user.UpdatedDate = DateTime.Now;
            }

            db.Entry(user).State = EntityState.Modified;
            UserDetails details = new UserDetails();
            try
            {
                details.id = user.id;
                details.address = user.address;
                details.blood_donate = user.blood_donate;
                details.blood_group = user.blood_group;
                details.date_of_birth = user.date_of_birth;
                details.email = user.email;
                details.gcmkey = user.gcmkey;
                details.gender = user.gender;
                details.latlong = user.latlong;
                details.mobile = user.mobile;
                details.name = user.name;
                details.OTP = user.OTP;
                details.password = user.password;
                details.photo = user.photo;
                details.ref_code = user.ref_code;
                details.used_ref = user.used_ref;
                details.user_type = user.user_type;
                details.creation_date = user.creation_date;
                int update = db.SaveChanges();

            }
            catch (Exception ex)
            {

            }

            return Json(new { Success = true, Msg = "Done" , data = details },JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDoctorProfile(DoctorInputUpdateViewModel model)
        {
            try
            {
                UserMaster userr = db.UserMasters.Where(a => a.id == model.docId).FirstOrDefault();

                userr.name = model.name ?? "";
                userr.user_type = "D";

                db.Entry(userr).State = EntityState.Modified;
                db.SaveChanges();

                model.docId = userr.id;

                DoctorMaster doctor = db.DoctorMasters.Where(a => a.docId == userr.id).FirstOrDefault();
                doctor.docId = userr.id;
                doctor.dept_id = model.deptId;
                doctor.AboutUs = model.AboutUs ?? "";
                doctor.experience = model.experience ?? "";
                doctor.Fee = model.Fee ?? "";
                doctor.HospitalAffiliation = model.HospitalAffiliation ?? "";
                doctor.online_appointment = model.OnlineAppointment;
                doctor.mci_no = model.mci_no ?? "";
                doctor.name = model.name ?? "";
                doctor.IsActive = false;
                doctor.creation_date = DateTime.Now;
                doctor.Membership = model.Membership;

                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();

                string[] dept = model.deptId.TrimEnd(',').Split(',');
                for (int i = 0; i < dept.Length; i++)
                {
                    int deptid = Convert.ToInt32(dept[i]);
                    var res = db.DoctorDeptMasters.Where(a => a.doc_id == doctor.docId && a.dept_id == deptid).FirstOrDefault();
                    if (res == null)
                    {
                        DoctorDeptMaster docdep = new DoctorDeptMaster()
                        {
                            doc_id = doctor.docId,
                            dept_id = deptid,
                            IsActive = true,
                            CreationDate = DateTime.Now,
                            IsDeleted = false,
                        };

                        db.DoctorDeptMasters.Add(docdep);
                        db.SaveChanges();
                    }
                }

                return Json(new { Success = true, Msg = "Done" }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Msg = "Done" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult Appointment()
        {

            AfterLoginViewModel user = (AfterLoginViewModel)Session["Details"];
            if (user == null)
            {
               return RedirectToAction("Login","Home", new { area = "" });
            }
            else
            {

                List<AppointmentViewModel> listUserAppointmentViewModel = new List<AppointmentViewModel>();

                try
                {
                    string uid = user.userDetails.Id.ToString();


                    var appointments = db.make_appointment.Where(a => a.user_id == uid)
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

        public ActionResult CancelAppointment(int id)
        {

            var appointment = db.make_appointment.Where(a => a.id == id).FirstOrDefault();
            if (appointment != null)
            {
                appointment.AppointmentStatus = "Request Cancelled";

                DateTime dat3 = Convert.ToDateTime(appointment.date);
                TimeSpan diff = DateTime.Now - dat3;
                double hours = diff.TotalHours;
                if (hours <= 24)
                {
                    #region User wallet Amount Deduction
                    wallet wallet = new wallet()
                    {
                        userid = Convert.ToInt32(appointment.user_id),
                        amount = -50,
                        message = "Your wallet is debited with Rs 50. for cancelling appointment within 24 hours.",
                        transdate = DateTime.Now.ToString(),
                        transtype = 2,
                        STATUS = true
                    };
                    db.wallets.Add(wallet);
                    db.SaveChanges();
                    #endregion

                    #region Fecilitation Fee Revert to wallet
                    int docId = Convert.ToInt32(appointment.doctor_id);
                    var docFee = db.DoctorMasters.Where(a => a.docId == docId).FirstOrDefault().Fee;
                    if (docFee != null)
                    {
                        var fecilitationFee = (Convert.ToInt32(docFee) / 100) * 10;
                        wallet drwallet = new wallet()
                        {
                            userid = docId,
                            amount = fecilitationFee,
                            transtype = 1,
                            message = "Your wallet is credited with Rs. " + (fecilitationFee) + " as fecilitation fee",
                            transdate = DateTime.Now.ToString(),
                            STATUS = true
                        };

                        db.wallets.Add(drwallet);
                        db.SaveChanges();
                    }
                    #endregion
                }

                try
                {
                    db.Entry(appointment).State = EntityState.Modified;
                    db.SaveChanges();

                }
                catch (Exception ex)
                {
                    return Json(new { Success = false, appointment = appointment.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            return RedirectToAction("Appointment");
        }

        public JsonResult TrackAppointment(Int64 id)
        {
            TrackAppointmentViewModel trackAppointmentViewModel = new TrackAppointmentViewModel();
            var appointment = db.make_appointment.Where(a => a.QueueNo == id).FirstOrDefault();
            if (appointment != null)
            {
                int docid = Convert.ToInt32(appointment.doctor_id);
                var docDetails = db.DoctorMasters.Where(a => a.docId == docid).FirstOrDefault();

                int clinicId = Convert.ToInt32(appointment.clinic_id);
                var clinicDetails = db.ClinicDetails.Where(a => a.id == clinicId).FirstOrDefault();

                var clinicschedule = db.clinicSchedules.Where(a => a.id == appointment.scheduleId).FirstOrDefault();


                string datestr = "";
                if (appointment.date != null)
                {
                    DateTime dat3 = Convert.ToDateTime(appointment.date);
                    datestr = dat3.ToString("dd/MM/YYYY");
                }


                trackAppointmentViewModel.AppointmentId = appointment.id;
                trackAppointmentViewModel.AppointmentNo = id.ToString();
                trackAppointmentViewModel.PatientName = appointment.PatientName;
                trackAppointmentViewModel.DoctorName = docDetails.name;
                trackAppointmentViewModel.DoctorId = docDetails.docId;
                trackAppointmentViewModel.Date = datestr;

                trackAppointmentViewModel.ClinicId = clinicDetails != null ? clinicDetails.id : 0;
                trackAppointmentViewModel.Address = clinicDetails != null ? clinicDetails.hospital_address : "";
                trackAppointmentViewModel.ClinicName = clinicDetails != null ? clinicDetails.hospital_name : "";
                trackAppointmentViewModel.Latlong = clinicDetails != null ? clinicDetails.latlog : "";
                trackAppointmentViewModel.Status = appointment.AppointmentStatus;
                trackAppointmentViewModel.scheduleTime = clinicschedule != null ? clinicschedule.StartTime + " - " + clinicschedule.EndTime : "";


                var appList = db.make_appointment.Where(a => a.date == appointment.date && (a.AppointmentStatus == "Request Pending" || a.AppointmentStatus == "Request Ongoing"))
                                                 .Select(a => new AppointmentList
                                                 {
                                                     AppointmentNo = a.QueueNo,
                                                     Status = a.AppointmentStatus
                                                 }).ToList();
                if (appList != null)
                {
                    trackAppointmentViewModel.appointmentList = appList;
                    return Json(new { Success = true, appointment = trackAppointmentViewModel.appointmentList }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { Success = false, appointment = "There is no appointment with this queue No" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = true, appointment = trackAppointmentViewModel.appointmentList }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MyWallet()
        {
            AfterLoginViewModel user = (AfterLoginViewModel)Session["Details"];

            if (user == null)
            {
               return RedirectToAction("Login","Home", new { area = "" });
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
                // viewModel.Status = false;
                // viewModel.Message = "Some error occurred. PLease try again later.";
                // viewModel.Data = new object();
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
               return RedirectToAction("Login","Home", new { area = "" });
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


                string outHTML = "<form action = 'https://medzoid.com/User/User/GatewayResponse?uid=" + user.userDetails.Id + "' method = 'post'>";
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

        public ActionResult MyFavDoctor()
        {
            AfterLoginViewModel user = (AfterLoginViewModel)Session["Details"];
            if (user == null)
            {
               return RedirectToAction("Login","Home", new { area = "" });
            }
            else
            {

                List<ViewModel.Doctor> listDoctor = new List<ViewModel.Doctor>();
                var doctors = db.DoctorMasters.Join(db.FavouriteDoctors, dm => dm.docId, ddm => ddm.DoctorId, (dm, ddm) => new { dm, ddm })
                                              .Join(db.UserMasters, um => um.dm.docId, d => d.id, (um, d) => new { um, d })
                                              .Where(a => a.um.dm.IsDeleted == false && a.um.dm.IsActive == true && a.um.ddm.UserID == user.userDetails.Id)
                                              .Select(c => new
                                              {
                                                  id = c.um.dm.id,
                                                  docId = c.um.dm.docId,
                                                  name = c.um.dm.name,
                                                  mci_no = c.um.dm.mci_no,
                                                  AboutUs = c.um.dm.AboutUs,
                                                  online_appointment = c.um.dm.online_appointment,
                                                  dept_id = c.um.dm.dept_id,
                                                  experience = c.um.dm.experience,
                                                  Fee = c.um.dm.Fee,
                                                  Photo = c.d.photo,
                                                  Membership = c.um.dm.Membership,
                                                  HospitalAffiliation = c.um.dm.HospitalAffiliation,
                                                  clinicId = c.um.ddm.ClinicId
                                              }).ToList();

                if (doctors.Count > 0)
                {
                    List<ClinicDet> ListclinicDet = new List<ClinicDet>();
                    foreach (var doctor in doctors)
                    {
                        string depName = "";
                        if (doctor.dept_id != null)
                        {
                            string[] depId = doctor.dept_id.TrimEnd(',').Split(',');
                            for (int i = 0; i < depId.Length; i++)
                            {
                                #region Department Name
                                int dpid = Convert.ToInt32(depId[i]);
                                var name = db.dept_master.Where(a => a.id == dpid).FirstOrDefault().name;
                                depName += name + ",";
                                #endregion
                            }

                            var qual = "";
                            var qualification = db.DoctorQualifications.Where(a => a.docid == doctor.docId).FirstOrDefault();
                            if (qualification != null)
                            {
                                qual = qualification.name ?? "";
                            }

                            double rate = 0.00;
                            int ratingByCount = 0;
                            var rating = db.ReviewBanks.Where(a => a.reviewfor == doctor.docId).GroupBy(a => a.reviewfor)
                                                       .Select(a =>
                                                       new
                                                       {
                                                           Avg = a.Average(b => b.reviewdata),
                                                           Count = a.Count()
                                                       }).FirstOrDefault();
                            if (rating != null)
                            {
                                rate = Convert.ToDouble(rating.Avg);
                                ratingByCount = rating.Count;
                            }

                            bool suffBal = false;
                            var walletAmount = db.wallets.Where(a => a.userid == doctor.docId).Sum(a => a.amount);

                            if (walletAmount != null)
                            {
                                if (walletAmount >= 1000)
                                {
                                    suffBal = true;
                                }
                            }

                            var clinicDetails = db.ClinicDetails.Where(a => a.doc_id == doctor.docId && a.IsDeleted == false && a.id == doctor.clinicId).ToList();
                            if (clinicDetails.Count > 0)
                            {


                                foreach (var clinic in clinicDetails)
                                {
                                    string[] latlong = clinic.latlog.Split(',');

                                    ViewModel.Doctor doctorobj = new ViewModel.Doctor();
                                    doctorobj.AboutUs = doctor.AboutUs ?? "";
                                    doctorobj.dept_id = doctor.dept_id.TrimEnd(',');
                                    doctorobj.deptName = (depName == null || depName == "") ? "" : depName.TrimEnd(',');
                                    doctorobj.DocName = doctor.name ?? "";
                                    doctorobj.doc_id = doctor.docId;
                                    doctorobj.experience = doctor.experience ?? "";
                                    doctorobj.mciNo = doctor.mci_no ?? "";
                                    doctorobj.Membership = doctor.Membership ?? "";
                                    doctorobj.OnlineAppointment = doctor.online_appointment ?? "";
                                    doctorobj.HospitalAffiliation = doctor.HospitalAffiliation ?? "";
                                    doctorobj.Fee = doctor.Fee ?? "";
                                    doctorobj.Photo = doctor.Photo ?? "";
                                    doctorobj.Qualification = qual ?? "";
                                    doctorobj.Rating = rate;
                                    doctorobj.RatingByCount = ratingByCount;
                                    doctorobj.clinicId = clinic.id;
                                    doctorobj.ContactNumber = clinic.contact_details ?? "";
                                    doctorobj.HospitalAddress = clinic.hospital_address ?? "";
                                    doctorobj.HospitalName = clinic.hospital_name ?? "";
                                    doctorobj.LatLong = clinic.latlog ?? "";
                                    doctorobj.IsSufficientWalletBalance = suffBal;
                                    listDoctor.Add(doctorobj);
                                }
                            }
                            else
                            {
                                // viewModel.Status = false;
                                // viewModel.Message = "No Favourite Doctor Found.";
                                // viewModel.Data = listDoctor;
                            }
                        }
                    }
                    if (listDoctor.Count == 0)
                    {
                        //  viewModel.Status = false;
                        //  viewModel.Message = "No Favourite Doctor Found.";
                        //  viewModel.Data = new object();
                    }
                }

                return View(listDoctor);
            }
        }

        public ActionResult CreatePayment()
        {
            return View();
        }

        public ActionResult MyOrders()
        {
            AfterLoginViewModel user = (AfterLoginViewModel)Session["Details"];
            if (user == null)
            {
               return RedirectToAction("Login","Home", new { area = "" });
            }
            else
            {
                List<MyOrderViewModel> myOrderList = new List<MyOrderViewModel>();
                var data = db.PharmacyPrescriptions.Join(db.PharmacyOrders, a => a.id, b => b.pharmacyPrescId, (a, b) => new { a, b })
                                                   .Join(db.Prescriptions, c => c.a.prescriptionId, b => b.id, (c, b) => new { c, b })
                                                   .Join(db.OrderPayments, d => d.c.b.id, c => c.orderId, (d, c) => new { d, c })
                                                   .Join(db.PharmacyMasters, e => e.d.c.a.PharmacyId, d => d.id, (e, d) => new { e, d })
                                                   .ToList().Where(a => a.e.d.b.userId == user.userDetails.Id)
                                                   .Select(a => new
                                                   {
                                                       PharmacyAddress = a.d.PharmacyAddress ?? "",
                                                       DeliveryType = a.e.d.b.DeliveryType,
                                                       OrderNo = a.e.d.c.b.OrderNo,
                                                       OrderAmount = a.e.d.c.b.OrderAmount,
                                                       OrderDate = (Convert.ToDateTime(a.e.d.c.b.CreationDate)).ToString("dd/MM/yyyy"),
                                                       OrderStatus = a.e.d.c.b.OrderStatus ?? "",
                                                       PaymentType = a.e.c.PaymentType,
                                                       PaymentStatus = a.e.c.status,
                                                       OrderDescription = string.IsNullOrEmpty(a.e.d.c.a.PrescriptionEditDescription) ? a.e.d.b.Description : a.e.d.c.a.PrescriptionEditDescription
                                                   }).ToList();

                if (data.Count > 0)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        MyOrderViewModel myOrder = new MyOrderViewModel()
                        {

                            OrderNo = data[i].OrderNo,
                            OrderAmount = data[i].OrderAmount,
                            OrderDate = data[i].OrderDate,
                            OrderDescription = data[i].OrderDescription,
                            OrderStatus = data[i].OrderStatus,
                            PaymentMode = data[i].PaymentType,
                            DeliveryType = data[i].DeliveryType
                        };
                        myOrderList.Add(myOrder);
                    }
                }
                else
                {
                    myOrderList = new List<MyOrderViewModel>();
                }

                return View(myOrderList);
            }
        }

        public ActionResult MyQuotation()
        {
            List<MyQuotationViewModel> myQuotationList = new List<MyQuotationViewModel>();
            AfterLoginViewModel user = (AfterLoginViewModel)Session["Details"];
            if (user == null)
            {
               return RedirectToAction("Login","Home", new { area = "" });
            }
            else
            {
                var data = db.Prescriptions.Join(db.PharmacyPrescriptions, a => a.id, b => b.prescriptionId, (a, b) => new { a, b }).Where(a => a.a.userId == user.userDetails.Id).ToList();
                if (data.Count > 0)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        MyQuotationViewModel myQuotation = new MyQuotationViewModel()
                        {
                            Amount = data[i].b.NewRate == null ? data[i].b.Rate : data[i].b.NewRate,
                            Prescription = data[i].a.Description
                        };
                        myQuotationList.Add(myQuotation);
                    }
                }
            }


            return View(myQuotationList);
        }

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
               return RedirectToAction("Login","Home", new { area = "" });
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

        [HttpPost]
        public ActionResult GetAddress(string search)
        {
            CommonFunctions cf = new CommonFunctions();
            var suggestions = cf.Data(search);
            return Json(new { Success = true, suggestions = suggestions }, JsonRequestBehavior.AllowGet);
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

        public ActionResult MyBloodReq()
        {
            List<BloodDonorDetailResponse> ListbloodDonorDetail = new List<BloodDonorDetailResponse>();
            AfterLoginViewModel usersession = (AfterLoginViewModel)Session["Details"];
            if (usersession == null)
            {
               return RedirectToAction("Login","Home", new { area = "" });
            }
            try
            {
                var res = db.BloodDonorDetails.Where(a => a.user_id == usersession.userDetails.Id).ToList();
                if (res.Count == 0)
                {
                    return View(ListbloodDonorDetail);
                }
                else
                {


                    foreach (var item in res)
                    {

                        if (!string.IsNullOrEmpty(item.ReqTo))
                        {


                            int reqPersonId = Convert.ToInt32(item.ReqTo);

                            var user = db.UserMasters.Where(a => a.id == reqPersonId).FirstOrDefault();

                            DateTime date = Convert.ToDateTime(item.CreationDate);
                            BloodDonorDetailResponse bloodDonorDetail = new BloodDonorDetailResponse();
                            bloodDonorDetail.age = item.age ?? 0;
                            bloodDonorDetail.doctor_name = item.doctor_name ?? "";
                            bloodDonorDetail.doctor_id = item.doctor_id ?? 0;
                            bloodDonorDetail.gender = item.gender ?? "";
                            bloodDonorDetail.hospital_name = item.hospital_name ?? "";
                            bloodDonorDetail.id = item.id;
                            bloodDonorDetail.patient_name = item.patient_name ?? "";
                            bloodDonorDetail.ReqStatus = item.ReqStatus ?? "";
                            bloodDonorDetail.when_required = item.when_required ?? "";
                            bloodDonorDetail.RequestedPersonName = user.name;
                            bloodDonorDetail.bloodGroup = user.blood_group;
                            bloodDonorDetail.ReqTo = item.ReqTo ?? "";
                            bloodDonorDetail.user_id = item.user_id;
                            bloodDonorDetail.CreationDate = date.ToString("dd/MM/yyyy");
                            bloodDonorDetail.IsActive = item.IsActive ?? true;

                            ListbloodDonorDetail.Add(bloodDonorDetail);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                tblException tblException = new tblException()
                {
                    Error = ex.Message,
                    creationDate = DateTime.Now
                };
                db.tblExceptions.Add(tblException);
                db.SaveChanges();
            }
            return View(ListbloodDonorDetail);
        }

        public ActionResult CancelBloodReq(int id)
        {
            try
            {
                var bloodreq = db.BloodDonorDetails.Where(a => a.id == id).FirstOrDefault();
                if (bloodreq != null)
                {

                    bloodreq.ReqStatus = "Request Cancelled";
                    db.Entry(bloodreq).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                tblException Exception = new tblException()
                {
                    Error = ex.Message,
                    creationDate = DateTime.Now
                };
                db.tblExceptions.Add(Exception);
                db.SaveChanges();
            }

            return RedirectToAction("MyBloodReq");
        }

        public ActionResult DonorRequest()
        {
            List<BloodDonorDetailResponse> ListbloodDonorDetail = new List<BloodDonorDetailResponse>();
            AfterLoginViewModel user = (AfterLoginViewModel)Session["Details"];
            if (user == null)
            {
               return RedirectToAction("Login","Home", new { area = "" });
            }

            try
            {

                string userid = user.userDetails.Id.ToString();
                var res = db.BloodDonorDetails.Where(a => a.ReqTo == userid).ToList();
                if (res.Count == 0)
                {

                    return View(ListbloodDonorDetail);
                }
                else
                {


                    foreach (var item in res)
                    {
                        if (!string.IsNullOrEmpty(item.ReqTo))
                        {
                            int reqPersonId = Convert.ToInt32(item.user_id);

                            var user1 = db.UserMasters.Where(a => a.id == reqPersonId).FirstOrDefault();

                            DateTime date = Convert.ToDateTime(item.CreationDate);
                            BloodDonorDetailResponse bloodDonorDetail = new BloodDonorDetailResponse();
                            bloodDonorDetail.age = item.age ?? 0;
                            bloodDonorDetail.doctor_name = item.doctor_name ?? "";
                            bloodDonorDetail.doctor_id = item.doctor_id ?? 0;
                            bloodDonorDetail.gender = item.gender ?? "";
                            bloodDonorDetail.hospital_name = item.hospital_name ?? "";
                            bloodDonorDetail.id = item.id;
                            bloodDonorDetail.patient_name = item.patient_name ?? "";
                            bloodDonorDetail.ReqStatus = item.ReqStatus ?? "";
                            bloodDonorDetail.when_required = item.when_required ?? "";
                            bloodDonorDetail.RequestedPersonName = user1.name;
                            bloodDonorDetail.bloodGroup = user1.blood_group;
                            bloodDonorDetail.ReqTo = item.ReqTo ?? "";
                            bloodDonorDetail.user_id = item.user_id;
                            bloodDonorDetail.CreationDate = date.ToString("dd/MM/yyyy");
                            bloodDonorDetail.IsActive = item.IsActive ?? true;

                            ListbloodDonorDetail.Add(bloodDonorDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(ListbloodDonorDetail);
        }
    }


    public class ClinicDet
    {
        public string HospitalName { get; set; }
        public string ContactNumber { get; set; }
        public string HospitalAddress { get; set; }
    }

    public class UserProfileType
    {
        public int Id { get; set; }
        public string UserType { get; set; }
    }

    public class DoctorInputUpdateViewModel
    {
        public string name { get; set; }
        public string deptId { get; set; }
        public int docId { get; set; }
        public string Fee { get; set; }
        public string experience { get; set; }
        public string AboutUs { get; set; }
        public string HospitalAffiliation { get; set; }
        public string mci_no { get; set; }
        public string Membership { get; set; }
        public string OnlineAppointment { get; set; }
    }

}