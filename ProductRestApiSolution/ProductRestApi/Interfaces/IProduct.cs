namespace ProductRestApi.Interfaces;

public interface IProduct
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? About { get; set; }
}