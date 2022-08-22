namespace Lyrox.Core.Configuration
{
    public class LyroxConfigurationBuilder
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

        public LyroxConfiguration Build()
            => _configuration;
    }
}
