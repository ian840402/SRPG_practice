using System.Collections.Generic;
using Godot;

namespace SRPGPractice.Core;

public sealed class Unit
{
  public Unit(string name, Vector2I gridPosition, UnitClass unitClass, Team team)
  {
    var definition = UnitClassDefinitions.Get(unitClass);
    GridPosition = gridPosition;
    Name = name;
    Hp = definition.MaxHp;
    Mp = definition.MaxMp;
    Team = team;
    UnitClass = unitClass;
  }

  public Vector2I GridPosition { get; private set; }
  public string Name { get; }
  public int Hp { get; private set; }
  public int Mp { get; private set; }
  public UnitClass UnitClass { get; }
  public UnitClassDefinition ClassDefinition => UnitClassDefinitions.Get(UnitClass);
  public int MaxHp => ClassDefinition.MaxHp;
  public int MaxMp => ClassDefinition.MaxMp;
  public int AttackPower => ClassDefinition.AttackPower;
  public int Defense => ClassDefinition.Defense;
  public int HitCoefficient => ClassDefinition.HitCoefficient;
  public int Evasion => ClassDefinition.Evasion;
  public int MoveRange => ClassDefinition.MoveRange;
  public int CriticalRate => ClassDefinition.CriticalRate;
  public AttackRange NormalAttackRange => ClassDefinition.NormalAttackRange;
  public int RemainingMovePoints { get; private set; }
  public bool HasAttackedThisTurn { get; private set; }
  public bool HasWaitedThisTurn { get; private set; }
  public bool CanMoveThisTurn => RemainingMovePoints > 0 && !HasWaitedThisTurn;
  public bool CanAttackThisTurn => !HasAttackedThisTurn && !HasWaitedThisTurn;
  public Team Team { get; }
  public Dictionary<Vector2I, int> ValidMovementTiles { get; private set; } = new();

  public void MoveTo(Vector2I newPosition)
  {
    GridPosition = newPosition;
  }

  public void SpendMovePoints(int movePoints)
  {
    RemainingMovePoints = Mathf.Max(0, RemainingMovePoints - movePoints);
  }

  public void MarkAttacked()
  {
    HasAttackedThisTurn = true;
  }

  public void MarkWaited()
  {
    HasWaitedThisTurn = true;
  }

  public void StartTurn()
  {
    RemainingMovePoints = MoveRange;
    HasAttackedThisTurn = false;
    HasWaitedThisTurn = false;
  }

  public void SetValidMovementTiles(Dictionary<Vector2I, int> tiles)
  {
    ValidMovementTiles = tiles;
  }

  public void TakeDamage(int damage)
  {
    Hp = Mathf.Max(0, Hp - damage);
  }
}
