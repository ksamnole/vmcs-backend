﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCS.API;
using VMCS.API.HostedServices;

namespace VMCS.IntegrationTests
{
    public class IntegrationTestBase 
    {
        protected readonly CustomWebApplicationFactory<Startup> _webApplicationFactory;
        public IntegrationTestBase() 
        {
            _webApplicationFactory = new CustomWebApplicationFactory<Startup>();
        }
    }

    public class CustomWebApplicationFactory<T> :WebApplicationFactory<T>
        where T : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var migrationService = services.FirstOrDefault(s => s.ImplementationType == typeof(MigrationHostedService));
                services.Remove(migrationService);
            });

            
        }
    }
}
