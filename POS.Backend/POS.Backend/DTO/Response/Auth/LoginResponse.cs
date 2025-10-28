namespace POS.Backend.DTO.Response.Auth
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            this.Email = string.Empty;
            this.AccessToken = string.Empty;
            this.RefreshToken = string.Empty;
            this.IsOtpEnable = false;
        }

        public string Email { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public bool IsOtpEnable { get; set; }
    }
}