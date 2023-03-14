using Microsoft.AspNetCore.Http;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(EmployeeDataTableViewModel))]
    public class EmployeeViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Required]
        [Display(Name = "Name")]
        [MaxLength(70)]
        public string Name { get; set; }
        [ProtoMember(3)]
        [Required]
        [Display(Name = "Phone")]
        [MaxLength(20)]
        public string Phone { get; set; }
        [ProtoMember(4)]
        [Required]
        [Display(Name = "Address")]
        [MaxLength(150)]
        public string Address { get; set; }
        [ProtoMember(5)]
        [Required]
        [Display(Name = "Identity Code")]
        [MaxLength(20)]
        public string IdentityCode { get; set; }
        [ProtoMember(6)]
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email not valid")]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }
        [ProtoMember(7)]
        [Required]
        [Display(Name = "Gender")]
        public int GenderId { get; set; }
        [ProtoMember(8)]
        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [ProtoMember(9)]
        [Required]
        [Display(Name = "Position")]
        public int PositionId { get; set; }
        [ProtoMember(10)]
        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime DoB { get; set; } = DateTime.Now;
        [ProtoMember(11)]
        [Display(Name = "Avatar")]
        [MaxLength(2048)]
        public string Avatar { get; set; } = string.Empty;
        [ProtoMember(12)]
        //[Required]
        [Display(Name = "Code")]
        [MaxLength(10)]
        public string Code { get; set; }
        [ProtoMember(13)]
        [Required]
        [Display(Name = "Place Of Birth")]
        [MaxLength(150)]
        public string PlaceOfBirth { get; set; }
        [ProtoMember(14)]
        [Required]
        [Display(Name = "Nationality")]
        [MaxLength(20)]
        public string Nationality { get; set; }
        [ProtoMember(15)]
        [Required]
        [Display(Name = "Religious")]
        public short Religious { get; set; }
        [ProtoMember(16)]
        [Display(Name = "Permanent Address")]
        [MaxLength(150)]
        [Required]
        public string PermanentAddress { get; set; }
        [ProtoMember(17)]
        [Required]
        [Display(Name = "Current Address")]
        [MaxLength(150)]
        public string CurrentAddress { get; set; }
        [ProtoMember(18)]
        [Required]
        [Display(Name = "Leaving Date")]
        public DateTime LeavingDate { get; set; } = DateTime.Now;
        [ProtoMember(19)]
        [Display(Name = "Starting Date")]
        public DateTime StartingDate { get; set; } = DateTime.Now;
        [ProtoMember(20)]
        [Display(Name = "Status")]
        public short Status { get; set; }

        public IFormFile AvatarFile { get; set; }

        public string DoBStr { get; set; }
        public string StartingDateStr { get; set; }
    }

    [ProtoContract]
    public class EmployeeDataTableViewModel : EmployeeViewModel
    {
        [ProtoMember(21)]
        public int Count { get; set; }
        [ProtoMember(22)]
        [Display(Name = "Actual Wage")]
        public decimal ActualWage { get; set; }
        [ProtoMember(23)]
        [Display(Name = "Account link")]
        public string UserName { get; set; } = string.Empty;
    }
    [ProtoContract]
    public class EmployeeElastic
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Code { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string Phone { get; set; }
        [ProtoMember(5)]
        public string IdentityCode { get; set; }
        public string Text
        {
            get
            {
                return Name;
            }
        }
    }

    [ProtoContract]
    public class EmployeeSearch
    {
        [ProtoMember(1)]
        public long Total { get; set; }
        [ProtoMember(2)]
        public List<EmployeeElastic> Items { get; set; }
    }

}
