using ContactsManagement.Core.Interfaces;
using ContactsManagement.Core.Services;

namespace ContatcsManagement.Configurations
{
    public static class RegisterServices
    {
        public static IServiceCollection ServiceCollection(this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            // Register Services Dependency Injection
            services.AddScoped<IContactsManagementService, ContactsManagementService>();

            return services;
        }
    }
}
