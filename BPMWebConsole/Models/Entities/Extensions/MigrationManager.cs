using Lib.Log;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace BPMWebConsole.Models.Entities.Extensions
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost webHost)
        {
            using (IServiceScope scope = webHost.Services.CreateScope())
            {
                using ApplicationContext appContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                try
                {
                    appContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    ErrorLog.Log("MigrationManager", ex);
                    throw;
                }
            }

            return webHost;
        }
    }
}
