using Microsoft.AspNetCore.Mvc;
using Z0key.Contracts;
using Z0key.Models;
using Z0key.Services.DatabaseServices;

namespace Z0key.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IDatabaseService _databaseService;
        

        public UserController(IDatabaseService DatabaseService)
        {
            _databaseService = DatabaseService;
            
            
        }

        [HttpPost]
        [Route("/setuser")]
        public IActionResult CreatUser(CreatEntryRequest request)
        {
            User user = new User(request.Name, request.Password, request.Email, request.Key);
            _databaseService.CreatDataEntry(user);
            return CreatedAtAction(nameof(VerifyUser),user);
        }


        [HttpPost]
        [Route("/verify")]

        public IActionResult VerifyUser(VerifyUserRequest request)
        {
            VerifyUser user = new VerifyUser(request.Name, request.Password);
            ReturnKey key = _databaseService.VerifyUser(user);
            if (key == null)
            {
                return Unauthorized();
            }
            return Ok(key);
        }

        [HttpPost]
        [Route("/cpwd")]

        public IActionResult ChangeUserPassword(ChangePasswordRequest request)
        {
            ChangeUserPassword user = new ChangeUserPassword(request.Name, request.NewPassword);
            _databaseService.ChangeUserPassword(user);
            return Ok();
            
        }


        [HttpPost]
        [Route("/deleteuser")]
        public IActionResult DeleteUser(DeleteUserRequest request)
        {
            DeleteUser user = new DeleteUser(request.Name);
           _databaseService.DeleteUser(user);
            return Ok();
           
        }
    }
}
