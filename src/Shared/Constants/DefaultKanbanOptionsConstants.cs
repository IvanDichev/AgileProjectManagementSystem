namespace Shared.Constants
{
    public class DefaultKanbanOptionsConstants
    {
        public const string Backlog = "Backlog";
        public const byte BacklogPosition = 1;
        public const ushort BacklogMaxItems = 3500;

        public const string Doing = "Backlog";
        public const byte DoingPosition = 1;
        public const ushort DoingMaxItems = 3500;

        public const string Done = "Done";
        public const byte DonePosition = byte.MaxValue;
        public const ushort DoneMaxItems = 5000;
    }
}
