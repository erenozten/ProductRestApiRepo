using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductRestApi.DTOs.Product
{
    public class ProductPostRequestDto
    {
        public string? Name { get; set; }
        public string? About { get; set; }
    }
}