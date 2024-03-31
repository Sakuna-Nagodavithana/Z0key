using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Z0key.Models;
using System.Text.Json;
using System.IO;
using System.Windows;

namespace Z0key.Services
{
    class UserServices:ServerRequests
    {
        private readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private readonly User _user;
        private readonly VerifyUser _verifyUser;
        private AESEncryptor _encryptor = new AESEncryptor();
        private byte[] _key = new byte[32];
        private string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files");
        public DateTime LastModifiedTime { get; set; }



        public UserServices(User user):base()
        {
            this._user = user;
            rng.GetBytes(_key);
            _user.SetKey(_key);
            _encryptor.Key = _key;
        }

        public UserServices(VerifyUser user):base()
        {
            this._verifyUser = user;
        }



        public string? CheckUser()
        {
            bool isEmail = Regex.IsMatch(_user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (_user.Password != _user.ConformPassword)
            {
                return "Conform Password Correctly";
            }

            if (!isEmail)
            {
                return "Invalid Email";
            }

            return null;
        }

        public void DeleteUser(DeleteUser user)
        {
            Task<string> requestResult = base.PostToDeleteUserAsync(user);

        }

        public void ChangeUserPassword(ChangeUserPassword user)
        {
            Task<string> requestResult = base.PostToChangeUserPasswordAsync(user);
            
        }

        public void CreatUser()
        {
            Task<string> requestResult = base.PostToCreatUserAsync(_user);
        }

        public async Task<string?> VerifyUser()
        {
            Task<string?> requestResult = base.PostToVerifyUserAsync(_verifyUser);
            string result = await requestResult;

            if (result == null)
            {
                return "Invalid Credentials";
            }
            else
            {
                ReturnKey returnKey = JsonSerializer.Deserialize<ReturnKey>(result);
                _encryptor.Key = Convert.FromBase64String(returnKey.Key);
                LastModifiedTime = returnKey.LastModifiedTime;


                if (Directory.Exists(folderPath))
                {
                    string[] files = Directory.GetFiles(folderPath);
                    foreach (string file in files)
                    {
                        _encryptor.DecryptFile(file);
                    }
                }

                return null;
            }
            
        }

        public void CloseApp()
        {
            
            if (Directory.Exists(folderPath))
            {
                string[] files = Directory.GetFiles(folderPath);
                foreach (string file in files)
                {
                    _encryptor.EncryptFile(file);
                    Console.WriteLine($"File encrypted {file}");
                }
            }
            
            
        }

    }
}
