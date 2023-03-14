using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace AdwardSoft.Web.Inside.Models
{
    [ProtoContract]
    public class GenderViewModel
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        [Required]
        public string Name { get; set; }
    }
}
