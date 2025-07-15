using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
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
public static class DependencyInjection
{
    public static IServiceCollection AddDatabaseSourceServices(this IServiceCollection services, IConfiguration config, TokenCredential credential)
    {
        if (config["UseLocalDatabase"] == "True")
        {
            var connectionString = config.GetConnectionString("Local");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }

        services.AddDbContext<AppDbContext>(options =>
        {
            var token = credential.GetToken(new TokenRequestContext(new[] { "https://database.windows.net/.default" }), default);
            var connectionString = config.GetConnectionString("DefaultConnection");
            var connection = new SqlConnection(connectionString);
            connection.AccessToken = token.Token;
            options.UseSqlServer(connection);
        });
        return services;
    }
}
