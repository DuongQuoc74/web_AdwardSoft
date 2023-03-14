using AdwardSoft.Utilities.Attributes;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdwardSoft.DTO.Presentation.POS
{
    [ProtoContract]
    [ProtoInclude(100, typeof(CategoryDatatable))]
    public class Category
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Name { get; set; }
        [ProtoMember(3)]
        public string Description { get; set; }
        [ProtoMember(4)]
        public int Status { get; set; }
    }

    [ProtoContract]
    public class CategoryDatatable : Category
    {
        [ProtoMember(5)]
        public int Count { get; set; }
    }

    public class CategoryRedis : Category
    {
        [AdsKey]
        public new int Id { get; set; }
    }

    public class CategoryElastic
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
