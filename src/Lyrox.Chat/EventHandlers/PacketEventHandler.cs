namespace Lyrox.Chat.EventHandlers
{
    //public class PacketEventHandler : IEventHandler<NetworkPacketReceivedEvent<ChatPacket>>
    //{
    //    private readonly IEventManager _eventManager;

    //    public PacketEventHandler(IEventManager eventManager)
    //        => _eventManager = eventManager;

    //    public void HandleEvent(NetworkPacketReceivedEvent<ChatPacket> evt)
    //    {
    //        var chatMessage = new ChatMessage(DateTime.Now, evt.Packet.Message);
    //        _eventManager.PublishEvent(new ChatMessageReceivedEvent(this, chatMessage));
    //    }
    //}
}
