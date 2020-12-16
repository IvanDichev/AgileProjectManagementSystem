namespace DataModels.Models.Users.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string TeamRoleRole { get; set; }

        public bool IsPublic { get; set; }
    }
}
