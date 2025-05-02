using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductRestApi.Interfaces;

namespace ProductRestApi.Entities
{
    public class Product : IProduct
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? About { get; set; }
    }
}