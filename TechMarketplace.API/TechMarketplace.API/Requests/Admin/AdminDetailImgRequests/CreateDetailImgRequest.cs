namespace TechMarketplace.API.Requests.Admin.AdminDetailImgRequests
{
    public class CreateDetailImgRequest
    {
        public IFormFile File { get; set; }

        public int ProductDetailId { get; set; }
    }
}
