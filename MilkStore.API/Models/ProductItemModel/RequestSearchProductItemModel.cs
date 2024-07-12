namespace MilkStore.API.Models.ProductItemModel
{
    public class RequestSearchProductItemModel
    {
        public string? ItemName { get; set; }
        public decimal? FromPrice { get; set; } = decimal.Zero;
        public decimal? ToPrice { get; set; } = null;
        public string? BrandName { get; set; }
        public string? CompanyName { get; set; }
        public string? CountryName { get; set; }
        public string? Mama { get; set; }
        public string? Baby { get; set; }
        public SortContent? SortContent { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class SortContent
    {
        public SortProductItemTypeEnum? SortPrice { get; set; }
        public SortSoldQuantityEnum? SortSoldQuantity { get; set; } // New property for sorting by SoldQuantity
    }

    public enum SortProductItemTypeEnum
    {
        Ascending = 1,
        Descending = 2,
    }

    public enum SortSoldQuantityEnum
    {
        Ascending = 1,
        Descending = 2,
    }
}
