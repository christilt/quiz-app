using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Tests.Integration.ModelExpected;
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

    [Fact]
    public async Task CreateQuiz_ShouldCreateQuiz()
    {
        var createResponse = await _api.PostAsync($"api/quizes", new StringContent("""
            {
                
            }
            """, Encoding.UTF8, "application/json"));

        createResponse.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);
        createResponse.Headers.Location.ShouldNotBeNull();
        // TODO: Test exists in database

        var getResponse = await _api.GetAsync(createResponse.Headers.Location);
        var quizFromApi = await getResponse.Content.ReadFromJsonAsync<Quiz>();
        getResponse.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        quizFromApi.ShouldNotBeNull();
    }
}
