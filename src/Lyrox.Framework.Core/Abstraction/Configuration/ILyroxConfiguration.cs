using System.Collections.Immutable;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Core.Configuration;

public interface ILyroxConfiguration
{
    ImmutableDictionary<string, object> CustomOptions { get; }
    bool DoOnlineAuthentication { get; }
    GameVersion GameVersion { get; }
    string IPAdress { get; }
    string Password { get; }
    int Port { get; }
    string Username { get; }
}