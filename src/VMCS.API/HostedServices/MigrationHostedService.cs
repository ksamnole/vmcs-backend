using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VMCS.Data.Contexts;

namespace VMCS.API.HostedServices
{
    public class MigrationHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public MigrationHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                DatabaseUpdate(scope.ServiceProvider.GetService<ApplicationContext>());
                DatabaseUpdate(scope.ServiceProvider.GetService<AuthenticationContext>());

                return Task.CompletedTask;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private static void DatabaseUpdate(DbContext context)
        {
            if (context == null)
                throw new Exception($"{nameof(ApplicationContext)} not registered");

            context.Database.Migrate();
        }
    }
}
