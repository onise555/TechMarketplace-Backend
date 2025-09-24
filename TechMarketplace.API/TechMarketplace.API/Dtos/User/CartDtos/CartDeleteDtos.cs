namespace TechMarketplace.API.Dtos.User.CartDtos
{
    public class CartDeleteDtos
    {

        public string Message { get; set; }
        public int RemovedItemsCount { get; set; }
        public decimal TotalValueRemoved { get; set; }
        public List<int> RemovedItemIds { get; set; } = new List<int>();
    }
}
