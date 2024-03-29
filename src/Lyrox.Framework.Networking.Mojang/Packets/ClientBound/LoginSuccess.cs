﻿using Lyrox.Framework.Networking.Mojang.Packets.Base;

namespace Lyrox.Framework.Networking.Mojang.Packets.ClientBound;

public class LoginSuccess : MojangClientBoundPacketBase
{
    public Guid UUID { get; private set; }
    public string Username { get; private set; }

    public override void Parse()
    {
        UUID = Reader.ReadUUID();
        Username = Reader.ReadStringWithVarIntPrefix();
    }
}
