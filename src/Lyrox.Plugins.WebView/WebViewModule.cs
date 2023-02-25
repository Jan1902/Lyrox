using Lyrox.Framework.Base.Shared;
using Lyrox.Framework.Core.Abstraction.Configuration;
using Lyrox.Framework.Core.Abstraction.Modules;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;

namespace Lyrox.Plugins.WebView;

public class WebViewModule : IModule
{
    public void Load(ServiceContainer serviceContainer, PacketTypeMapping packetMapping, ILyroxConfiguration lyroxConfiguration)
        => serviceContainer.RegisterType<WebViewManager>();
}
