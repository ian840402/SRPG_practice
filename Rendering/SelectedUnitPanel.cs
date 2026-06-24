using Godot;
using SRPGPractice.Core;

namespace SRPGPractice.Rendering;

public partial class SelectedUnitPanel : PanelContainer
{
  private readonly VBoxContainer _container = new();
  private readonly VBoxContainer _infoContainer = new();
  private readonly Label _nullLabel = new();
  private readonly Label _nameLabel;
  private readonly Label _unitClassLabel;
  private readonly Label _hpLabel;
  private readonly Label _mpLabel;
  private readonly Label _attackLabel;
  private readonly Label _hitLabel;
  private readonly Label _criticalRateLabel;
  private readonly Label _rangeLabel;

  public SelectedUnitPanel(Vector2 position)
  {
    _nullLabel.Text = "No unit selected.";
    _container.AddChild(_nullLabel);
    _container.AddChild(_infoContainer);
    _nameLabel = AddInfoLabel();
    _unitClassLabel = AddInfoLabel();
    _hpLabel = AddInfoLabel();
    _mpLabel = AddInfoLabel();
    _attackLabel = AddInfoLabel();
    _hitLabel = AddInfoLabel();
    _criticalRateLabel = AddInfoLabel();
    _rangeLabel = AddInfoLabel();

    _infoContainer.Visible = false;
    Position = position;
    CustomMinimumSize = new Vector2(300, 400);
    AddChild(_container);
  }

  private Label AddInfoLabel()
  {
    var label = new Label();
    _infoContainer.AddChild(label);
    return label;
  }

  public void ShowInfo(bool isShow)
  {
    _nullLabel.Visible = !isShow;
    _infoContainer.Visible = isShow;
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
}