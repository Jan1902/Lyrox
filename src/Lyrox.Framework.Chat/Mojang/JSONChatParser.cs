using Newtonsoft.Json.Linq;

namespace Lyrox.Framework.Chat.Mojang;

internal class JSONChatParser : IJSONChatParser
{
    public string ParseChatJson(string json)
        => ParseRecursive(JObject.Parse(json));

    private string ParseRecursive(JToken chatElement)
    {
        var text = string.Empty;
        if (chatElement["text"] != null)
            text += chatElement["text"];

        foreach (var child in chatElement.Children().Where(c => c is JArray))
            foreach (var part in child)
                text += ParseRecursive(child);

        return text;
    }
}
