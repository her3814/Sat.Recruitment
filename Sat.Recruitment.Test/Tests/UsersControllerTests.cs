using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Test.Fixtures;
using Xunit;


namespace Sat.Recruitment.Test.Tests
{
    [CollectionDefinition("FileRepository Collection")]
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
                UserType = "Normal",
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
                UserType = "Normal",
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
                UserType = "Normal",
                Money = 124
            };
            var result = await userController.AddUser(userAgustina);


            Assert.False(result.IsSuccess);
            Assert.Contains("The name is required", result.ErrorsText);
            Assert.Contains("The email is required", result.ErrorsText);
            Assert.Contains("The address is required", result.ErrorsText);
            Assert.Contains("The phone is required", result.ErrorsText);
        }

    }
}
