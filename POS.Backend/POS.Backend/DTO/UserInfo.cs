namespace POS.Backend.DTO
{
    public class UserInfo
    {
        public UserInfo() { 
            
            this.UserId = string.Empty;
            this.FullName = string.Empty;
            this.Email = string.Empty;
        }

        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
