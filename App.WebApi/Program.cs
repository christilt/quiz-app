using App.DatabaseSource;
using Azure.Core;
using Azure.Identity;

namespace App.WebApi;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var credential = new DefaultAzureCredential();
        builder.Services.AddSingleton<TokenCredential>(credential);
        // TODO:
        //var keyVaultSecretManager = new HierarchicalKeyVaultSecretManager();
        //builder.Services.AddSingleton(keyVaultSecretManager);
        //builder.Configuration.AddAzureKeyVault(
        //    new Uri(builder.Configuration["KeyVault:VaultUri"]!),
        //    credential,
        //    keyVaultSecretManager);

        builder.Configuration.AddAzureKeyVault(
            new Uri(builder.Configuration["KeyVault:VaultUri"]!),
            credential);
        builder.Services.AddDatabaseSourceServices(builder.Configuration, credential);
        builder.Services.AddDatabaseSourceServices(builder.Configuration, credential);


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapGet("/api/quizes", () =>
        {
            return new object[] { };
        })
        .WithName("GetQuizes")
        .WithOpenApi();

        app.Run();
    }
}
