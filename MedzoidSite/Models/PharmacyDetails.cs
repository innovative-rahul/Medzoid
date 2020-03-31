using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Web;

namespace MedzoidSite.Models
{
    public class PharmacyDetails
    {
        
        public ViewModel SearchPharmacy(PharmacySeachModel model)
        {
            ViewModel viewModel = new ViewModel();
            viewModel.Status = true;
            viewModel.Message = "Done";

            using (var db = new MedzoidEntities())
            {

                if (model.Mode == "1")
                {
                    var pharmacy = db.PharmacyMasters.Where(a => a.Isdeleted == false && a.PharmacyType.Contains(model.MedicineName)).Select(a =>
                                                     new
                                                     {
                                                         id = a.id,
                                                         name = a.name ?? "",
                                                         pName = a.pharmacy_name ?? "",
                                                         dl_no = a.dl_no ?? "",
                                                         phImage = a.userId + ".jpg",
                                                         phAddress = a.PharmacyAddress ?? "",
                                                         phphoneNo = a.PharmacyPhnNo ?? "",
                                                         contactPerson = a.contact_person_name ?? "",
                                                         latLong = a.lat_log ?? "",
                                                         Announcement = a.Announcement ?? "",
                                                         pharmacyCategory = a.PharmacyCategory,
                                                         sellOnline = a.SellOnline,
                                                         pharmacyType = a.PharmacyType,
                                                         DeliveryType = a.DeliveryType ?? "",
                                                         DeliveryDistance = a.DeliveryDistance ?? 0,
                                                         Rating = "",
                                                         RateCount = "",
                                                         userId = a.userId
                                                     }).Distinct().ToList();
                    if (pharmacy.Count > 0)
                    {
                        var pharmacyList = pharmacy;
                        for (int i = 0; i < pharmacy.Count; i++)
                        {
                            string[] latlong = pharmacy[i].latLong.Split(',');
                            var sCoord = new GeoCoordinate(Convert.ToDouble(latlong[0]), Convert.ToDouble(latlong[1]));
                            var eCoord = new GeoCoordinate(Convert.ToDouble(model.latLong.Split(',')[0]), Convert.ToDouble(model.latLong.Split(',')[1]));
                            var distanceInMetres = sCoord.GetDistanceTo(eCoord);

                            double distanceInKm = (distanceInMetres / 1000);
                            if (distanceInKm > pharmacy[i].DeliveryDistance)
                            {
                                pharmacyList.Remove(pharmacy[i]);
                            }
                        }
                        viewModel.Data = pharmacyList;
                    }
                    else
                    {
                        viewModel.Status = false;
                        viewModel.Message = "No pharmacy Found";
                        viewModel.Data = new object();
                    }
                }
                else if (model.Mode == "2")
                {

                    var pharmacy = db.PharmacyMasters.Join(db.medical_store_master, a => a.id, b => b.pharmacy_id, (a, b) => new { a, b })
                                                     .Join(db.medicine_master, b => b.b.medicine_id, c => c.id, (a, b) => new { a, b }).Where(c => c.b.medicine_name.Contains(model.MedicineName) && c.a.b.STATUS == true)
                                                     .Select(a =>
                                                              new
                                                              {
                                                                  id = a.a.a.id,
                                                                  name = a.a.a.name ?? "",
                                                                  pName = a.a.a.pharmacy_name ?? "",
                                                                  dl_no = a.a.a.dl_no ?? "",
                                                                  phImage = a.a.a.userId + ".jpg",
                                                                  phAddress = a.a.a.PharmacyAddress ?? "",
                                                                  phphoneNo = a.a.a.PharmacyPhnNo ?? "",
                                                                  contactPerson = a.a.a.contact_person_name ?? "",
                                                                  latLong = a.a.a.lat_log ?? "",
                                                                  Announcement = a.a.a.Announcement ?? "",
                                                                  pharmacyCategory = a.a.a.PharmacyCategory,
                                                                  sellOnline = a.a.a.SellOnline,
                                                                  pharmacyType = a.a.a.PharmacyType,
                                                                  DeliveryType = a.a.a.DeliveryType ?? "",
                                                                  DeliveryDistance = a.a.a.DeliveryDistance ?? 0,
                                                                  Rating = "",
                                                                  RateCount = "",
                                                                  userId = a.a.a.userId
                                                              }).Distinct().ToList();

                    if (pharmacy.Count > 0)
                    {
                        viewModel.Data = pharmacy;
                    }
                    else
                    {
                        viewModel.Status = false;
                        viewModel.Message = "No pharmacy Found";
                        viewModel.Data = new object();
                    }
                }
            }
            return viewModel;
        }

    }

    public class PharmacySeachModel
    {
        public string Mode { get; set; }
        public string MedicineName { get; set; }
        public string latLong { get; set; }
    }
}