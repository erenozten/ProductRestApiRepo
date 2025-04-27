namespace ProductRestApi.DTOs;

public class ListWithCountDto<TItem>
{
    public List<TItem> Items { get; set; } = new();
    public int TotalCount { get; set; }
}