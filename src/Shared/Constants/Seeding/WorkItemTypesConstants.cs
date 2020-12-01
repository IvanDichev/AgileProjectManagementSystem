namespace Shared.Constants.Seeding
{
    public class WorkItemTypesConstants
    {
        private static readonly string[] workItemTypesList = new string[] { Bug, UserStory, Test, Task };
        
        public const string Bug = "Bug";
        public const string UserStory = "User Story";
        public const string Test = "Test";
        public const string Task = "Task";
        public static string[] WorkItemTypesList { get => workItemTypesList; }
    }
}
