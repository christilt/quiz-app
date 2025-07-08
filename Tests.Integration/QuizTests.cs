using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Tests.Integration.Setup;
using Xunit;

namespace Tests.Integration;

public class QuizTests : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _api;

    public QuizTests(TestApplicationFactory factory)
    {
        _api = factory.CreateClient();
    }

    [Fact]
    public async Task GetQuizes_ShouldReturnEmptyList_WhenThereAreNoQuizes()
    {
        var response = await _api.GetAsync($"api/quizes");

        response.IsSuccessStatusCode.ShouldBeTrue();
        var items = await response.Content.ReadFromJsonAsync<IEnumerable<object>>();
        items.ShouldNotBeNull();
        items.ShouldBeEmpty();
    }
}
