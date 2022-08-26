namespace Lyrox.Core.Configuration
{
    public class LyroxConfiguration
    {
        public string IPAdress { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool DoOnlineAuthentication { get; set; }
    }
}
