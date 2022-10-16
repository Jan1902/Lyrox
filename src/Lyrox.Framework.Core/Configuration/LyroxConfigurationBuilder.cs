using System.Collections.Immutable;
using Lyrox.Framework.Core.Networking.Types;

namespace Lyrox.Framework.Core.Configuration
{
    public class LyroxConfigurationBuilder : ILyroxConfigurationBuilder
    {
        private string _ipAdress = "localhost";
        private int _port = 25565;
        private string _username = "Bot";
        private string _password;
        private bool _doOnlineAuthentication;
        private GameVersion _gameVersion = GameVersion.Mojang;
        private readonly Dictionary<string, object> _customOptions = new();

        public LyroxConfigurationBuilder WithConnection(string ipAdress, int port)
        {
            _ipAdress = ipAdress;
            _port = port;
            return this;
        }

        public LyroxConfigurationBuilder WithCredentials(string username, string password)
        {
            _username = username;
            _password = password;
            return this;
        }

        public LyroxConfigurationBuilder WithCredentials(string username)
        {
            _username = username;
            return this;
        }

        public LyroxConfigurationBuilder DoOnlineAuthentication()
        {
            _doOnlineAuthentication = true;
            return this;
        }

        public LyroxConfigurationBuilder UseGameVersion(GameVersion gameVersion)
        {
            _gameVersion = gameVersion;
            return this;
        }

        public LyroxConfigurationBuilder WithCustomOption(string option, object value)
        {
            _customOptions.Add(option, value);
            return this;
        }

        public LyroxConfiguration Build()
            => new(_ipAdress,
                _port,
                _username,
                _password,
                _doOnlineAuthentication,
                _gameVersion,
                _customOptions.ToImmutableDictionary());
    }
}
