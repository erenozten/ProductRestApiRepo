using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductRestApi.Entities;

namespace ProductRestApi.Interfaces.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> GetProductsByNameAsync(string name);
    }
}