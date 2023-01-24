using Microsoft.Extensions.DependencyInjection;
using Sat.Recruitment.Application.services;
using Sat.Recruitment.Domain.Repositories;
using Sat.Recruitment.FileRepository;

namespace Sat.Recruitment.Api.Configuration
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUsersRepository, FileUsersRepository>();
            return services;
        }
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<UserService>();
            return services;
        }
    }
}
