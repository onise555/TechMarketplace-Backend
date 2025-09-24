namespace TechMarketplace.API.Requests.User.AuthRequests
{
    public class VerifyRequest
    {
        public string Email { get; set; }   

        public string Code { get; set; }    
    }
}
