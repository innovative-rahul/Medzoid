using MedzoidSite.Models;
using MedzoidSite.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;

namespace MedzoidSite.Controllers
{
    //[RouteArea("Admin")]
    //[RoutePrefix("dashboard")]
    //[Route("{action}")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
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

                        ViewBag.KeyWords = blog.Select(a =>  a.keyword ).ToList();

                        if (TempData["keyWord"] != null)
                        {
                            string keyword = TempData["KeyWord"].ToString();
                            blog = blog.Where(a => a.keyword.Contains(keyword)).ToList();
                        }
                        blogModel.BlogList = blog;


                        #region Recent Blogs
                        var recentBlog = blog.Where(a => a.STATUS == true)
                                                .Select(a => new RecentBlogs
                                                {
                                                    id = a.id, author = a.author, auth_image = a.auth_image, date = a.date, descr = a.descr, image = a.image, keyword = a.keyword, likes = a.likes, POS = a.POS, STATUS = a.STATUS,
                                                    sub_title = a.sub_title, text = a.text,title = a.title,views = a.views,url = a.url
                                                }).OrderByDescending(a => a.id).Take(5).ToList();
                        if (recentBlog.Count > 0)
                        {
                            blogModel.RecentBloglist = recentBlog;
                        }
                        #endregion

                        #region Popular Blogs
                        var popularBlogs = blog.Where(a => a.likes > 0 && a.STATUS == true)
                                .Select(a => new PopularBlogs
                                {
                                    id = a.id,author = a.author,auth_image = a.auth_image,date = a.date, descr = a.descr, image = a.image,keyword = a.keyword,likes = a.likes, POS = a.POS, STATUS = a.STATUS, sub_title = a.sub_title,
                                    text = a.text,title = a.title,views = a.views,url = a.url
                                }).OrderByDescending(a => a.id).Take(7).ToList();

                        if (popularBlogs.Count > 0)
                        {
                            blogModel.PopularBlogsList = popularBlogs;
                        }
                        #endregion

                    }

                   
                    

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

        public ActionResult BlogPost(string id)
        {
            if (id != null)
            {
                BlogPostViewModel model = new BlogPostViewModel();
                using (var db = new MedzoidEntities())
                {
                    var blogpost = db.BlogNews.Where(a => a.url == id).FirstOrDefault();
                    if (blogpost != null)
                    {
                        model.blog = blogpost;
                        var comments = db.BlogComments.Where(a => a.postId == blogpost.id).OrderByDescending(a => a.Date).Take(5).ToList();
                        if (comments.Count > 0)
                        {
                            model.comments = comments;
                        }
                        return View(model);
                    }
                }
            }
            return RedirectToAction("Blog");
        }

        public ActionResult BlogbyKeyword(string keyword)
        {
            TempData["keyWord"] = keyword;
            return RedirectToAction("Blog");
        }

        public ActionResult SubmitBlogComment(BlogComment model)
        {
            using (var db = new MedzoidEntities())
            {
                model.Date = DateTime.Now;
                db.BlogComments.Add(model);
                db.SaveChanges();
            }
            return RedirectToAction("BlogPost", new { id = model.postId });
        }

        public ActionResult Career()
        {
            using (var db = new MedzoidEntities())
            {
                var carrier = db.carrers.Where(a => a.STATUS == true).ToList();
                return View(carrier);
            }
                
        }

        public ActionResult Apply()
        {

            try
            {
                string Message = HttpContext.Request.Params.Get("message");
                string Email = HttpContext.Request.Params.Get("email");
                string name = HttpContext.Request.Params.Get("name");
                HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

                HttpPostedFile hpf = null;
                // CHECK THE FILE COUNT.
                for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
                {
                    hpf = hfc[iCnt];
                }

                MailMessage message = new MailMessage();
                message.To.Add(new MailAddress("info@medzoid.com"));
                message.From = new MailAddress(Email);
                Attachment attachment;
                attachment = new Attachment(hpf.InputStream, hpf.FileName);
                message.Attachments.Add(attachment);
                message.Subject = "New Application";

                string body = "New Applicant : <br/>" + name + "<br/>" + Email + "<br/>" + Message;
                message.Body = body;
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "info@medzoid.com",  // replace with valid value
                        Password = "F408vc^v"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "medzoid.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
           

            return Json(new { Status = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult SubmitContact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                var message = new MailMessage();
                message.To.Add(new MailAddress("info@medzoid.com"));  // replace with valid value 
                message.From = new MailAddress(model.Email);  // replace with valid value
                message.Subject = "New Query";
                message.Body = string.Format(body, model.Name, model.Email, model.Message);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "info@medzoid.com",  // replace with valid value
                        Password = "F408vc^v"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "medzoid.com";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;
                    smtp.Send(message);
                    ViewBag.Msg = "<script>alert('Submit Successfully');</script>";
                    return RedirectToAction("Contact");
                }
            }

                return View("Contact");
        }

        [HttpPost]
        public ActionResult Subscribe(string Email)
        {
            if (ModelState.IsValid)
            {
                using (var db = new MedzoidEntities())
                {
                    var sub = db.Subscriptions.Where(a => a.email == Email).FirstOrDefault();
                    if (sub != null)
                    {
                        TempData["msg"] = "<script>alert('Already Subscribed');</script>";
                    }
                    else
                    {
                        var message = new MailMessage();
                        message.To.Add(new MailAddress(Email));  // replace with valid value 
                        message.From = new MailAddress("info@medzoid.com");  // replace with valid value
                        message.Subject = "Medzoid Subsciption";
                        message.Body = string.Format("You have subsscribed for the Medazoid Updates.");
                        message.IsBodyHtml = true;

                        using (var smtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = "info@medzoid.com",  // replace with valid value
                                Password = "F408vc^v"  // replace with valid value
                            };
                            smtp.Credentials = credential;
                            smtp.Host = "medzoid.com";
                            smtp.Port = 25;
                            smtp.EnableSsl = false;
                            smtp.Send(message);
                            TempData["msg"] = "<script>alert('Submit Successfully');</script>";



                            Subscription subscription = new Subscription()
                            {
                                email = Email
                            };
                            db.Subscriptions.Add(subscription);
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Blog");
                }
            }

            return View("Blog");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserMaster userMaster)
        {
            try
            {
                using (var db = new MedzoidEntities())
                {
                    AfterLoginViewModel model = new AfterLoginViewModel();
                    var user = db.UserMasters.Where(a => a.mobile == userMaster.mobile && a.password == userMaster.password).FirstOrDefault();
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
                            Address = user.address,
                            LatLong = user.latlong
                        };

                        //Session["user"] = userVM;
                        // var list = JsonConvert.SerializeObject(userVM , Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
                        model.userDetails = userVM;

                        var depts = db.dept_master.Select(a => new Department { Id = a.id, DepartmentName = a.name, deptType = a.deptType }).ToList();

                        model.Departments = depts;

                        if (user.user_type == "D")
                        {
                            var doc = db.DoctorMasters.Where(a => a.docId == user.id).FirstOrDefault();
                            if (doc != null)
                            {
                                DoctorViewModel docVM = new DoctorViewModel()
                                {
                                    AboutUs = doc.AboutUs,
                                    deptId = doc.dept_id,
                                    docId = doc.docId,
                                    experience = doc.experience,
                                    Fee = doc.Fee,
                                    HospitalAffiliation = doc.HospitalAffiliation,
                                    MciNo = doc.mci_no,
                                    Membership = doc.Membership,
                                    Name = doc.name,
                                    OnlineAppointment = doc.online_appointment
                                };
                                model.DoctorDetails = docVM;

                                Session["Details"] = model;

                                return Json(new { Success = true, msg = "User Exists", details = Session["details"] }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        Session["Details"] = model;
                        return Json(new { Success = true, msg = "User Exists", details = Session["details"] }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var employee = db.doc_employee_master.Where(a => a.Mobile == userMaster.mobile && a.Password == userMaster.password && a.IsDeleted == false).FirstOrDefault();
                        if(employee != null)
                        {
                            UserMasterVM userVM = new UserMasterVM()
                            {
                                Id = employee.id,
                                Mobile = employee.Mobile,
                                Name = employee.EmpName,
                                Password = employee.Password,
                                UserType = "E",
                                Email = employee.Mobile,
                                Gender = "",
                                BloodDonor = "",
                                BloodGroup = "",
                                DOB = "",
                                Address = "",
                                LatLong = ""
                            };
                            model.userDetails = userVM;
                            Session["Details"] = model;

                            return Json(new { Success = true, msg = "User Exists", details = Session["details"] }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { Success = false, msg = "Invalid credentials" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (var db = new MedzoidEntities())
                {
                    tblException exception = new tblException()
                    {
                        Error = "Login " + ex.InnerException,
                        creationDate = DateTime.Now
                    };
                    db.tblExceptions.Add(exception);
                    db.SaveChanges();
                }
                return Json(new { Success = false, msg = "Some error occurred. please try again later." }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Department()
        {
            return View();
        }

        public ActionResult DiseaseDepartment()
        {
            return View();
        }

        public ActionResult DoctorList()
        {
            return View();
        }

        public ActionResult DocDetails()
        {
            return View();
        }

        public ActionResult Procedure()
        {
            return View();
        }

        [Route("~/Pharmacy/Allopathic")]
        public ActionResult Allopathic()
        {
            return View();
        }

        public ActionResult Ayurvedic()
        {
            return View();
        }

        public ActionResult Homeopathic()
        {
            return View();
        }

        public ActionResult Unani()
        {
            return View();
        }

        public ActionResult symptoms()
        {
            return View();
        }

        public ActionResult Drugs()
        {
            return View();
        }

        public ActionResult Press()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AreyouQualifiedDoctor()
        {
            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }

        public ActionResult HelpFaq()
        {
            return View();
        }

        [HttpPost]
        public ActionResult HelpFagJson()
        {
            using (var db = new MedzoidEntities())
            {
                var faq = db.FAqs.Where(a => a.Category != "Corona").ToList();
                return Json(new { Status = true, data = faq }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SendDownLoadLink(string mobile)
        {
            Sms sms_obj = new Sms();
            sms_obj.mobileno = mobile;
            sms_obj.txt = "You can download the app from play store from the link http://bit.ly/2STx27S";
            sms_obj.SMS_SEND("Promotional");
            return RedirectToAction("Index");
        }

        public ActionResult RegisterDoctor()
        {
            return View();
        }

        public ActionResult RegisterPharmacy()
        {
            return View();
        }

        public ActionResult UploadPrescription()
        {
            return View();
        }

        public ActionResult SearchBloodDonor()
        {
            return View();
        }

        public JsonResult LoadDepartments()
        {
            DepartmentDetails dpet = new DepartmentDetails();
            var data = dpet.GetDepartments();
            return Json(new { Status = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDoctorList()
        {
            var data = new object();
            return Json(data,JsonRequestBehavior.AllowGet);
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
                using (var db = new MedzoidEntities())
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

                    
                    userMaster.user_type = "B";
                    userMaster.mobile = userMaster.mobile;
                    userMaster.email = userMaster.mobile;
                    userMaster.creation_date = DateTime.Now;
                    userMaster.IsDeleted = false;
                    userMaster.OTP = OTP.ToString();
                    db.UserMasters.Add(userMaster);
                    db.SaveChanges();
                    #endregion

                    #region Send OTP
                    Sms sms_obj = new Sms();
                    sms_obj.mobileno = userMaster.mobile;
                    sms_obj.txt = OTP.ToString();
                    sms_obj.SMS_SEND("Normal");
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

        public JsonResult VerifyOTP(OTPInputModel model)
        {
            OTPViewModel otp = new OTPViewModel();
            try
            {
                using (var db = new MedzoidEntities())
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
                
            }
            catch (Exception ex)
            {
                // viewModel.Status = false;
                // viewModel.Message = ex.Message;
                // viewModel.Data = new object();
            }

            return Json(new { Success = true, msg = "Done", OTP = otp }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string mobile)
        {
            try
            {
                using (var db = new MedzoidEntities())
                {
                    var res = db.UserMasters.Where(a => a.mobile == mobile).FirstOrDefault();
                    if (res == null)
                    {
                        
                        var Message = "User with mobile no. " + mobile + " not found";
                        
                        return Json(new { Success = true, msg = Message }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {

                        Session["UserId"] = res.id;

                        Random gen = new Random();
                        var OTP = gen.Next(1001, 9999);

                        Sms sms_obj = new Sms();
                        sms_obj.mobileno = mobile;
                        sms_obj.txt = OTP.ToString();
                        sms_obj.SMS_SEND("Normal");
                        Session["OTP"] = OTP.ToString();

                        //ForgotPwdViewModel fvm = new ForgotPwdViewModel()
                        //{
                        //    Id = res,
                        //    OTP = OTP.ToString()
                        //};

                       // var message = "Password has been sent your registered number";
                        return Json(new { Success = true}, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, msg = "Some Error Occurred" }, JsonRequestBehavior.AllowGet);
            }
           
        }

        [HttpPost]
        public ActionResult GetDoctorList(InputDoctorListing model)
        {
            DoctorDetails doc = new DoctorDetails();

            var data = doc.GetListofDoctors(model);

            return Json(new { Status = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SymptomsList(symptomsInputModel model)
        {
            Symptoms symptoms = new Symptoms();

            var data = symptoms.PostSymptoms(model);

            return Json(new { Status = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DiseaseDepartmentList(DiseaseDepartmentInput model)
        {
            DiseaseDepartment dept = new DiseaseDepartment();

            var data = dept.PostDiseasebyDepartment(model);

            return Json(new { Status = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DiseasebySearch(Disease model)
        {
            DiseaseDepartment dept = new DiseaseDepartment();

            var data = dept.PostDiseasebySearch(model);

            return Json(new { Status = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetprocedurebySearch(ProcedureDeptDetailModel model)
        {
            Procedures proc = new Procedures();

            var data = proc.PostprocedurebySearch(model);

            return Json(new { Status = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ProcedureDeptDetail(ProcedureDeptDetailModel model)
        {
            Procedures proc = new Procedures();

            var data = proc.PostProcedureDeptDetail(model);

            return Json(new { Status = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SearchPharmacy(PharmacySeachModel model)
        {
            PharmacyDetails proc = new PharmacyDetails();

            var data = proc.SearchPharmacy(model);

            return Json(new { Status = true, Data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Corona()
        {
            return View();
        }

        [HttpGet]
        public JsonResult CoronaQuiz()
        {
            using(var db = new MedzoidEntities())
            {
                var quiz = db.CoronaQuizs.ToList();
                return Json(new { Status = true, data = quiz }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SubmitQuiz(QuizModel[] model)
        {
            using (var db = new MedzoidEntities())
            {
                List<CorrectAnswers> ListCorrectAnswers = new List<CorrectAnswers>();
                var faqquiz = db.CoronaQuizs.ToList();
                decimal totalQuestion = faqquiz.Count();
                decimal correctAnswers = 0;
                for (int i = 0; i < model.Length; i++)
                {
                    var answer = faqquiz.Where(a => a.id == model[i].id).FirstOrDefault();
                    CorrectAnswers correctAnswer = new CorrectAnswers();
                    var faq = db.FAqs.Where(a => a.id == answer.FaqNo).FirstOrDefault();
                    if (faq != null)
                    {
                        
                        correctAnswer.Question = answer.Question;
                        correctAnswer.Answer = faq.Answer;
                        correctAnswer.AnswerMarked = model[i].answer;
                        
                        
                    }
                    if (answer.Answer == model[i].answer)
                    {
                        correctAnswers += 1;
                      
                    }
                    correctAnswer.corrAns = answer.Answer;
                    ListCorrectAnswers.Add(correctAnswer);
                }
                decimal totalscore = Math.Round((correctAnswers / totalQuestion) * 100);
                return Json(new { Status = true, data =  totalscore, ans = ListCorrectAnswers }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult HealthEmergency(string id)
        {
            return View();
        }

        public ActionResult HowSearchWorks()
        {
            return View();
        }

        public ActionResult VerifiedReviews()
        {
            return View();
        }


        public ActionResult Resetpassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string password)
        {
            try
            {
                using (var db = new MedzoidEntities())
                {
                    int uid = Convert.ToInt32(Session["UserId"]);
                    var userObj = db.UserMasters.Where(a => a.id == uid).FirstOrDefault();
                    if (userObj != null)
                    {
                        userObj.password = password;
                        db.Entry(userObj).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { Success = false, msg = "Some error Occurred" }, JsonRequestBehavior.AllowGet);
            }

            

            
        }



        public JsonResult VerifyOTPforforgotPassword(string OTP)
        {
            OTPViewModel otp = new OTPViewModel();
            try
            {
                string sessionOTP = Session["OTP"].ToString();
                if(!string.IsNullOrEmpty(sessionOTP))
                {
                    if (OTP == sessionOTP)
                    {
                        return Json(new { Success = true, msg = "OTP Verified"}, JsonRequestBehavior.AllowGet);
                    }
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


    }


    


}