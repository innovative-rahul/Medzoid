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
    
    public partial class manage_body_symptom
    {
        public int id { get; set; }
        public Nullable<int> body_part_id { get; set; }
        public Nullable<int> symptoms_id { get; set; }
        public string x1 { get; set; }
        public string x2 { get; set; }
        public string x3 { get; set; }
        public Nullable<int> POS { get; set; }
        public Nullable<bool> STATUS { get; set; }
    }
}
