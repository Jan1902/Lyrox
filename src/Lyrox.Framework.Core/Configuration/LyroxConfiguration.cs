using Lyrox.Framework.Core.Networking.Types;

namespace Lyrox.Framework.Core.Configuration
{
    public class LyroxConfiguration
    {
        public string IPAdress { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool DoOnlineAuthentication { get; set; }
        public GameVersion GameVersion { get; set; }
    }
}
