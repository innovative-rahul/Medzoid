using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedzoidSite.Models
{
    public class DepartmentDetails
    {
        public object GetDepartments()
        {
            using (var db = new MedzoidEntities())
            {
                var dept = db.dept_master.ToList().OrderBy(a => a.name)
                                     .Select(a => new
                                     {
                                         id = a.id,
                                         name = a.name ?? "",
                                         deptType = a.deptType ?? "1"
                                     });

                return dept;
            }
        }
    }
}