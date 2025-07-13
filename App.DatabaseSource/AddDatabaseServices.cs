using Azure.Core;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DatabaseSource;
public static class AddDatabaseServices
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config, TokenCredential credential)
    {
        if (config["UseLocalDatabase"] == "True")
        {
            var connectionString = config.GetConnectionString("LocalConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }

        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = config.GetConnectionString("DefaultConnection");
            var token = credential.GetToken(new TokenRequestContext(new[] { "https://database.windows.net/.default" }), default);
            options.UseSqlServer(connectionString);
        });
        return services;
    }
}
