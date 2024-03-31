namespace Z0key.Models
{
    public class ChangeUserPassword
    {
        public string UserName { get; }
        public string NewPassword { get; }

        public DateTime LastModifiedTime = DateTime.UtcNow;

        public ChangeUserPassword(string username,string newPassword)
        {
            this.UserName = username;
            this.NewPassword = newPassword;
        }
    }
}
