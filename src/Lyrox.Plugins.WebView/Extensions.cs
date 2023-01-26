using Lyrox.Framework.Configuration;
using Lyrox.Framework.Core.Abstraction.Modules;

namespace Lyrox.Plugins.WebView;

public static class Extensions
{
    public static void AddWebView(this IModuleManager manager)
        => manager.RegisterModule<WebViewModule>();

    public static LyroxConfigurationBuilder UseWebViewPort(this LyroxConfigurationBuilder configurationBuilder, int port)
        => configurationBuilder.WithCustomOption("webViewPort", port);
}
