namespace CharacterStats.Api.Contracts.Requests
{
    public record CreateCharacterRequest
    (
        string Name,
        int MaxHealth,
        int AttackPower,
        int DefensePower
    );
}
