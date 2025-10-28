using POS.Backend.Infrastructure.BusinessLogic.Implementation;
using POS.Backend.Infrastructure.BusinessLogic.Interfaces;
using POS.Backend.Infrastructure.DataLogic.Database.Implementation;
using POS.Backend.Infrastructure.DataLogic.Database.Interfaces;
using POS.Backend.Infrastructure.DataLogic.Service.Implementation;
using POS.Backend.Infrastructure.DataLogic.Service.Interfaces;

namespace POS.Backend.Infrastructure.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Business Logic Service Collection
            #region Logic Scoped Region
            services.AddScoped<IPOSLogic, POSLogic>();
            services.AddScoped<IAuthLogic, AuthLogic>();
            #endregion

            // Data Access Layer Service Collection
            #region DAL(Database) Scoped Region
            services.AddScoped<IPOSDb, POSDb>();
            services.AddScoped<IAuthDb, AuthDb>();
            #endregion

            // Data Access Layer Service Collection
            #region DAL(Services) Scoped Region
            services.AddScoped<IPOSService, POSService>();
            #endregion

            // Register other infrastructure services (logging, caching, etc.)
        }
    }
}
