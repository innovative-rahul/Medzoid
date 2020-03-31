using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.Models
{
    public class Symptoms
    {
        
        public ViewModel PostSymptoms(symptomsInputModel model)
        {
            ViewModel viewModel = new ViewModel();
            viewModel.Status = true;
            viewModel.Message = "Done";

            using (var db = new MedzoidEntities())
            {
                try
                {
                    if (model.Search == "$$$$")
                    {
                        model.Search = "";
                    }


                    var res = db.symptoms_to_dept.Join(db.symptoms_master, a => a.symptoms, b => b.id, (a, b) => new { a, b }).Where(a => a.a.body_part == model.BodyPartId && a.b.symptoms_name.Contains(model.Search))
                                             .Select(a => new
                                             {
                                                 symptoms_id = a.b.id,
                                                 symptoms_name = a.b.symptoms_name ?? "",
                                                 medical_warning = a.b.medical_warning ?? "",
                                                 warning_text = a.a.reason
                                             }).ToList();

                    if (res.Count > 0)
                    {
                        viewModel.Data = res;
                    }
                    else
                    {
                        viewModel.Status = false;
                        viewModel.Message = "No Symptom found";
                        viewModel.Data = new object();
                    }
                }
                catch (Exception ex)
                {
                    viewModel.Status = false;
                    viewModel.Message = "Some error occurred. Please try again later.";
                    viewModel.Data = new object();

                    tblException exception = new tblException() { Error = "Symptoms " + ex.InnerException, creationDate = DateTime.Now };

                    db.tblExceptions.Add(exception);
                    db.SaveChanges();
                }
            }

            
            return viewModel;
        }
    }

    public class symptomsInputModel
    {
        public string BodyPartId { get; set; }
        public string Search { get; set; }
    }
}