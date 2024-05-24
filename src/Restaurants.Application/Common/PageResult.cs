namespace Restaurants.Application.Common;

public class PageResult<T>
{
    public PageResult(IEnumerable<T> items, int totalItemsCount, int pageSize, int pageNumber)
    {
        Items = items;
        TotalItemsCount = totalItemsCount;
        TotalPages = (int)Math.Ceiling(TotalItemsCount / (double)pageSize);
        ItemsFrom = (pageNumber - 1) * pageSize + 1;
        ItemsTo =
            (ItemsFrom <= TotalItemsCount && ItemsFrom > pageSize * (TotalPages - 1))
                ? TotalItemsCount
                : (ItemsFrom + pageSize - 1);
    }

    public IEnumerable<T> Items { get; set; } = [];
    public int TotalPages { get; set; }
    public int TotalItemsCount { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
}
