using ProductRestApi.Entities;
using ProductRestApi.Interfaces;

namespace ProductRestApi.Common.Logging;

public class ProductLogModel: IProduct
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? About { get; set; }
}