using AdwardSoft.DTO.Presentation.POS;
using Microsoft.Extensions.Hosting;
using Nest;
using System.Threading;
using System.Threading.Tasks;

namespace AdwardSoft.API.POS.SearchEngineer.Elastic
{
    public class ElasticsearchHostedService : IHostedService
    {
        private readonly IElasticClient _elasticClient;
        public ElasticsearchHostedService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //Product
            if (!_elasticClient.Indices.Exists("product").Exists)
            {
                _elasticClient.Indices.Create("product", index => index.Map<ProductElastic>(x => x.AutoMap()));
            }

            // Supplier
            if (!_elasticClient.Indices.Exists("supplier").Exists)
            {
                _elasticClient.Indices.Create("supplier", index => index.Map<SupplierElastic>(x => x.AutoMap()));
            }

            // Customer
            if (!_elasticClient.Indices.Exists("customer").Exists)
            {
                _elasticClient.Indices.Create("customer", index => index.Map<CustomerElastic>(x => x.AutoMap()));
            }

            // Employee
            if (!_elasticClient.Indices.Exists("employee").Exists)
            {
                _elasticClient.Indices.Create("employee", index => index.Map<EmployeeElastic>(x => x.AutoMap()));
            }
        }
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
