using System.Web.Http;
using WebService.Models.Req.Users;
using WebService.Repositories;
using WebService.Models.Res.Users;

namespace WebService.ApiControllers.Users
{
    public class UserController : ApiController
    {
        [Route("api/v1/users/web/login")]
        [HttpPost()]
        public User Login([FromBody] UserLogin login)
        {
            UserLoginRepo loginRepo = new UserLoginRepo();
            try
            {
                if (!ModelState.IsValid)
                    return new User
                    {
                        Status = "0",
                        Message = "Invalid username or password.",
                    };

                var user = loginRepo.Login(login);
                return user;
            }
            catch
            {
                return new User
                {
                    Status = "0",
                    Message = "Invalid username or password."
                };
            }
        }
    }
}