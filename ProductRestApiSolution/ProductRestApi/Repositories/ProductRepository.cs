using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductRestApi.Data;
using ProductRestApi.Entities;
using ProductRestApi.Interfaces.Repositories;

namespace ProductRestApi.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context) { }

        public async Task<List<Product>> GetProductsByNameAsync(string name)
        {
            return await _context.Products
                .Where(c => c.Name!.Contains(name))
                .ToListAsync();
        }
    }
}


