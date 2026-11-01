namespace CommandPattern
{
    internal class CommandInvoker
    {
        private readonly List<ICommand> _commands = new();

        public void AddCommand(ICommand command)
        {
            _commands.Add(command);
        }

        //public void RemoveCommand(ICommand command)
        //{
        //    _commands.Remove(command);
        //}

        //public void GetAllCommands()
        //{
        //    Console.WriteLine("List of Commands:");
        //    foreach (var command in _commands)
        //    {
        //        Console.WriteLine(command.GetType().Name);
        //    }
        //}

        //public void ClearCommands()
        //{
        //    _commands.Clear();
        //    Console.WriteLine("All commands cleared.");
        //}

        //public void UpdateCommand(int index, ICommand newCommand)
        //{
        //    if (index < 0 || index >= _commands.Count)
        //    {
        //        Console.WriteLine("Invalid command index.");
        //        return;
        //    }
        //    _commands[index] = newCommand;
        //    Console.WriteLine($"Command at index {index} updated to {newCommand.GetType().Name}.");
        //}

        //public void DeleteCommand(int index)
        //{
        //    if (index < 0 || index >= _commands.Count)
        //    {
        //        Console.WriteLine("Invalid command index.");
        //        return;
        //    }
        //    var removedCommand = _commands[index];
        //    _commands.RemoveAt(index);
        //    Console.WriteLine($"Command {removedCommand.GetType().Name} at index {index} removed.");
        //}

        public void ExecuteCommands()
        {
            foreach (var command in _commands)
            {
                command.Execute();
            }
            _commands.Clear();
        }
    }
}
