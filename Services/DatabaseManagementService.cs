using BancoParaleloAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BancoParaleloAPI.Services
{
    public static class DatabaseManagementService
    {
        public static Task MigrationInitialisation(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var serviceDb = serviceScope.ServiceProvider.GetService<AppDbContext>();
                serviceDb.Database.MigrateAsync();
                return Task.CompletedTask;
            }
        }
    }
}
