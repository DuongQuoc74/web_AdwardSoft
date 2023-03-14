using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    public class UserBranch
    {
        [ProtoMember(1)]
        public long UserId { get; set; }
        [ProtoMember(2)]
        public int BranchId { get; set; }
    }
}
