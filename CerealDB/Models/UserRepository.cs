namespace CerealDB.Models
{
    public static class UserRepository
    {
        public static User? Find(string username, string password)
        {
            var users = new List<User>(){
            new User() { Id = 42, Username = "admin", Password = "admin", Role = "admin" }  };
            return users.FirstOrDefault(user => user.Username.ToLower() == username.ToLower() && user.Password == password);
        }
    }
}
