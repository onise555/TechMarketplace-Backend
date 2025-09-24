using System.ComponentModel.DataAnnotations;

namespace TechMarketplace.API.Requests.User.OrderRequests
{
    public class BuyNowRequest
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        public int AddressId { get; set; }
    }
}
