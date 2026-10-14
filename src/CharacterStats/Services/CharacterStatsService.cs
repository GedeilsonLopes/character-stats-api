using CharacterStats.Models;

namespace CharacterStats.Services
{
    public class CharacterStatsService
    {
        public static int PerformAttack(Character attacker, Character defender)
        {
            ArgumentNullException.ThrowIfNull(attacker);
            VerifyCharacterIsAlive(attacker, "Attacker");

            ArgumentNullException.ThrowIfNull(defender);
            VerifyCharacterIsAlive(defender, "Defender");

            int finalDamage = CalculateDamage(attacker, defender);

            defender.TakeDamage(finalDamage);

            return finalDamage;
        }

        private static int CalculateDamage(Character attacker, Character defender)
        {
            int rawDamage = attacker.AttackPower - defender.DefensePower;
            int finalDamage = Math.Max(rawDamage, 0);
            return finalDamage;
        }

        private static void VerifyCharacterIsAlive(Character character, string characterRole)
        {
            if (!character.IsAlive)
            {
                throw new InvalidOperationException($"{characterRole} is dead");
            }
        }
    }
}
