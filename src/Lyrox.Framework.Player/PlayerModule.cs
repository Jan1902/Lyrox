using Lyrox.Framework.Base.Shared;
using Lyrox.Framework.Core.Abstraction.Configuration;
using Lyrox.Framework.Core.Abstraction.Managers;
using Lyrox.Framework.Core.Abstraction.Modules;
using Lyrox.Framework.Core.Abstraction.Networking.Packet;
using Lyrox.Framework.Networking.Core;
using Lyrox.Framework.Player.Mojang.PacketHandlers;
using Lyrox.Framework.Player.Mojang.Packets.ClientBound;

namespace Lyrox.Framework.Player;

public class PlayerModule : IModule
{
    public void Load(ServiceContainer serviceContainer, PacketTypeMapping packetMapping, ILyroxConfiguration lyroxConfiguration)
    {
        serviceContainer.RegisterType<IPhysicsPlayer, PhysicsPlayer>();
        serviceContainer.RegisterType<IPlayerManager, PlayerManager>();

        serviceContainer.RegisterPacketHandler<SynchronizePlayerPosition, PlayerPacketHandler>(packetMapping, 0x39);
    }
}
