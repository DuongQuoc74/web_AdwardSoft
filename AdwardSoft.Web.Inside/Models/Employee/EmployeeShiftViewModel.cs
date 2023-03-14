using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(EmployeeShiftDataTableViewModel))]
    public class EmployeeShiftViewModel
    {
        [ProtoMember(1)]
        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        [ProtoMember(2)]
        [Required]
        [Remote(action: "CheckExist", controller: "EmployeeShift"
            , ErrorMessage = "Employee in shift is already exist!", AdditionalFields = "EmployeeId,Month,Year")]
        [Display(Name = "Shift")]
        public int ShiftId { get; set; }
        [ProtoMember(3)]
        [Required]
        [Display(Name = "Month")]
        public decimal Month { get; set; }
        [ProtoMember(4)]
        [Required]
        [Display(Name = "Year")]
        public decimal Year { get; set; }
        [ProtoMember(5)]
        public int Type { get; set; }
        [ProtoMember(6)]
        [Display(Name = "CheckoutCounter")]
        public int CheckoutCounterId { get; set; }

        public bool IsNew { get; set; }

        public string Employee { get; set; }
    }

    [ProtoContract]
    public class EmployeeShiftDataTableViewModel : EmployeeShiftViewModel
    {
        [ProtoMember(7)]
        public int Count { get; set; }
        [ProtoMember(8)]
        [Display(Name = "Shift Name")]
        public string ShiftName { get; set; }
        [ProtoMember(9)]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [ProtoMember(10)]
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }
    }
}
