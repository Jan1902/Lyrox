namespace Lyrox.Core.Events
{
    public abstract class Event
    {
        public DateTime CreationTime { get; }

        public Event()
            => CreationTime = DateTime.Now;
    }
}
