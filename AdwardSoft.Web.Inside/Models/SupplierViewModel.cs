using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    [ProtoInclude(100, typeof(SupplierDatatableViewModel))]
    public class SupplierViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Required]
        [MaxLength(120)]
        public string Name { get; set; }
        [ProtoMember(3)]
        [Required]
        [MaxLength(128)]
        public string Address { get; set; }
        [ProtoMember(4)]
        [Required]
        [MaxLength(12)]
        public string Tel { get; set; }
        [ProtoMember(5)]
        [Required]
        [MaxLength(254)]
        [EmailAddress]
        [RegularExpression(@"^([A-Za-z0-9][^'!&\\#*$%^?<>()+=:;`~\[\]{}|/,₹€@ ][a-zA-z0-9-._][^!&\\#*$%^?<>()+=:;`~\[\]{}|/,₹€@ ]*\@[a-zA-Z0-9][^!&@\\#*$%^?<>()+=':;~`.\[\]{}|/,₹€ ]*\.[a-zA-Z]{2,6})$", ErrorMessage = "Please enter a valid Email")]
        public string Email { get; set; }
        [ProtoMember(6)]
        [Required]
        [Display(Name = "Default")]
        public bool IsDefault { get; set; }
    }

    [ProtoContract]
    public class SupplierDatatableViewModel : SupplierViewModel
    {
        [ProtoMember(7)]
        public int Count { get; set; }
    }

    [ProtoContract]
    public class SupplierContactViewModel
    {
        [ProtoMember(1)]
        public int SupplierId { get; set; }
        [ProtoMember(2)]
        public int Idx { get; set; }
        [ProtoMember(3)]
        [Required]
        [Display(Name = "Phone")]
        public string Phone { get; set; }
        [ProtoMember(4)]
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [ProtoMember(5)]
        [Required]
        [Display(Name = "Position")]
        public string Position { get; set; }
        [ProtoMember(6)]
        [Required]
        [Display(Name = "Note")]
        public string Note { get; set; }
        [ProtoMember(7)]
        [Required]
        [Display(Name = "Default")]
        public bool IsDefault { get; set; }
    }
}
