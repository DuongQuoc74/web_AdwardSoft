using AdwardSoft.DTO.Generic;
using ProtoBuf;

namespace AdwardSoft.DTO.Presentation.Core
{
    [ProtoContract]
    public class Menu
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public int ParentId { get; set; }

        [ProtoMember(3)]
        public int Order { get; set; }

        [ProtoMember(5)]
        public short Type { get; set; }

        [ProtoMember(6)]
        public int MenuGroupId { get; set; }

        [ProtoMember(7)]
        public string LanguageCode { get; set; }

        [ProtoMember(8)]
        public string NavigationLabel { get; set; }

        [ProtoMember(9)]
        public string URL { get; set; }

        [ProtoMember(10)]
        public int ReferenceId { get; set; }

        [ProtoMember(11)]
        public bool IsInMenu { get; set; }
        [ProtoMember(12)]
        public string MenuGroupName { get; set; }
    }

    [ProtoContract]
    public class MenuTranslation
    {
        [ProtoMember(1)]
        public int MenuId { get; set; }

        [ProtoMember(2)]
        public string LanguageCode { get; set; }

        [ProtoMember(3)]
        public string URL { get; set; }

        [ProtoMember(4)]
        public string NavigationLabel { get; set; }
    }

    [ProtoContract]
    public class MenuJson : JsonData
    {
    }
}
