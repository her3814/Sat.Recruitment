using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Domain.Repositories;

namespace Sat.Recruitment.FileRepository
{
    public class FileUsersRepository : IUsersRepository
    {
        public void CreateUser(User newUser)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsers()
        {
            var usersList = new List<User>();

            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";

            FileStream fileStream = new FileStream(path, FileMode.Open);

            StreamReader reader = new StreamReader(fileStream);

            return usersList;
        }
    }
}