using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Application.services;
using Sat.Recruitment.Domain.Repositories;
using Sat.Recruitment.Test.Helpers;
using Serilog;
using System;
using System.Net.Http;
using Xunit;

namespace Sat.Recruitment.Test.Fixtures
{
    public class InjectionWithFileRepositoryFixture : IDisposable
    {
        private readonly TestServer server;
        private readonly HttpClient client;
        public IServiceProvider ServiceProvider => server.Host.Services;

        public UserService usersService;
        public UsersController usersController;
        public IUsersRepository usersRepository;
        public ILogger logger;
        public InjectionWithFileRepositoryFixture()
        {
            TestFileHelper.ResetTestFile();
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            client = server.CreateClient();

            usersRepository = (IUsersRepository)ServiceProvider.GetService(typeof(IUsersRepository));
            usersService = (UserService)ServiceProvider.GetService(typeof(UserService));
            usersController = new UsersController(usersService);
            logger = (ILogger)ServiceProvider.GetService(typeof(ILogger));

        }


        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                server.Dispose();
                client.Dispose();

            }
        }

        [CollectionDefinition("FileRepository Collection", DisableParallelization = true)]
        public class FileRepositoryCollection : ICollectionFixture<InjectionWithFileRepositoryFixture>
        {
            // This class has no code, and is never created. Its purpose is simply
            // to be the place to apply [CollectionDefinition] and all the
            // ICollectionFixture<> interfaces.
        }
    }
}
