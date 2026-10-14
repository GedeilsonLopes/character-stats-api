using CharacterStats.Api;
using CharacterStats.Api.Contracts;
using CharacterStats.Models;
using CharacterStats.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var characters = new List<Character>
{
    new(1, "Chewie", 100, 20, 15),
    new(2, "Aragorn", 100, 40, 30),
    new(3, "Link", 100, 30, 10),
};

app.MapGet("/characters", () => characters);

app.MapGet(
    "/characters/{id}",
    (int id) =>
    {
        var character = characters.FirstOrDefault(c => c.Id == id);
        return character is null ? Results.NotFound() : Results.Ok(character);
    }
);
app.MapPost(
    "/characters/{attackerId}/attack",
    (int attackerId, AttackRequest request) =>
    {
        Character? attacker = characters.FirstOrDefault(c => c.Id == attackerId);
        if (attacker is null)
        {
            return Results.NotFound($"Character {attackerId} not found.");
        }

        Character? defender = characters.FirstOrDefault(c => c.Id == request.DefenderId);
        if (defender is null)
        {
            return Results.NotFound($"Character {request.DefenderId} not found.");
        }

        try
        {
            int damage = CharacterStatsService.PerformAttack(attacker, defender);

            return Results.Ok(
                new AttackResponse(
                    attacker.Id,
                    defender.Id,
                    damage,
                    defender.CurrentHealth,
                    defender.IsAlive
                )
            );
        }
        catch (InvalidOperationException e)
        {
            return Results.Conflict(e.Message);
        }
    }
);

app.Run();
