using Lyrox.Framework;
using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Networking.Types;
using Lyrox.Framework.Shared.Types;
using Lyrox.Plugins.WebView;

var lyroxConfiguration = new LyroxConfigurationBuilder()
    .UseGameVersion(GameVersion.Mojang)
    .WithConnection("localhost", 25565)
    .WithCredentials("Botfried")
    .UseWebViewPort(8080)
    .Build();

var lyroxClient = LyroxClientFactory.GetLyroxClient(lyroxConfiguration, builder => builder.AddWebView());
await lyroxClient.Connect();

lyroxClient.ChatMessageReceived += (s, e) => HandleChatMessage(e.Message);

Console.ReadLine();

void HandleChatMessage(string message)
{
    var tokens = message.ToLower().Split(" ");
    if (tokens.Length < 2)
        return;

    if (tokens[0] != "!" + lyroxConfiguration.Username.ToLower())
        return;

    lyroxClient.SendChatMessage("Aye sir!");

    if (tokens[1] == "goto")
        lyroxClient.Goto(new Vector3d(double.Parse(tokens[2]), double.Parse(tokens[3]), double.Parse(tokens[4])));

    else if (tokens[1] == "block")
        lyroxClient.SendChatMessage(lyroxClient.GetBlock(new Vector3i(int.Parse(tokens[2]), int.Parse(tokens[3]), int.Parse(tokens[4]))).BlockName);

    else if (tokens[1] == "chunk")
    {
        var chunkPos = new Vector3i(int.Parse(tokens[2]), int.Parse(tokens[3]), int.Parse(tokens[4]));
        var offsetY = int.Parse(tokens[5]);
        var section = lyroxClient.GetChunkSection(chunkPos);

        var text = "";
        for (int z = 0; z < 16; z++)
        {
            var line = "";
            for (int x = 0; x < 16; x++)
                line += section.BlockStates[x, offsetY, z]?.BlockName.Split(":").Last().First();

            text += line + "\n";
        }

        lyroxClient.SendChatMessage(text);
    }
}
