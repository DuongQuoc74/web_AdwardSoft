using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.Core
{
    [ProtoContract]
    public class MenuGroup
    {
        
        [ProtoMember(1)]
        public int Id { get; set; }
        
        [ProtoMember(2)]
        public string Name { get; set; }
        
        [ProtoMember(3)]
        public short Position { get; set; }

        [ProtoMember(4)]
        public int Count { get; set; }
    }

    [ProtoContract]
    public class MenuGroupSort
    {
        [ProtoMember(1)]
        public int SelectedId { get; set; }
        [ProtoMember(2)]
        public bool IsUp { get; set; }
    }
}
