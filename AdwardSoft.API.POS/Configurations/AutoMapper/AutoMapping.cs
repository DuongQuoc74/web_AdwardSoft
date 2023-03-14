using AdwardSoft.DTO.Presentation.POS;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdwardSoft.API.POS.Configurations.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CustomerRedis, CustomerElastic>();
            CreateMap<EmployeeRedis, EmployeeElastic>();
            CreateMap<SupplierRedis, SupplierElastic>();
            CreateMap<ProductRedis, ProductElastic>();
            CreateMap<CategoryRedis, CategoryElastic>();
            CreateMap<SaleReceiptDetailDataTable, SaleReceiptDetail>();
        }
    }
}
