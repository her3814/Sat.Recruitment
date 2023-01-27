using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Test.Fixtures;
using System.Linq;
using Xunit;


namespace Sat.Recruitment.Test.Tests
{
    [Collection("FileRepository Collection")]
    public class UsersControllerTests : IClassFixture<InjectionWithFileRepositoryFixture>
    {
        private readonly InjectionWithFileRepositoryFixture injection;

        public UsersControllerTests(InjectionWithFileRepositoryFixture injection)
        {
            this.injection = injection;
        }

        [Fact(DisplayName = "Adds a new user that doesn't exists on the users file, and checks that it's been saved to the data file.")]
        public async void AddsNewUser()
        {
            var userController = new UsersController(injection.usersService);

            var userMike = new User()
            {
                Name = "Mike",
                Email = "mike@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = UserTypeEnum.Normal,
                Money = 124
            };
            var result = await userController.AddUser(userMike);


            Assert.True(result.IsSuccess, "Error adding new user");
            Assert.Empty(result.Errors);

            var resultUsersList = await userController.GetAllUsers();

            Assert.True(resultUsersList.IsSuccess, "Error listing users");
            Assert.Empty(resultUsersList.Errors);
            Assert.Contains(userMike, resultUsersList.Data);
        }

        [Fact(DisplayName = "Tries to add a duplicated user, which mustn't be added.")]
        public async void DoesntAddDuplicatedUsers()
        {
            var userController = new UsersController(injection.usersService);


            var userAgustina = new User()
            {
                Name = "Agustina",
                Email = "Agustina@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354215",
                UserType = UserTypeEnum.Normal,
                Money = 124
            };
            var result = await userController.AddUser(userAgustina);


            Assert.False(result.IsSuccess);
            Assert.Contains("User is duplicated", result.ErrorsText);
        }

        [Fact(DisplayName = "Checks every required parameter on the user DTO")]
        public async void DetectsWrongParameters()
        {
            var userController = new UsersController(injection.usersService);


            var userAgustina = new User()
            {
                UserType = UserTypeEnum.Normal,
                Money = 124
            };
            var result = await userController.AddUser(userAgustina);


            Assert.False(result.IsSuccess);
            Assert.Contains("The name is required", result.ErrorsText);
            Assert.Contains("The email is required", result.ErrorsText);
            Assert.Contains("The address is required", result.ErrorsText);
            Assert.Contains("The phone is required", result.ErrorsText);
        }

        [Fact(DisplayName = "Adds no Gift to new users with money under floor")]
        public async void AddsNoGiftToNewUsers()
        {
            var userController = new UsersController(injection.usersService);

            var userTestA = new User()
            {
                Name = "Agustina1",
                Email = "Agustina1@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354211",
                UserType = UserTypeEnum.Normal,
                Money = 10
            };
            var userTestB = new User()
            {
                Name = "Agustina2",
                Email = "Agustina2@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354212",
                UserType = UserTypeEnum.Premium,
                Money = 100
            };
            var userTestC = new User()
            {
                Name = "Agustina3",
                Email = "Agustina3@gmail.com",
                Address = "Av. Juan G",
                Phone = "+349 1122354213",
                UserType = UserTypeEnum.SuperUser,
                Money = 100
            };
            await userController.AddUser(userTestA);
            await userController.AddUser(userTestB);
            await userController.AddUser(userTestC);
            var users = await userController.GetAllUsers();
            var newUserA = users.Data.First(u => u.Email == userTestA.Email);
            Assert.Equal(userTestA.Money, newUserA.Money);
            var newUserB = users.Data.First(u => u.Email == userTestA.Email);
            Assert.Equal(userTestA.Money, newUserB.Money);
            var newUserC = users.Data.First(u => u.Email == userTestA.Email);
            Assert.Equal(userTestA.Money, newUserC.Money);
        }

        [Fact(DisplayName = "Adds  Gift to new users given it's user type")]
        public async void AddsGiftsToNewUsers()
        {
            var userController = new UsersController(injection.usersService);

            var userTestA = new User()
            {
                Name = "Agustina1",
                Email = "Agustina1@gmail.com",
                Address = "Av. Juan G 1",
                Phone = "+349 1122354211",
                UserType = UserTypeEnum.Normal,
                Money = 11
            };
            var userTestB = new User()
            {
                Name = "Agustina2",
                Email = "Agustina2@gmail.com",
                Address = "Av. Juan G 2",
                Phone = "+349 1122354212",
                UserType = UserTypeEnum.Normal,
                Money = 101
            };
            var userTestC = new User()
            {
                Name = "Agustina3",
                Email = "Agustina3@gmail.com",
                Address = "Av. Juan G 3",
                Phone = "+349 1122354213",
                UserType = UserTypeEnum.Premium,
                Money = 101
            };
            var userTestD = new User()
            {
                Name = "Agustina4",
                Email = "Agustina4@gmail.com",
                Address = "Av. Juan G 4",
                Phone = "+349 1122354214",
                UserType = UserTypeEnum.SuperUser,
                Money = 101
            };

            await userController.AddUser(userTestA);
            await userController.AddUser(userTestB);
            await userController.AddUser(userTestC);
            await userController.AddUser(userTestD);

            Assert.Equal(11 * (1 + NewUserGiftPercentages.NormalOver10), userTestA.Money);
            Assert.Equal(101 * (1 + NewUserGiftPercentages.NormalOver100), userTestB.Money);
            Assert.Equal(101 * (1 + NewUserGiftPercentages.PremiumUser), userTestC.Money);
            Assert.Equal(101 * (1 + NewUserGiftPercentages.SuperUser), userTestD.Money);
        }

    }
}
