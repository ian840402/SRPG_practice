using Godot;
using System;
using SRPGPractice.Core;

namespace SRPGPractice.Rendering;

public partial class SelectedUnitPanel : PanelContainer
{
  private readonly HBoxContainer _wrapContainer = new();
  private readonly VBoxContainer _infoContainer = new();
  private readonly VBoxContainer _unitInfoContainer = new();
  private readonly Label _nullLabel = new();
  private readonly Label _nameLabel;
  private readonly Label _unitClassLabel;
  private readonly Label _hpLabel;
  private readonly Label _mpLabel;
  private readonly Label _attackLabel;
  private readonly Label _hitLabel;
  private readonly Label _criticalRateLabel;
  private readonly Label _rangeLabel;
  private readonly VBoxContainer _actionContainer = new();
  private readonly Button _moveButton = new();
  private readonly Button _attackButton = new();
  private readonly Button _waitButton = new();
  public event Action? MoveRequested;
  public event Action? AttackRequested;
  public event Action? WaitRequested;

  public SelectedUnitPanel(Vector2 position)
  {
    InitInfoContainer();
    _nameLabel = AddInfoLabel();
    _unitClassLabel = AddInfoLabel();
    _hpLabel = AddInfoLabel();
    _mpLabel = AddInfoLabel();
    _attackLabel = AddInfoLabel();
    _hitLabel = AddInfoLabel();
    _criticalRateLabel = AddInfoLabel();
    _rangeLabel = AddInfoLabel();
    InitActionContainer();

    Position = position;
    CustomMinimumSize = new Vector2(500, 400);
    AddChild(_wrapContainer);
  }

  private Label AddInfoLabel()
  {
    var label = new Label();
    _unitInfoContainer.AddChild(label);
    return label;
  }

  private void InitInfoContainer()
  {
    _wrapContainer.AddChild(_infoContainer);
    _infoContainer.AddChild(_nullLabel);
    _infoContainer.AddChild(_unitInfoContainer);
    _infoContainer.CustomMinimumSize = new Vector2(300, 400);
    _nullLabel.Text = "No unit selected.";
    _unitInfoContainer.Visible = false;
  }

  public void ShowUnitInfo(bool isShow)
  {
    _nullLabel.Visible = !isShow;
    _unitInfoContainer.Visible = isShow;
  }

  public void SetUnitInfo(Unit unit)
  {
    _nameLabel.Text = unit.Name;
    _unitClassLabel.Text = $"CLASS: {unit.ClassDefinition.DisplayName}";
    _hpLabel.Text = $"HP: {unit.Hp} / {unit.ClassDefinition.MaxHp}";
    _mpLabel.Text = $"MP: {unit.Mp} / {unit.ClassDefinition.MaxMp}";
    _mpLabel.Visible = unit.ClassDefinition.MaxMp > 0;
    _attackLabel.Text = $"ATK: {unit.AttackPower} / DEF: {unit.Defense}";
    _hitLabel.Text = $"HIT: {unit.HitCoefficient}% / EVA: {unit.Evasion}%";
    _criticalRateLabel.Text = $"CRIT: {unit.CriticalRate}%";
    _rangeLabel.Text = $"RANGE: {unit.ClassDefinition.NormalAttackRange.Min} - {unit.ClassDefinition.NormalAttackRange.Max}";
  }

  private void InitActionContainer()
  {
    _wrapContainer.AddChild(_actionContainer);
    _actionContainer.AddChild(_moveButton);
    _actionContainer.AddChild(_attackButton);
    _actionContainer.AddChild(_waitButton);

    _moveButton.Text = "Move";
    _moveButton.Pressed += () => MoveRequested?.Invoke();
    _attackButton.Text = "Attack";
    _attackButton.Pressed += () => AttackRequested?.Invoke();
    _waitButton.Text = "Wait";
    _waitButton.Pressed += () => WaitRequested?.Invoke();

    SetAllButtonDisabled();
  }

  public void SetAllButtonDisabled()
  {
    _moveButton.Disabled = true;
    _attackButton.Disabled = true;
    _waitButton.Disabled = true;
  }

  public void SetAllButtonUndisabled(Unit unit)
  {
    _moveButton.Disabled = !unit.CanMoveThisTurn;
    _attackButton.Disabled = !unit.CanAttackThisTurn;
    _waitButton.Disabled = unit.HasWaitedThisTurn;
  }
}
