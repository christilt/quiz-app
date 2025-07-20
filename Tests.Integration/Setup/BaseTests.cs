using App.DatabaseSource;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Integration.Setup;
public class BaseTests : IClassFixture<TestApplicationFactory>
{
    protected readonly HttpClient _api;
    protected readonly AppDbContext _database;

    public BaseTests(TestApplicationFactory factory)
    {
        _api = factory.CreateClient();
        _database = factory.Services.GetRequiredService<AppDbContext>();    
    }
}
