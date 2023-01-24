using Sat.Recruitment.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sat.Recruitment.Domain.Repositories
{
    public interface IUsersRepository
    {
        Task AddUser(User newUser);
        Task<IEnumerable<User>> GetUsers();
        

    }
}
