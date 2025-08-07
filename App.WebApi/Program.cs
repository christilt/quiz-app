using App.DatabaseSource;
using App.WebApi.Features.Admin;
using Azure.Core;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace App.WebApi;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var authority = builder.Configuration["Auth0:Authority"];
        var audience = builder.Configuration["Auth0:Audience"];

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = authority;
            options.Audience = audience;
        });
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "App.WebApi", Version = "v1" });

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        Scopes = new Dictionary<string, string>
                            {
                                { "openid", "Open Id" }
                            },
                        AuthorizationUrl = new Uri(authority + "authorize?audience=" + audience)
                    }
                }
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="oauth2"
                            }
                        },
                        new string[]{}
                    }
                });
        });
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
        builder.Services.AddWebApiServices();


        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "App.WebApi v1");
            c.OAuthClientId(builder.Configuration["Auth0:ClientId"]);
        });

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapQuizEndpoints();

        app.Run();
    }
}
