using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Domain.Repositories;

namespace Sat.Recruitment.FileRepository
{
    public class FileUsersRepository : IUsersRepository
    {
        private readonly string usersFilePath;
        private readonly char columnSeparator = ';';
        public FileUsersRepository()
        {
            usersFilePath = Directory.GetCurrentDirectory() + "/Files/Users.txt";
        }
        public async Task AddUser(User newUser)
        {
            var userAsText = string.Join(columnSeparator, newUser.Name, newUser.Email, newUser.Phone, newUser.Address, newUser.UserType, newUser.Money);
            List<string> users = (await File.ReadAllLinesAsync(usersFilePath)).ToList();
            users= users.Append(userAsText).ToList();

            //await File.WriteAllLinesAsync(usersFilePath, users);
            await File.WriteAllTextAsync(usersFilePath, string.Join(Environment.NewLine, users));
            return;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var usersList = new List<User>();

            string[] usersOnFile = await File.ReadAllLinesAsync(usersFilePath);

            foreach (string userOnFile in usersOnFile)
            {
                var splittedLine = userOnFile.Split(columnSeparator);
                var user = new User
                {
                    Name = splittedLine[0].ToString(),
                    Email = splittedLine[1].ToString(),
                    Phone = splittedLine[2].ToString(),
                    Address = splittedLine[3].ToString(),
                    UserType = Enum.Parse<UserTypeEnum>(splittedLine[4].ToString()),
                    Money = decimal.Parse(splittedLine[5].ToString()),
                };
                usersList.Add(user);
            }
            return usersList;
        }
    }
}