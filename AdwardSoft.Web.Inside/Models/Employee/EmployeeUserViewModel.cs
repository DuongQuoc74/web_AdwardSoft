using Microsoft.AspNetCore.Mvc;
using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class EmployeeUserViewModel
    {
        [ProtoMember(1)]
        public int EmployeeId { get; set; }
        [ProtoMember(2)]
        [Remote(action: "CheckExist", controller: "EmployeeUser"
            , ErrorMessage = "Already exist", AdditionalFields = "EmployeeId")]
        [Display(Name = "Account")]
        [Required]
        public long UserId { get; set; }
    }
}
