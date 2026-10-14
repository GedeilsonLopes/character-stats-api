namespace CharacterStats.Tests
{
    using CharacterStats.Models;
    using CharacterStats.Services;

    public class CharacterStatsServiceTests
    {
        [Fact]
        public void PerformAttack_AttackerDead_ShouldThrowInvalidOperationException()
        {
            Character attacker = new(1,"Attacker", 100, 1, 1);
            Character defender = new(2, "Defender", 100, 1, 1);

            attacker.TakeDamage(100);
            Assert.Throws<InvalidOperationException>(() =>
                CharacterStatsService.PerformAttack(attacker, defender)
            );
        }

        [Fact]
        public void PerformAttack_DefenderDead_ShouldThrowInvalidOperationException()
        {
            Character attacker = new(1, "Attacker", 100, 1, 1);
            Character defender = new(2, "Defender", 100, 1, 1);

            defender.TakeDamage(100);
            Assert.Throws<InvalidOperationException>(() =>
                CharacterStatsService.PerformAttack(attacker, defender)
            );
        }

        public static readonly TheoryData<int, int, int, int, int> PerformAttackValidCases = new()
        {
            { 50, 20, 100, 30, 70 },
            { 50, 50, 100, 0, 100 },
            { 50, 80, 100, 0, 100 },
        };

        [Theory]
        [MemberData(nameof(PerformAttackValidCases))]
        public static void PerformAttack_AliveCharacters_ShouldReturnDamageAndUpdateDefenderHealth(
            int attackPower,
            int defensePower,
            int defenderHealth,
            int expectedDamage,
            int expectedRemainingHealth
        )
        {
            Character attacker = new(1, "Attacker", 100, attackPower, 1);
            Character defender = new(2, "Defender", defenderHealth, 1, defensePower);

            int damage = CharacterStatsService.PerformAttack(attacker, defender);

            Assert.Multiple(
                () => Assert.Equal(expectedDamage, damage),
                () => Assert.Equal(expectedRemainingHealth, defender.CurrentHealth)
            );
        }
    }
}
