using CharacterStats.Api.Contracts.Requests;
using CharacterStats.Api.Contracts.Responses;
using CharacterStats.Models;
using CharacterStats.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

List<Character> characters = [];

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

app.MapPost(
    "/characters/create", 
    (CreateCharacterRequest request) =>
    {
        characters.Add(new(request.Name, request.MaxHealth, request.AttackPower, request.DefensePower));
    }

);

app.Run();
