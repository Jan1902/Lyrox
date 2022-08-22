namespace Lyrox.Actions
{
    public interface IActionQueue
    {
        void Enqueue(IAction action);
        void Dequeue(IAction action);
        void EnqueueImmediately(IAction action);
    }
}
