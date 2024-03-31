using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Z0key.Models;

namespace Z0key.Services
{
    public class ServerRequests
    {
        private static readonly HttpClient _sharedClient = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:5091/")
        };


        public async Task<string> PostToCreatUserAsync(User user)
        {
            using StringContent jsonContent = new StringContent(
                JsonSerializer.Serialize(new
                {
                    name = user.UserName,
                    password = user.Password,
                    email = user.Email,
                    key = user.Key,
                }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _sharedClient.PostAsync(
                "setuser",
                jsonContent);


            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }


        public async Task<string?> PostToVerifyUserAsync(VerifyUser user)
        {
            using StringContent jsonContent = new StringContent(
                JsonSerializer.Serialize(new
                {
                    name = user.UserName,
                    password = user.Password,

                }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _sharedClient.PostAsync(
                "verify",
                jsonContent);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return null;
            }
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }

        public async Task<string> PostToChangeUserPasswordAsync(ChangeUserPassword user)
        {
            using StringContent jsonContent = new StringContent(
                JsonSerializer.Serialize(new
                {
                    name = user.UserName,
                    newpassword = user.NewPassword,

                }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _sharedClient.PostAsync(
                "cpwd",
                jsonContent);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }

        public async Task<string> PostToDeleteUserAsync(DeleteUser user)
        {
            using StringContent jsonContent = new StringContent(
                JsonSerializer.Serialize(new
                {
                    name = user.UserName,
                }),
                Encoding.UTF8,
                "application/json");

            using HttpResponseMessage response = await _sharedClient.PostAsync(
                "deleteuser",
                jsonContent);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }
    }
}