namespace POS.Backend.Models.Configuration
{
    public class AppSettings
    {
        //public AppSettings()
        //{
        //    this.IsOtpEnabled = false;
        //    this.ConnectionStrings = new ConnectionStrings();
        //}
        public bool IsOtpEnabled { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }
}
