namespace Lyrox.Framework.Core.Events
{
    public abstract record EventBase
    {
        public DateTime CreationTime { get; }

        public EventBase()
            => CreationTime = DateTime.Now;
    }
}
