using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z0key.Models
{

    public class DeleteUser
    {
        public string UserName { get; }
        public string Password { get; }

        public DeleteUser(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }

    }
}
