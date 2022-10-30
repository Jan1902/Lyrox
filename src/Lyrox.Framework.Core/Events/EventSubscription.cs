namespace Lyrox.Framework.Core.Events
{
    public class EventSubscription : IDisposable
    {
        private bool _disposed;

        public event EventHandler? Disposed;

        public Guid UUID { get; }

        public EventSubscription()
            => UUID = Guid.NewGuid();

        public void Dispose()
        {
            if (_disposed)
                return;

            Disposed?.Invoke(this, EventArgs.Empty);
            _disposed = true;
        }
    }
}
