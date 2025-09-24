namespace TechMarketplace.API.Requests.Admin.AdminDetailImgRequests
{
    public class UpdateDetailImgRequest
    {
        public IFormFile File { get; set; }

        public int ProductDetailId { get; set; }
    }
}
