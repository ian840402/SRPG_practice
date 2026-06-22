public readonly record struct AttackRange(int Min, int Max)
{
  public bool Contains(int distance)
  {
    return distance >= Min && distance <= Max;
  }
}
