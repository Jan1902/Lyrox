using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Core.Modules.Abstractions;

namespace Lyrox.Plugins.WebView
{
    public static class Extensions
    {
        public static void AddWebView(this IModuleManager manager)
            => manager.RegisterModule<WebViewModule>();

        public static LyroxConfigurationBuilder UseWebViewPort(this LyroxConfigurationBuilder configurationBuilder, int port)
            => configurationBuilder.WithCustomOption("webViewPort", port);
    }
}
