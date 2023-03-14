using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class EmployeeUser
    {
        [ProtoMember(1)]
        public int EmployeeId { get; set; }
        [ProtoMember(2)]
        public long UserId { get; set; }
    }
}
