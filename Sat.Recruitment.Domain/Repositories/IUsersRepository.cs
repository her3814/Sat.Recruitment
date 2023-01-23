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
        void CreateUser(User newUser);
        IEnumerable<User> GetUsers();

    }
}
