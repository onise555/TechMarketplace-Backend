namespace TechMarketplace.API.Requests.User
{
    public class ResetPasswordRequest
    {
 
        public string Code { get; set; }    
        public string NewPassword { get; set; }
    }
}
