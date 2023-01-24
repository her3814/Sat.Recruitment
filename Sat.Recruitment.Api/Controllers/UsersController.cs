using Microsoft.AspNetCore.Mvc;
using Sat.Recruitment.Application;
using Sat.Recruitment.Application.services;
using Sat.Recruitment.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{

    [ApiController]
    public partial class UsersController : ControllerBase
    {
        private readonly UserService userService;
        public UsersController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("/users")]
        public async Task<AppResult<List<User>>> GetAllUsers()
        {
            var result = await userService.ListAllUsers();
            return result;
        }

        [HttpPost]
        [Route("/user")]
        public async Task<AppResult> AddUser(User user)
        {
            var result = await userService.AddUser(user);
            return result;
        }

    }
}
