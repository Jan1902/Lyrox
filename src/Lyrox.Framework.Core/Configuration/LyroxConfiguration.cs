using System.Collections.Immutable;
using Lyrox.Framework.Core.Networking.Types;

namespace Lyrox.Framework.Core.Configuration
{
    public class LyroxConfiguration
    {
        public string IPAdress { get; }
        public int Port { get; }
        public string Username { get; }
        public string Password { get; }
        public bool DoOnlineAuthentication { get; }
        public GameVersion GameVersion { get; }
        public ImmutableDictionary<string, object> CustomOptions { get; }

        public LyroxConfiguration(string iPAdress, int port, string username, string password, bool doOnlineAuthentication, GameVersion gameVersion, ImmutableDictionary<string, object> customOptions)
        {
            IPAdress = iPAdress;
            Port = port;
            Username = username;
            Password = password;
            DoOnlineAuthentication = doOnlineAuthentication;
            GameVersion = gameVersion;
            CustomOptions = customOptions;
        }
    }
}
