using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Domain.Repositories;
using Sat.Recruitment.Helpers;

namespace Sat.Recruitment.Application.services
{
    public class UserService
    {


        private readonly IUsersRepository _usersRepository;
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger, IUsersRepository usersRepository)
        {
            _logger = logger;
            _usersRepository = usersRepository;
        }


        public async Task<AppResult> AddUser(User newUser)
        {
            var result = new AppResult();

            //Validate user info
            ValidateErrors(newUser, result);
            if (!result.IsSuccess)
            {
                return result;
            }

            //Normalize email
            newUser.Email = newUser.Email.NormalizeEmail();

            var users = await _usersRepository.GetUsers();

            if (users.Any(user => user.Equals(newUser)))
            {
                _logger.LogWarning("WARN: Creating new user");
                _logger.LogWarning("User is duplicated");
                _logger.LogWarning(JsonConvert.SerializeObject(newUser));

                result.AddBusinessError("User is duplicated");
                return result;
            }


            //If everything is OK add Gift if corresponds
            decimal giftPercentage = NewUserGiftPercentages.GetNewUserGiftPercentage(newUser);
            if (giftPercentage > 0)
            {
                decimal gift = newUser.Money * giftPercentage;
                newUser.Money += gift;
            }

            await _usersRepository.AddUser(newUser);

            return result;
        }

        public async Task<AppResult<List<User>>> ListAllUsers()
        {
            AppResult<List<User>> result = new AppResult<List<User>>();
            try
            {
                result.Data = (await _usersRepository.GetUsers()).ToList();
            }
            catch (Exception ex)
            {
                result.AddInternalError(ex.Message);
                _logger.LogError("ERROR Listing All Users");
                _logger.LogError(ex, ex.ToString());
            }

            return result;
        }


        //Validate errors
        private void ValidateErrors(User userToValidate, AppResult result)
        {
            if (string.IsNullOrEmpty(userToValidate.Name))
                //Validate if Name is null
                result.AddInputDataError("The name is required");
            if (string.IsNullOrEmpty(userToValidate.Email))
                //Validate if Email is null
                result.AddInputDataError("The email is required");
            else if (!userToValidate.Email.IsValidEmail())
                //Validate if Email is valid
                result.AddInputDataError("The email is invalid");
            if (string.IsNullOrEmpty(userToValidate.Address))
                //Validate if Address is null
                result.AddInputDataError("The address is required");
            if (string.IsNullOrEmpty(userToValidate.Phone))
                //Validate if Phone is null
                result.AddInputDataError("The phone is required");
        }
    }
}
