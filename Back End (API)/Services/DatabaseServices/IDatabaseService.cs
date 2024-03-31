using Z0key.Models;

namespace Z0key.Services.DatabaseServices
{
    public interface IDatabaseService
    {
        public void CreatDataEntry(User user);
        public ReturnKey VerifyUser(VerifyUser user);

        public void ChangeUserPassword(ChangeUserPassword user);
        public void DeleteUser(DeleteUser user);


    }
}
