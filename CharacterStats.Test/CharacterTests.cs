namespace CharacterStats.Tests
{
    using CharacterStats.Models;

    public class CharacterTests
    {
        [Fact]
        public void NewCharacter_ShouldHaveCurrentHealthEqualMaxHealth()
        {
            Character character = new(1, "Chewie", 100, 1, 0);

            Assert.Equal(character.MaxHealth, character.CurrentHealth);
        }

        public static readonly TheoryData<int, int> TakeDamageValidCases = new()
        {
            { 0, 100 },
            { 1, 99 },
            { 100, 0 },
            { 1000, 0 },
        };

        [Theory]
        [MemberData(nameof(TakeDamageValidCases))]
        public void TakeDamage_ValidAmounts_ShouldReduceHealthWithoutGoingBelowZero(
            int amount,
            int expectedRemainingLife
        )
        {
            Character character = new(1, "Chewie", 100, 1, 0);
            character.TakeDamage(amount);

            Assert.Equal(expectedRemainingLife, character.CurrentHealth);
        }

        public static TheoryData<int> TakeDamageInvalidCases => [-1, -100, -1000];

        [Theory]
        [MemberData(nameof(TakeDamageInvalidCases))]
        public void TakeDamage_InvalidAmounts_ShouldThrowArgumentOutOfRangeException(int amount)
        {
            Character character = new(1, "Chewie", 100, 1, 0);

            Assert.Throws<ArgumentOutOfRangeException>(() => character.TakeDamage(amount));
        }

        public static TheoryData<int> NewCharacterInvalidId => [0, -1];

        [Theory]
        [MemberData(nameof(NewCharacterInvalidId))]
        public void NewCharacter_InvalidId_ShouldThrowArgumentOutOfRangeException(int id)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Character(id, "Chewie", 100, 1, 0));
        }

        public static TheoryData<string> NewCharacterInvalidName => ["", "  ", "    "];

        [Theory]
        [MemberData(nameof(NewCharacterInvalidName))]
        public void NewCharacter_InvalidName_ShouldThrowArgumentException(string name)
        {
            Assert.Throws<ArgumentException>(() => new Character(1, name, 100, 1, 0));
        }

        public static TheoryData<int> NewCharacterInvalidMaxHealth => [0, -1, -100];

        [Theory]
        [MemberData(nameof(NewCharacterInvalidMaxHealth))]
        public void NewCharacter_InvalidMaxHealth_ShouldThrowArgumentOutOfRangeException(
            int maxHealth
        )
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Character(1, "Chewie", maxHealth, 0, 0)
            );
        }

        public static TheoryData<int, int, int> TakeDamageAndHealValidCases =>
            new()
            {
                { 30, 20, 90 },
                { 30, 70, 100 },
                { 100, 50, 50 },
                { 0, 0, 100 },
            };

        [Theory]
        [MemberData(nameof(TakeDamageAndHealValidCases))]
        public void Character_TakeDamageAndHealing_ShouldRegenerateHealth(
            int damageAmount,
            int healingAmount,
            int expectedCurrentHealth
        )
        {
            Character character = new(1, "Chewie", 100, 1, 0);

            character.TakeDamage(damageAmount);
            character.Heal(healingAmount);

            Assert.Equal(expectedCurrentHealth, character.CurrentHealth);
        }

        public static TheoryData<int, int> TakeDamageAndHealInvalidCases =>
            new()
            {
                { 30, -10 },
                { 30, -70 },
                { 100, -50 },
                { 0, -1 },
            };

        [Theory]
        [MemberData(nameof(TakeDamageAndHealInvalidCases))]
        public void Character_TakeDamageAndHealing_ShouldThrowArgumentOutOfRangeException(
            int damageAmount,
            int healingAmount
        )
        {
            Character character = new(1, "Chewie", 100, 1, 0);

            character.TakeDamage(damageAmount);

            Assert.Throws<ArgumentOutOfRangeException>(() => character.Heal(healingAmount));
        }

        public static readonly TheoryData<int, bool> TakeDamageAndCheckIfStillAliveCases = new()
        {
            { 0, true },
            { 50, true },
            { 100, false },
            { 150, false },
        };

        [Theory]
        [MemberData(nameof(TakeDamageAndCheckIfStillAliveCases))]
        public void TakeDamage_ShouldUpdateAliveState(int damageAmount, bool isAlive)
        {
            Character character = new(1, "Chewie", 100, 1, 0);

            character.TakeDamage(damageAmount);

            Assert.Equal(isAlive, character.IsAlive);
        }
    }
}
