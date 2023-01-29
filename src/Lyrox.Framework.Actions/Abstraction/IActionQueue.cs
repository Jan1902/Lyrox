namespace Lyrox.Framework.Actions.Abstraction;

public interface IActionQueue
{
    void Enqueue(IAction action);
    void EnqueueImmediately(IAction action);
}
