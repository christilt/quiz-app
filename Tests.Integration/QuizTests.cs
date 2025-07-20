using Microsoft.EntityFrameworkCore;
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

public class QuizTests : BaseTests
{
    public QuizTests(TestApplicationFactory factory) : base(factory)
    {
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
        var getResponse = await _api.GetAsync(createResponse.Headers.Location);
        var quizFromApi = await getResponse.Content.ReadFromJsonAsync<Quiz>();
        getResponse.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
        quizFromApi.ShouldNotBeNull();
        var quizInDb = await _database.Quizes.AsNoTracking()
            .SingleOrDefaultAsync(q => q.Guid == quizFromApi.Guid);
        quizInDb.ShouldNotBeNull();
    }
}
