using Godot;

public sealed class Unit
{
  public Unit(Vector2I gridPosition, int hp, int attackPower, int moveRange, Team team)
      : this(gridPosition, hp, attackPower, moveRange, team, UnitClass.Warrior)
  {
  }

  public Unit(Vector2I gridPosition, int hp, int attackPower, int moveRange, Team team, UnitClass unitClass)
  {
    GridPosition = gridPosition;
    Hp = hp;
    AttackPower = attackPower;
    MoveRange = moveRange;
    Team = team;
    UnitClass = unitClass;
  }

  public Unit(Vector2I gridPosition, UnitClass unitClass, Team team)
  {
    var definition = UnitClassDefinitions.Get(unitClass);
    GridPosition = gridPosition;
    Hp = definition.MaxHp;
    AttackPower = definition.AttackPower;
    MoveRange = definition.MoveRange;
    Team = team;
    UnitClass = unitClass;
  }

  public Vector2I GridPosition { get; private set; }
  public int Hp { get; private set; }
  public int AttackPower { get; }
  public int MoveRange { get; }
  public UnitClass UnitClass { get; }
  public UnitClassDefinition ClassDefinition => UnitClassDefinitions.Get(UnitClass);
  public int MaxHp => ClassDefinition.MaxHp;
  public int MaxMp => ClassDefinition.MaxMp;
  public int Defense => ClassDefinition.Defense;
  public int HitCoefficient => ClassDefinition.HitCoefficient;
  public int Evasion => ClassDefinition.Evasion;
  public int CriticalRate => ClassDefinition.CriticalRate;
  public AttackRange NormalAttackRange => ClassDefinition.NormalAttackRange;
  public int RemainingMovePoints { get; private set; }
  public bool HasAttackedThisTurn { get; private set; }
  public Team Team { get; }

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

  public void StartTurn()
  {
    RemainingMovePoints = MoveRange;
    HasAttackedThisTurn = false;
  }

  public void TakeDamage(int damage)
  {
    Hp = Mathf.Max(0, Hp - damage);
  }
}
