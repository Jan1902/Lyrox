using Lyrox.Framework.Core.Networking.Types;

namespace Lyrox.Framework.Core.Exceptions
{
    public class GameVersionNotSupportedException : Exception
    {
        public GameVersion GameVersion { get; private set; }

        public GameVersionNotSupportedException(GameVersion gameVersion, string message) : base(message)
            => GameVersion = gameVersion;

        public GameVersionNotSupportedException(GameVersion gameVersion) : base($"The {gameVersion} game version is currently not supported yet")
            => GameVersion = gameVersion;
    }
}
