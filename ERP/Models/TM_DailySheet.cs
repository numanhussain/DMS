//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TM_DailySheet
    {
        public int TDID { get; set; }
        public Nullable<int> ProjectID { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string TaskName { get; set; }
        public Nullable<System.TimeSpan> AssignTime { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> ActualTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public Nullable<bool> Status { get; set; }
        public string Remarks { get; set; }
        public string TaskDescription { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<bool> Repeat { get; set; }
        public string Proirity { get; set; }
        public Nullable<int> NumOfDays { get; set; }
    }
}
