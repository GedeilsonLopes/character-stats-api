namespace CharacterStats.Api.Contracts.Responses
{
    public record CharacterResponse(
        int Id,
        string Name,
        int MaxHealth,
        int CurrentHealth,
        int AttackPower,
        int DefensePower,
        bool IsAlive
    );
}
