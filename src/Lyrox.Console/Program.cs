using Lyrox;
using Lyrox.Core.Configuration;
using Lyrox.Core.Networking.Types;

var lyroxConfiguration = new LyroxConfigurationBuilder()
    .UseGameVersion(GameVersion.Mojang)
    .WithConnection("localhost", 25565)
    .WithCredentials("Botfried")
    .Build();

var lyroxClient = LyroxClientFactory.GetLyroxClient(lyroxConfiguration);
await lyroxClient.Connect();

lyroxClient.ChatMessageReceived += (e, chatMessage)
    => lyroxClient.SendChatMessage(chatMessage.Text);

Console.ReadLine();
