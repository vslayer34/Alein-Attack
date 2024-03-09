using Godot;
using System;

public partial class GameOverScreen : Control
{
	[Export]
	private GameManager _gameManager;

	[ExportCategory("UI elements")]
	[Export]
	private Label _scoreLabel;
	
	[Export]
	private Button _retryBtn;

    public override void _Ready()
    {
        base._Ready();
		_retryBtn.Pressed += ReloadGameScene;
    }

    public override void _Notification(int what)
    {
        base._Notification(what);
		if (what == NotificationVisibilityChanged)
		{
			UpdateScoreLabel();
		}
    }


	/// <summary>
	/// Display the score the player earned through out the game
	/// </summary>
	private void UpdateScoreLabel()
	{
		if (Visible == true)
		{
			_scoreLabel.Text = $"Score: {_gameManager.GameScore}";
		}
	}


	/// <summary>
	/// Reload the current scene
	/// </summary>
    private void ReloadGameScene() => GetTree().ReloadCurrentScene();

}
