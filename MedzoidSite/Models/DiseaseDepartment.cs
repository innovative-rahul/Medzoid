using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.Models
{
    public class DiseaseDepartment
    {
       
        public ViewModel PostDiseasebyDepartment(DiseaseDepartmentInput model)
        {
            ViewModel viewModel = new ViewModel();
            viewModel.Status = true;
            viewModel.Message = "Done";

            using (var db = new MedzoidEntities())
            {
                try
                {

                    var disease = db.diseas_dept_list.Join(db.dept_master, a => a.dept_id, b => b.id, (a, b) => new { a, b }).Where(a => a.a.dieases_id == model.id)
                                                     .Select(a => new
                                                     {
                                                         departName = a.b.name,
                                                         deptId = a.b.id,
                                                         id = a.a.dieases_id

                                                     }).ToList();
                    if (disease.Count > 0)
                    {
                        viewModel.Data = disease;
                    }
                    else
                    {
                        viewModel.Status = false;
                        viewModel.Message = "No data Found";
                        viewModel.Data = new object();
                    }
                }
                catch (Exception ex)
                {
                    viewModel.Status = false;
                    viewModel.Message = "Some error occurred please try again later.";
                    viewModel.Data = new object();

                    tblException exception = new tblException()
                    {
                        Error = "DiseaseDepartment " + ex.InnerException,
                        creationDate = DateTime.Now
                    };

                    db.tblExceptions.Add(exception);
                    db.SaveChanges();

                }
            }

                
            return viewModel;
        }


        
        public ViewModel PostDiseasebySearch(Disease model)
        {
            ViewModel viewModel = new ViewModel();
            viewModel.Status = true;
            viewModel.Message = "Done";

            using (var db = new MedzoidEntities())
            {
                try
                {
                    var disease = db.diseas_master.Where(a => a.name.Contains(model.Search)).ToList();
                    if (disease.Count > 0)
                    {
                        viewModel.Data = disease;
                    }
                    else
                    {
                        viewModel.Status = false;
                        viewModel.Message = "No data Found";
                        viewModel.Data = new object();
                    }
                }
                catch (Exception ex)
                {
                    viewModel.Status = false;
                    viewModel.Message = "Some error occurred please try again later.";
                    viewModel.Data = new object();

                    tblException exception = new tblException()
                    {
                        Error = "DiseasebySearch " + ex.InnerException,
                        creationDate = DateTime.Now
                    };

                    db.tblExceptions.Add(exception);
                    db.SaveChanges();

                }
            }

              
            return viewModel;
        }
    }

    public class Disease
    {
        public string Search { get; set; }
    }

    public class DiseaseDepartmentInput
    {
        public int? id { get; set; }

        public string Name { get; set; }
    }
}