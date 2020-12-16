namespace DataModels.Models.WorkItems.UserStory
{
    public class PaginatedAllViewModel
    {
        public UserStoryAllViewmodel ViewModel { get; set; }

        public int RecordsPerPage { get; set; }

        public int TotalPages { get; set; }
    }
}
