using Z0key.Models;
using Z0key.Services.DatabaseServices;

namespace Z0key.Services.UserServices
{
    public class UserServices
    {
        public DatabaseService DatabaseServices;

        public UserServices()
        {
            this.DatabaseServices = new DatabaseService();
        }

        public void AddUser(User user)
        {
            DatabaseServices.CreatDataEntry(user);
        }

        public void VerifyUser(VerifyUser user)
        {
            DatabaseServices.VerifyUser(user);
        }
    }
}
