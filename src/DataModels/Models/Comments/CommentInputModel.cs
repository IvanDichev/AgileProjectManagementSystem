namespace DataModels.Models.Comments
{
    public class CommentInputModel
    {
        public string Description { get; set; }
        public int UserStoryId { get; set; }
        public int AddedById { get; set; }
    }
}
