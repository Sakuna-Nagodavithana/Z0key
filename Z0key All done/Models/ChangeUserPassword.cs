using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z0key.Models
{
    public class ChangeUserPassword
    {
        public string UserName { get; }
        public string OldPassword { get; }
        public string NewPassword { get;}

        public ChangeUserPassword(string userName,string oldPassword,string newPassword)
        {
            this.UserName = userName;
            this.NewPassword = newPassword;
            this.OldPassword = oldPassword;
            
        }
    }
}
