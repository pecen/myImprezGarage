//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ImprezGarage.Infrastructure.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class PartsReplacementRecord
    {
        public int Id { get; set; }
        public Nullable<bool> IsWiring { get; set; }
        public Nullable<System.DateTime> WiringDate { get; set; }
        public string WiringDetails { get; set; }
        public Nullable<bool> IsHoses { get; set; }
        public Nullable<System.DateTime> HosesDate { get; set; }
        public string HosesDetails { get; set; }
        public Nullable<bool> IsTires { get; set; }
        public Nullable<System.DateTime> TiresDate { get; set; }
        public string TiresDetails { get; set; }
        public Nullable<bool> Other { get; set; }
        public Nullable<System.DateTime> OtherDate { get; set; }
        public string OtherDetails { get; set; }
    }
}
