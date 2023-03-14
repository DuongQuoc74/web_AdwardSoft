using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.Core
{
    [ProtoContract]
    public class Post
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public byte Status { get; set; } //0:Draff, 1: Pending Preview, 2: Publish, 3: Trash
        [ProtoMember(3)]
        public byte Visibility { get; set; }// --0: Public, 1: Password Protected, 2: Private
        [ProtoMember(4)]
        public bool IsStick { get; set; }// Only visible > Visibility = Public
        [ProtoMember(5)]
        public string PasswordProtected { get; set; }// Only visible > Visibility = Password Protected
        [ProtoMember(6)]
        public DateTime PublishedOn { get; set; }
        [ProtoMember(7)]
        public byte Format { get; set; } //0: Standard, 1: Image, 2: Video, 3: Quote, 4: Gallery
        [ProtoMember(8)]
        public string FeaturedImage { get; set; }
        [ProtoMember(9)]
        public string Title { get; set; }
        [ProtoMember(10)]
        public string Description { get; set; }
        [ProtoMember(11)]
        public string Content { get; set; }
        [ProtoMember(12)]
        public string Permalink { get; set; }
        [ProtoMember(13)]
        public bool IsMenuLink { get; set; }
    }

    [ProtoContract]
    public class PostSEO
    {
        [ProtoMember(1)]
        public int PostId { get; set; }
        [ProtoMember(2)]
        public string Title { get; set; }
        [ProtoMember(3)]
        public string Description { get; set; }
        [ProtoMember(4)]
        public string CanonicalURL { get; set; }
        [ProtoMember(5)]
        public byte MetaRobotIndex { get; set; } // 1: Index, 0: NoIndex
        [ProtoMember(6)]
        public byte MetaRobotFollow { get; set; } //  1: Follow, 0: NoFollow
        [ProtoMember(7)]
        public byte MetaRobotAdvanced { get; set; } //0: None, 1: NO ODP, 2: NO YDIR, 3: No Archive, 4: No Snippet
    }


  
    [ProtoContract]
    public class PostInfor
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public byte Status { get; set; } //0:Draff, 1: Pending Preview, 2: Publish, 3: Trash
        [ProtoMember(3)]
        public byte Visibility { get; set; }// --0: Public, 1: Password Protected, 2: Private
        [ProtoMember(4)]
        public bool IsStick { get; set; }// Only visible > Visibility = Public
        [ProtoMember(5)]
        public string PasswordProtected { get; set; }// Only visible > Visibility = Password Protected
        [ProtoMember(6)]
        public DateTime PublishedOn { get; set; }
        [ProtoMember(7)]
        public byte Format { get; set; } //0: Standard, 1: Image, 2: Video, 3: Quote, 4: Gallery
        [ProtoMember(8)]
        public string FeaturedImage { get; set; }
        [ProtoMember(9)]
        public string Title { get; set; }
        [ProtoMember(10)]
        public string Description { get; set; }
        [ProtoMember(11)]
        public string Content { get; set; }
        [ProtoMember(12)]
        public string Permalink { get; set; }
        [ProtoMember(13)]
        public bool IsMenuLink { get; set; }
        [ProtoMember(14)]
        public string TitleSEO { get; set; }
        [ProtoMember(15)]
        public string DescriptionSEO { get; set; }
        [ProtoMember(16)]
        public string CanonicalURL { get; set; }
        [ProtoMember(17)]
        public byte MetaRobotIndex { get; set; } // 1: Index, 0: NoIndex
        [ProtoMember(18)]
        public byte MetaRobotFollow { get; set; } //  1: Follow, 0: NoFollow
        [ProtoMember(19)]
        public byte MetaRobotAdvanced { get; set; } //0: None, 1: NO ODP, 2: NO YDIR, 3: No Archive, 4: No Snippet
    }

    [ProtoContract]
    public class DataTablePostInfor
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public byte Status { get; set; } //0:Draff, 1: Pending Preview, 2: Publish, 3: Trash
        [ProtoMember(3)]
        public byte Visibility { get; set; }// --0: Public, 1: Password Protected, 2: Private
        [ProtoMember(4)]
        public bool IsStick { get; set; }// Only visible > Visibility = Public
        [ProtoMember(5)]
        public string PasswordProtected { get; set; }// Only visible > Visibility = Password Protected
        [ProtoMember(6)]
        public DateTime PublishedOn { get; set; }
        [ProtoMember(7)]
        public byte Format { get; set; } //0: Standard, 1: Image, 2: Video, 3: Quote, 4: Gallery
        [ProtoMember(8)]
        public string FeaturedImage { get; set; }
        [ProtoMember(9)]
        public string Title { get; set; }
        [ProtoMember(10)]
        public string Description { get; set; }
        [ProtoMember(11)]
        public string Content { get; set; }
        [ProtoMember(12)]
        public string Permalink { get; set; }
        [ProtoMember(13)]
        public bool IsMenuLink { get; set; }
        [ProtoMember(14)]
        public string TitleSEO { get; set; }
        [ProtoMember(15)]
        public string DescriptionSEO { get; set; }
        [ProtoMember(16)]
        public string CanonicalURL { get; set; }
        [ProtoMember(17)]
        public byte MetaRobotIndex { get; set; } // 1: Index, 0: NoIndex
        [ProtoMember(18)]
        public byte MetaRobotFollow { get; set; } //  1: Follow, 0: NoFollow
        [ProtoMember(19)]
        public byte MetaRobotAdvanced { get; set; } //0: None, 1: NO ODP, 2: NO YDIR, 3: No Archive, 4: No Snippet
        [ProtoMember(20)]
        public int Count { get; set; }
    }
}
