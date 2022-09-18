using Lyrox.Framework;
using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Networking.Types;

var lyroxConfiguration = new LyroxConfigurationBuilder()
    .UseGameVersion(GameVersion.Mojang)
    .WithConnection("localhost", 25565)
    .WithCredentials("Botfried")
    .Build();

var lyroxClient = LyroxClientFactory.GetLyroxClient(lyroxConfiguration);
await lyroxClient.Connect();

Console.ReadLine();
