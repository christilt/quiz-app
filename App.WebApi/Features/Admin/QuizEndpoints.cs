using App.DatabaseSource;
using App.DatabaseSource.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.WebApi.Features.Admin;

public static class QuizEndpoints
{
    public static IEndpointRouteBuilder MapQuizEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/quizes")
            .WithOpenApi()
            ;

        group.MapGet("/", (QuizEndpointHandler handler, CancellationToken cancellation) => 
            handler.GetQuizes(cancellation));

        group.MapGet("/{guid}", (Guid guid, QuizEndpointHandler handler, CancellationToken cancellation) => 
            handler.GetQuiz(guid, cancellation));

        group.MapPost("/", (QuizCreationOptions options, QuizEndpointHandler handler, CancellationToken cancellation) =>
            handler.CreateQuiz(options, cancellation));

        return builder;
    }
}

public class QuizEndpointHandler
{
    private readonly ILogger<QuizEndpointHandler> _logger;
    private readonly AppDbContext _appDbContext;

    public QuizEndpointHandler(ILogger<QuizEndpointHandler> logger, AppDbContext appDbContext)
    {
        _logger = logger;
        _appDbContext = appDbContext;
    }

    public async Task<IResult> GetQuizes(CancellationToken cancellation)
    {
        var quizes = await _appDbContext.Quizes.ToListAsync(cancellation);
        return Results.Ok(quizes);
    }

    public async Task<IResult> GetQuiz(Guid guid, CancellationToken cancellation)
    {
        var quiz = await _appDbContext.Quizes.SingleOrDefaultAsync(q => q.Guid == guid, cancellation);
        if (quiz is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(quiz);
    }

    public async Task<IResult> CreateQuiz(QuizCreationOptions options, CancellationToken cancellation)
    {
        var quiz = new Quiz();
        await _appDbContext.Quizes.AddAsync(quiz);
        await _appDbContext.SaveChangesAsync();

        return Results.Created($"api/quizes/{quiz.Guid}", quiz);
    }
}

public class QuizCreationOptions
{
}