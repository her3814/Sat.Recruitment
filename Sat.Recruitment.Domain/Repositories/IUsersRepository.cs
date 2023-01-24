using Sat.Recruitment.Domain.Models;

namespace Sat.Recruitment.Domain.Repositories
{
    public interface IUsersRepository
    {
        Task AddUser(User newUser);
        Task<IEnumerable<User>> GetUsers();
        

    }
}
