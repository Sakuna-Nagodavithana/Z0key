using System;

namespace Z0key.Models
{
    public class User
    {
        public string UserName { get;}
        public string Password { get; }

        public string ConformPassword { get; }
        public string Email { get; }
        public string Key { get; set; }


        public User(string username, string password, string conformPassword,string email)
        {
            this.UserName = username;
            this.Password = password;
            this.ConformPassword = conformPassword;
            this.Email = email;
            
        }

        public void SetKey(byte[] key)
        {
            this.Key = Convert.ToBase64String(key);
        }

    }
}
