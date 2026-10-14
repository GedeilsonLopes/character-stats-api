using System.Net;
using System.Net.Http.Json;
using CharacterStats.Api;
using CharacterStats.Api.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CharacterStats.Tests;

public class CharacterStatsApiTests : IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory = new();
    private HttpClient _client = null!;

    public Task InitializeAsync()
    {
        _client = _factory.CreateClient();
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
    }

    [Fact]
    public async Task GetCharacter_ExistingId_ShouldReturnOkWithCharacter()
    {
        HttpResponseMessage response = await _client.GetAsync("/characters/1");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        CharacterResponse? character =
            await response.Content.ReadFromJsonAsync<CharacterResponse>();

        Assert.NotNull(character);
        Assert.Multiple(
            () => Assert.Equal(1, character.Id),
            () => Assert.Equal("Chewie", character.Name),
            () => Assert.Equal(100, character.CurrentHealth),
            () => Assert.True(character.IsAlive)
        );
    }

    [Fact]
    public async Task GetCharacter_NonExistingId_ShouldReturnNotFound()
    {
        HttpResponseMessage response = await _client.GetAsync("/characters/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PerformAttack_WithPositiveDamage_ShouldReturnOkAndAttackResult()
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/characters/2/attack",
            new { DefenderId = 1 }
        );
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        AttackResponse? attackResult = await response.Content.ReadFromJsonAsync<AttackResponse>();

        Assert.NotNull(attackResult);
        Assert.Multiple(
            () => Assert.Equal(2, attackResult.AttackerId),
            () => Assert.Equal(1, attackResult.DefenderId),
            () => Assert.Equal(25, attackResult.Damage),
            () => Assert.Equal(75, attackResult.RemainingHealth),
            () => Assert.True(attackResult.DefenderIsAlive)
        );
    }

    [Fact]
    public async Task PerformAttack_InsufficientAttackPower_ShouldNotDamageDefender()
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/characters/1/attack",
            new { DefenderId = 2 }
        );
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        AttackResponse? attackResult = await response.Content.ReadFromJsonAsync<AttackResponse>();

        Assert.NotNull(attackResult);
        Assert.Multiple(
            () => Assert.Equal(1, attackResult.AttackerId),
            () => Assert.Equal(2, attackResult.DefenderId),
            () => Assert.Equal(0, attackResult.Damage),
            () => Assert.Equal(100, attackResult.RemainingHealth),
            () => Assert.True(attackResult.DefenderIsAlive)
        );
    }

    [Fact]
    public async Task PerformAttack_NonExistingAttackerId_ShouldReturnNotFound()
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/characters/999/attack",
            new { DefenderId = 1 }
        );
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PerformAttack_NonExistingDefenderId_ShouldReturnNotFound()
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(
            "/characters/1/attack",
            new { DefenderId = 999 }
        );
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

public record CharacterResponse(
    int Id,
    string Name,
    int MaxHealth,
    int CurrentHealth,
    int AttackPower,
    int DefensePower,
    bool IsAlive
);
