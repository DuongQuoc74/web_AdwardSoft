using ProtoBuf;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class EmployeeSalaryViewModel
    {
        [ProtoMember(1)]
        public int EmployeeId { get; set; }
        [ProtoMember(2)]
        [Display(Name = "Effective Date")]
        public DateTime EffectiveDate { get; set; } = DateTime.Now;
        [ProtoMember(3)]
        [Display(Name = "Basic Salary")]
        public decimal BasicSalary { get; set; }
        [ProtoMember(4)]
        public short Type { get; set; }
        [ProtoMember(5)]
        public short Wage { get; set; }
        [ProtoMember(6)]
        [Display(Name = "Actual Wage")]
        public decimal ActualWage { get; set; }
        public string EffectiveDateStr { get; set; }
    }
}
