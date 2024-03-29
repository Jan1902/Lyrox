﻿using System.Collections.Immutable;
using Lyrox.Framework.Core.Abstraction.Configuration;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Configuration;

public class LyroxConfiguration : ILyroxConfiguration
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
