using Godot;

public sealed class Unit
{
  public Unit(Vector2I gridPosition, int hp, int attackPower, int moveRange, Team team)
  {
    GridPosition = gridPosition;
    Hp = hp;
    AttackPower = attackPower;
    MoveRange = moveRange;
    Team = team;
  }

  public Vector2I GridPosition { get; private set; }
  public int Hp { get; private set; }
  public int AttackPower { get; }
  public int MoveRange { get; }
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
