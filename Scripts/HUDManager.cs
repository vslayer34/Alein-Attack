using Godot;
using System;

public partial class HUDManager : Control
{
	private Label _label;

	[Export]
	private GameManager _gameManager;

    public override void _Ready()
    {
        base._Ready();
		_label = GetNode<Label>("Label");
		SetScoreLabel(0);

		_gameManager.OnScoreIncreased += SetScoreLabel;
    }


    public override void _ExitTree()
    {
        base._ExitTree();
		_gameManager.OnScoreIncreased -= SetScoreLabel;
    }

    private void SetScoreLabel(int newScore)
	{
		_label.Text = $"Score: {newScore}";
	}
}
