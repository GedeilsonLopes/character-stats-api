namespace CharacterStats.Models
{
    public class Character
    {
        private static int _nextId = 1;

        public int Id { get; }
        public string Name { get; }
        public int MaxHealth { get; }
        public int CurrentHealth { get; private set; }
        public int AttackPower { get; }
        public int DefensePower { get; }

        public bool IsAlive => CurrentHealth > 0;

        public Character(string name, int maxHealth, int attackPower, int defensePower)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxHealth);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(attackPower);
            ArgumentOutOfRangeException.ThrowIfNegative(defensePower);

            Id = _nextId++;
            Name = name;
            MaxHealth = maxHealth;
            CurrentHealth = MaxHealth;
            AttackPower = attackPower;
            DefensePower = defensePower;
        }

        public void TakeDamage(int amount)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(amount);
            CurrentHealth = Math.Max(CurrentHealth - amount, 0);
        }

        public void Heal(int amount)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(amount);
            CurrentHealth = Math.Min(CurrentHealth + amount, MaxHealth);
        }
    }
}
