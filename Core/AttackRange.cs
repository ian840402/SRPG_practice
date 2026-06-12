public readonly struct AttackRange
{
  public AttackRange(int min, int max)
  {
    Min = min;
    Max = max;
  }

  public int Min { get; }
  public int Max { get; }
}
