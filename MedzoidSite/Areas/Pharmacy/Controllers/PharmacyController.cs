using MedzoidSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedzoidSite.ViewModel;
using System.Data.Entity;

namespace MedzoidSite.Areas.Pharmacy.Controllers
{
    public class PharmacyController : Controller
    {

     
        // GET: Pharmacy/Pharmacy
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

                PharmacyDashboardViewModel dashboard = new PharmacyDashboardViewModel()
                {
                    DuePayment = 0,
                    DueFecilitationCharges = 0,
                    Orders = db.PharmacyPrescriptions.Where(a => a.PrescriptionStatus == "Accepted").Count(),
                    Prescriptions = db.Prescriptions.Where(a => a.IsApproved == "1").Count()
                };
                return View(dashboard);
            }
        }


        public ActionResult Orders()
        {
            AfterLoginViewModel user = (AfterLoginViewModel)Session["Details"];
            if (user == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            else
            {
                List<OrderViewModel> myOrderList = new List<OrderViewModel>();
                using (var db = new MedzoidEntities())
                {
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
                            OrderViewModel myOrder = new OrderViewModel()
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
                        myOrderList = new List<OrderViewModel>();
                    }
                }
                return View(myOrderList);
            }
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
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            using (var db = new MedzoidEntities())
            {
                var userObj = db.UserMasters.Where(a => a.id == user.userDetails.Id).FirstOrDefault();
                if (userObj != null)
                {
                    userObj.password = userMaster.password;
                    db.Entry(userObj).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
 

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SignOut()
        {
            Session["Details"] = null;
            AfterLoginViewModel userSession = (AfterLoginViewModel)Session["Details"];
            if (userSession == null)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            return RedirectToAction("Login", "Home", new { area = "" });
        }
    }
}