﻿using Lyrox.Framework.Base.Messaging.Abstraction;
using Lyrox.Framework.Base.Shared;
using Lyrox.Framework.Core.Abstraction.Configuration;
using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Core.Abstraction.Modules;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Networking.Mojang;
using Lyrox.Framework.Networking.Mojang.MessageHandlers;
using Lyrox.Framework.Networking.Mojang.PacketHandlers;
using Lyrox.Framework.Networking.Mojang.Packets.ClientBound;
using Lyrox.Framework.Shared.Exceptions;
using Lyrox.Framework.Shared.Messages;
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
            serviceContainer.RegisterMessageHandler<ConnectionEstablishedMessage, MojangNetworkingMessageHandler>();
        }
        else
            throw new GameVersionNotSupportedException(lyroxConfiguration.GameVersion);
    }
}
