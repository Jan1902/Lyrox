namespace Lyrox.Framework.Actions.Abstraction
{
    public interface IAction
    {
        void Execute();
        void Halt();
        void Continue();
        bool IsRunning { get; }
        IEnumerable<IAction> GetPrerequisites();
    }
}
