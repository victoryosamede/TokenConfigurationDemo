using ConfigureJwtAuthenticationDemo.Models;

namespace ConfigureJwtAuthenticationDemo.Services
{
    public class UsersStore
    {
        private readonly static List<User> _users = new();
        static UsersStore()
        {
            _users.AddRange(new List<User>()
        {
            new User {Id = 1, UserName="Bart Allen", PasswordHash=BCrypt.Net.BCrypt.HashPassword("password")},
            new User {Id = 2, UserName="Bart Allen", PasswordHash=BCrypt.Net.BCrypt.HashPassword("password")},
        });
        }

        public static void AddNewUser(User user) => _users.Add(user);

        public static IEnumerable<User> GetAllUsers() => _users;
    }
}
