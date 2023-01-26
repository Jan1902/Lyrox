using Lyrox.Framework.Base.Shared;
using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Core.Abstraction.Modules;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Core.Configuration;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Networking.Mojang;
using Lyrox.Framework.Networking.Mojang.PacketHandlers;
using Lyrox.Framework.Networking.Mojang.Packets.ClientBound;
using Lyrox.Framework.Shared.Exceptions;
using Lyrox.Framework.Shared.Types;

namespace Lyrox.Framework.Networking;

public class NetworkingModule : IModule
{
    public void Load(ServiceContainer serviceContainer, PacketTypeMapping packetMapping, ILyroxConfiguration lyroxConfiguration)
    {
        serviceContainer.RegisterType<INetworkingManager, NetworkingManager>();

        if (lyroxConfiguration.GameVersion == GameVersion.Mojang)
        {
            serviceContainer.RegisterType<INetworkConnection, NetworkConnection>();

            serviceContainer.RegisterRawPacketHandler<MojangNetworkingPacketHandler>();
            serviceContainer.RegisterPacketHandler<KeepAliveCB, MojangNetworkingPacketHandler>(packetMapping, 0x20);
        }
        else
            throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
    }
}
