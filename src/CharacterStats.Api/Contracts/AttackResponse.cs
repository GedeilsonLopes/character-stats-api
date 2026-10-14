namespace CharacterStats.Api
{
    public record AttackResponse(
        int AttackerId,
        int DefenderId,
        int Damage,
        int RemainingHealth,
        bool DefenderIsAlive
    );
}
