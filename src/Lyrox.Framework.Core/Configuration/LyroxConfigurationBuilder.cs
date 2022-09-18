using Lyrox.Framework.Core.Networking.Types;

namespace Lyrox.Framework.Core.Configuration
{
    public class LyroxConfigurationBuilder : ILyroxConfigurationBuilder
    {
        private LyroxConfiguration _configuration;

        public LyroxConfigurationBuilder()
            => _configuration = new();

        public LyroxConfigurationBuilder WithConnection(string ipAdress, int port)
        {
            _configuration.IPAdress = ipAdress;
            _configuration.Port = port;
            return this;
        }

        public LyroxConfigurationBuilder WithCredentials(string username, string password)
        {
            _configuration.Username = username;
            _configuration.Password = password;
            return this;
        }

        public LyroxConfigurationBuilder WithCredentials(string username)
        {
            _configuration.Username = username;
            return this;
        }

        public LyroxConfigurationBuilder DoOnlineAuthentication()
        {
            _configuration.DoOnlineAuthentication = true;
            return this;
        }

        public LyroxConfigurationBuilder UseGameVersion(GameVersion gameVersion)
        {
            _configuration.GameVersion = gameVersion;
            return this;
        }

        public LyroxConfiguration Build()
            => _configuration;
    }
}
