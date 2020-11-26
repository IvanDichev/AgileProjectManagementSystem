namespace DataModels.Models.Comments.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int WorkItemId { get; set; }
    }
}
