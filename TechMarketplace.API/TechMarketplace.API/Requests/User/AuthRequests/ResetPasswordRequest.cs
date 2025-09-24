namespace TechMarketplace.API.Requests.User.AuthRequests
{
    public class ResetPasswordRequest
    {
 
        public string Code { get; set; }    
        public string NewPassword { get; set; }
    }
}
