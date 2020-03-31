using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace MedzoidSite.Models
{
    public class DoctorDetails
    {
        public ViewModel GetListofDoctors(InputDoctorListing model)
        {
            ViewModel viewModel = new ViewModel();
            viewModel.Status = true;
            viewModel.Message = "Done";

            using (var db = new MedzoidEntities())
            {
                #region Old Query
                var doctors = db.DoctorMasters.Join(db.DoctorDeptMasters, dm => dm.docId, ddm => ddm.doc_id, (dm, ddm) => new { dm, ddm })
                                              .Join(db.UserMasters, um => um.dm.docId, d => d.id, (um, d) => new { um, d })
                                              .Where(a => a.um.dm.IsDeleted == false && a.um.dm.IsActive == true && a.um.ddm.dept_id == model.deptId)
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
                                                  Photo = c.d.id + ".jpg",
                                                  Membership = c.um.dm.Membership,
                                                  HospitalAffiliation = c.um.dm.HospitalAffiliation,
                                                  IsClaimed = c.um.dm.Isclaimed,
                                                  IsApproved = c.um.dm.IsApproved

                                              }).ToList();
                #endregion




                //First Filter by deptId 
                List<Doctor> listDoctor = new List<Doctor>();
                List<ClinicDet> ListclinicDet = new List<ClinicDet>();
                if (model.Mode == "1")
                {
                    foreach (var doctor in doctors)
                    {
                        string depName = "";
                        if (doctor.dept_id != null)
                        {
                            string[] depId = doctor.dept_id.TrimEnd(',').Split(',');
                            for (int i = 0; i < depId.Length; i++)
                            {
                                #region Department Name
                                int id = Convert.ToInt32(depId[i]);
                                var name = db.dept_master.Where(a => a.id == id).FirstOrDefault().name;
                                depName += name + ",";
                                #endregion


                                if (depId[i] == model.deptId.ToString())
                                {
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

                                    var clinicDetails = db.ClinicDetails.Where(a => a.doc_id == doctor.docId && a.IsDeleted == false).ToList();
                                    if (clinicDetails.Count > 0)
                                    {
                                        foreach (var clinic in clinicDetails)
                                        {
                                            string[] latlong = clinic.latlog.Split(',');

                                            //double distanceInMetres = GetDistanceBetweenPoints(Convert.ToDouble(latlong[0]), Convert.ToDouble(latlong[1]), Convert.ToDouble(model.LatLong.Split(',')[0]), Convert.ToDouble(model.LatLong.Split(',')[1]));

                                            var sCoord = new GeoCoordinate(Convert.ToDouble(latlong[0]), Convert.ToDouble(latlong[1]));
                                            var eCoord = new GeoCoordinate(Convert.ToDouble(model.LatLong.Split(',')[0]), Convert.ToDouble(model.LatLong.Split(',')[1]));

                                            var distanceInMetres = sCoord.GetDistanceTo(eCoord);

                                            double distanceInKm = (distanceInMetres / 1000);
                                            if (distanceInKm <= 20)
                                            {
                                                Doctor doctorobj = new Doctor();
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
                                                doctorobj.IsClaimed = doctor.IsClaimed;
                                                doctorobj.IsApproved = doctor.IsApproved == null ? false : doctor.IsApproved;
                                                listDoctor.Add(doctorobj);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else if (model.Mode == "2")
                {
                    foreach (var doctor in doctors)
                    {

                        if (doctor.name.Contains(model.DoctorName))
                        {
                            string depName = "";
                            if (doctor.dept_id != null)
                            {
                                string[] depId = doctor.dept_id.TrimEnd(',').Split(',');
                                for (int i = 0; i < depId.Length; i++)
                                {

                                    #region Department Name
                                    int id = Convert.ToInt32(depId[i]);
                                    var name = db.dept_master.Where(a => a.id == id).FirstOrDefault().name;
                                    depName += name + ",";
                                    #endregion

                                    var qual = "";
                                    var qualification = db.DoctorQualifications.Where(a => a.docid == doctor.docId).FirstOrDefault();
                                    if (qualification != null)
                                    {
                                        qual = qualification.name;
                                    }

                                    double? rate = 0.00;
                                    int ratingByCount = 0;
                                    var rating = db.ReviewBanks.Where(a => a.reviewfor == doctor.docId).GroupBy(a => 1)
                                                               .Select(a =>
                                                               new
                                                               {
                                                                   Avg = a.Average(b => b.reviewdata),
                                                                   Count = a.Count()
                                                               }).FirstOrDefault();
                                    if (rating != null)
                                    {
                                        rate = rating.Avg;
                                        ratingByCount = rating.Count;
                                    }


                                    bool suffBal = false;
                                    var walletAmount = db.wallets.Where(a => a.userid == id).Sum(a => a.amount);

                                    if (walletAmount != null)
                                    {

                                        if (walletAmount >= 1000)
                                        {
                                            suffBal = true;
                                        }
                                    }

                                    Doctor doctorobj = new Doctor();
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
                                    doctorobj.Qualification = qual ?? "";
                                    doctorobj.Photo = doctor.Photo ?? "";
                                    doctorobj.Rating = rate;
                                    doctorobj.RatingByCount = ratingByCount;
                                    doctorobj.IsSufficientWalletBalance = suffBal;
                                    doctorobj.IsClaimed = doctor.IsClaimed;
                                    doctorobj.IsApproved = doctor.IsApproved == null ? false : doctor.IsApproved;
                                    if (depId[i] == model.deptId.ToString())
                                    {
                                        var clinicDetails = db.ClinicDetails.Where(a => a.doc_id == doctor.docId && a.IsDeleted == false).ToList();
                                        if (clinicDetails.Count > 0)
                                        {
                                            foreach (var clinic in clinicDetails)
                                            {

                                                doctorobj.clinicId = clinic.id;
                                                doctorobj.LatLong = clinic.latlog ?? "";
                                                doctorobj.ContactNumber = clinic.contact_details ?? "";
                                                doctorobj.HospitalAddress = clinic.hospital_address ?? "";
                                                doctorobj.HospitalName = clinic.hospital_name ?? "";
                                            }
                                        }
                                        listDoctor.Add(doctorobj);
                                    }
                                }
                            }
                        }
                    }
                }
                viewModel.Data = listDoctor;
                return viewModel;
            }


        }
    }


    public class ViewModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }


    public class InputDoctorListing
    {
        public int? deptId { get; set; }
        public string Mode { get; set; }
        public string DoctorName { get; set; }
        public string LatLong { get; set; }
    }


    public class Doctor
    {
        public int doc_id { get; set; }
        public string DocName { get; set; }
        public string mciNo { get; set; }
        public string AboutUs { get; set; }
        public string OnlineAppointment { get; set; }
        public string dept_id { get; set; }
        public string deptName { get; set; }
        public string experience { get; set; }
        public string Fee { get; set; }
        public string LatLong { get; set; }
        public string Photo { get; set; }
        public string HospitalAffiliation { get; set; }
        public string Membership { get; set; }
        public string Qualification { get; set; }
        public int clinicId { get; set; }
        public string HospitalName { get; set; }
        public string ContactNumber { get; set; }
        public string HospitalAddress { get; set; }
        public double? Rating { get; set; }
        public int RatingByCount { get; set; }
        public bool IsSufficientWalletBalance { get; set; }
        public bool? IsClaimed { get; set; }
        public bool? IsApproved { get; set; }

    }

    public class ClinicDet
    {
        public string HospitalName { get; set; }
        public string ContactNumber { get; set; }
        public string HospitalAddress { get; set; }
    }
}