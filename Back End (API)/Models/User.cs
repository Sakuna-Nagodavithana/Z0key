namespace Z0key.Models
{
    public class User
    {
        public string UserName { get;}
        public string Password { get; }
        public string Email { get; }
        public string Key { get;  }
        public DateTime LastModifiedTime = DateTime.UtcNow;

        public User(string username, string password, string email,string key)
        {
            this.UserName = username;
            this.Password = password;
            this.Email = email;
            this.Key = key;

        }

    }
}
