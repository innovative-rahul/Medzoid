using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.Models
{
    public class Procedures
    {

        
        public ViewModel PostprocedurebySearch(ProcedureDeptDetailModel model)
        {
            ViewModel viewModel = new ViewModel();
            viewModel.Status = true;
            viewModel.Message = "Done";

            using (var db = new MedzoidEntities())
            {
                try
                {
                    var data = db.proc_master.Where(a => a.name.Contains(model.Search)).Select(a => new { id = a.id, Name = a.name }).OrderBy(a => a.Name).ToList();
                    if (data.Count > 0)
                    {
                        viewModel.Data = data;
                    }
                    else
                    {
                        viewModel.Status = false;
                        viewModel.Message = "No Data Found";
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
                        Error = "Procedure " + ex.InnerException,
                        creationDate = DateTime.Now
                    };
                    db.tblExceptions.Add(exception);
                    db.SaveChanges();
                }
            }

                


            return viewModel;
        }



        public ViewModel PostProcedureDeptDetail(ProcedureDeptDetailModel model)
        {
            ViewModel viewModel = new ViewModel();
            viewModel.Status = true;
            viewModel.Message = "Done";
            using (var db = new MedzoidEntities())
            {

                try
                {

                    var data = db.proc_master.Join(db.procedure_dept_list, pm => pm.id, pd => pd.proc_id, (pm, pd) => new { pm, pd })
                                         .Join(db.dept_master, pd => pd.pd.dept_id, dm => dm.id, (pd, dm) => new { pd, dm }).Where(pd => pd.pd.pd.proc_id == model.id || pd.pd.pm.name.Contains(model.Search))
                                         .Select(a => new { depid = a.dm.id, departmentName = a.dm.name }).ToList();



                    if (data.Count > 0)
                    {
                        viewModel.Data = data;
                    }
                    else
                    {
                        viewModel.Status = false;
                        viewModel.Message = "No Data Found";
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
                        Error = "ProcedureDeptDetail " + ex.InnerException,
                        creationDate = DateTime.Now
                    };
                    db.tblExceptions.Add(exception);
                    db.SaveChanges();
                }
            }
            return viewModel;
        }

    }

    public class ProcedureDeptDetailModel
    {
        public int id { get; set; }
        public string Search { get; set; }
    }
}