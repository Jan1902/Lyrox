using Lyrox;
using Lyrox.Core.Configuration;

var lyroxConfiguration = new LyroxConfigurationBuilder()
    .WithConnection("localhost", 25565)
    .Build();

var lyroxClient = LyroxClientFactory.GetLyroxClient(lyroxConfiguration);

await lyroxClient.Connect();

Console.ReadLine();
