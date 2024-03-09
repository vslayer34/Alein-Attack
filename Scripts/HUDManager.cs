using Godot;
using System;

public partial class HUDManager : Control
{
	private Label _label;

	[Export]
	private GameManager _gameManager;

	[Export]
	private Label _livesLabels;

    public override void _Ready()
    {
        base._Ready();
		_label = GetNode<Label>("Label");

		_gameManager.OnScoreIncreased += SetScoreLabel;
		_gameManager.OnUpdateLivesUI += DisplayLivesUI;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
		_gameManager.OnScoreIncreased -= SetScoreLabel;
		_gameManager.OnUpdateLivesUI -= DisplayLivesUI;
    }

	/// <summary>
	/// Update the score label based on the event of the Game Manager
	/// </summary>
	/// <param name="newScore">The new updated score</param>
    private void SetScoreLabel(int newScore)
	{
		_label.Text = $"Score: {newScore}";
	}


	/// <summary>
	/// Update the number of lives the player have based on the event of the Game Manager
	/// </summary>
	/// <param name="newScore">The new updated lives count</param>
	private void DisplayLivesUI(int amount)
    {
        _livesLabels.Text = amount.ToString();
    }
}
