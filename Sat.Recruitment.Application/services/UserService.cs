using Newtonsoft.Json;
using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Domain.Repositories;
using Sat.Recruitment.Helpers;
using System.ComponentModel.DataAnnotations;
using Serilog;

namespace Sat.Recruitment.Application.services
{
    public class UserService
    {


        private readonly IUsersRepository _usersRepository;
        private readonly ILogger _logger;
        public UserService(ILogger logger, IUsersRepository usersRepository)
        {
            _logger = logger;
            _usersRepository = usersRepository;
        }


        public async Task<AppResult> AddUser(User newUser)
        {
            var result = new AppResult();

            //Validate user info
            var validationResults = newUser.Validate();

            if (validationResults.Any())
            {
                result.AddInputDataErrors(validationResults);
                return result;
            }

            //Normalize email
            newUser.Email = newUser.Email.NormalizeEmail();

            var users = await _usersRepository.GetUsers();

            if (users.Any(user => user.Equals(newUser)))
            {
                _logger.Warning("WARN: Creating new user");
                _logger.Warning("User is duplicated");
                _logger.Warning(JsonConvert.SerializeObject(newUser));

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
                _logger.Error("ERROR Listing All Users");
                _logger.Error(ex, ex.ToString());
            }

            return result;
        }


    }
}
