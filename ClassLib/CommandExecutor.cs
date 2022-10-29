namespace ClassLib
{
    public abstract class CommandExecutor
    {
        public abstract Task Execute(Command command);
    }
}
