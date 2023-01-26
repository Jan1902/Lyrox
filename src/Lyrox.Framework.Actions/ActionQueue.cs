using Lyrox.Framework.Actions.Abstraction;

namespace Lyrox.Framework.Actions;

internal class ActionQueue : IActionQueue
{
    private readonly Queue<IAction> _queue;
    private IAction? _currentAction;
    private readonly Thread _runThread;

    public ActionQueue()
    {
        _queue = new();
        _runThread = new Thread(() => Run());
    }

    private void Run()
    {
        while (_queue.Any())
        {
            var action = _queue.Dequeue();
            _currentAction = action;
            action.Execute();
        }
    }

    public void Enqueue(IAction action)
    {
        foreach (var prerequisite in action.GetPrerequisites())
            _queue.Enqueue(prerequisite);

        _queue.Enqueue(action);

        if (!_runThread.IsAlive)
            _runThread.Start();
    }

    public void EnqueueImmediately(IAction action)
    {
        if (_runThread.IsAlive)
        {
            _currentAction?.Halt();
            _currentAction = action;
            action.Execute();
        }
    }
}
