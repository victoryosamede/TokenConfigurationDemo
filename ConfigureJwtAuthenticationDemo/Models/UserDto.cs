namespace ConfigureJwtAuthenticationDemo.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
}
