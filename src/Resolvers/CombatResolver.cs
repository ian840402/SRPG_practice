using Godot;
using SRPGPractice.Core;

namespace SRPGPractice.Resolvers;

public sealed record AttackResult(
  bool IsHit,
  double HitRate,
  bool IsCritical,
  int Damage
);

public static class CombatResolver
{
  private const double MaxHitCoefficient = 95;
  private const double CriticalCoefficient = 1.5;

  public static AttackResult ResolveNormalAttack(Unit attacker, Unit target)
  {
    var hitRate = Mathf.Min(Mathf.Max(attacker.HitCoefficient - target.Evasion, 0), MaxHitCoefficient);
    var isHit = RollSuccess(hitRate);
    var isCritical = isHit && RollSuccess(attacker.CriticalRate);
    var baseDamage = Mathf.Max(attacker.AttackPower - target.Defense, 1);
    var criticalDamage = isCritical ? Mathf.CeilToInt(baseDamage * CriticalCoefficient) : baseDamage;
    var finalDamage = isHit ? criticalDamage : 0;

    return new AttackResult(
      isHit,
      hitRate,
      isCritical,
      finalDamage
    );
  }

  private static bool RollSuccess(double rate)
  {
    var roll = GD.RandRange(0, 100);
    return roll <= rate;
  }

  public static string FormatAttackResult(string name, bool isHit, bool isCritical)
  {
    if (isHit) return isCritical ? $"{name} attack has critical!" : $"{name} attack!";

    return $"{name}'s attack miss!";
  }
}