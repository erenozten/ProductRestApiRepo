using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductRestApi.DTOs.Product
{
    public class ProductPostResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? About { get; set; }
    }
}