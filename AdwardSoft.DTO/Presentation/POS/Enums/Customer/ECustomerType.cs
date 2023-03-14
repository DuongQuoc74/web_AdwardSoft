using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.DTO.Presentation.POS
{
    public enum ECustomerType
    {
        [Display(Name = "Cá nhân")]
        [Description("Cá nhân")]
        Personal = 0,

        [Display(Name = "Tổ chức")]
        [Description("Tổ chức")]
        Organization = 1
    }
}
