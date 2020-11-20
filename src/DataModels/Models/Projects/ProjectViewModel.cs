namespace DataModels.Models.Projects
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ShortDescription => 
            this.Description?.Length > 150 ? this.Description.Substring(0, 150) + "..." : this.Description;

        public int Count { get; set; }
    }
}
