namespace DataModels.Models.Comments.Dtos
{
    public class CommentsUpdateModel
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int UserStoryId { get; set; }

        public int AddedById { get; set; }
    }
}
