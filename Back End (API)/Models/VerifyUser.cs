namespace Z0key.Models
{
    public class VerifyUser
    {
        public string UserName { get; }
        public string Password { get; }

        public VerifyUser(string username, string password)
        {
            this.UserName = username;
            this.Password = password;
        }
    }
}
