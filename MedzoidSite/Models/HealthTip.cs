//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MedzoidSite.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HealthTip
    {
        public int id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
        public Nullable<bool> IsUpdated { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string MainImage { get; set; }
        public string SeondaryImage { get; set; }
    }
}
