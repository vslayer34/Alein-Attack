using Godot;
using System;

public partial class GameManager : Node2D
{
	[Signal]
	public delegate void OnScoreIncreasedEventHandler(int score);

	[Signal]
	public delegate void OnUpdateLivesUIEventHandler(int amount);

	[Export]
	private GameOverScreen _gameOverScreen;

	[Export]
	private GameEvents _gameEvents;

	[Export]
	private int _playerLivesLimit = 3;

	[Export]
	private Player _player;

	private int _currentLives;
	private int _gameScore;


	[ExportCategory("Gameplay sounds")]
	[Export]
	private AudioStreamPlayer2D _enemyHitSound;

	[Export]
	private AudioStreamPlayer2D _playerExplodeSound;

	[Export]
	private AudioStreamPlayer2D _rocketLaunchedSound;


    public override void _Ready()
    {
        base._Ready();

		_currentLives = _playerLivesLimit;
		PrintLivesToConsole();
		EnemyShip.OnEnemyShipDestroyed += PlayDeathSound;

		// _gameEvents.OnEnemyDeath += PlayDeathSound;

		EmitSignal(SignalName.OnScoreIncreased, _gameScore);
		EmitSignal(SignalName.OnUpdateLivesUI, _currentLives);
    }


    public override void _ExitTree()
    {
        base._ExitTree();
		EnemyShip.OnEnemyShipDestroyed -= PlayDeathSound;
		// EnemyShip.OnEnemyShipDestroyed -= IncreaseScore;
    }


    /// <summary>
    /// Detect escaped enemy ship and destroy it
    /// </summary>
    /// <param name="area">the enemy ship that entere the area</param>
    private void OnDeathZoneEntered(Area2D area)
	{
		if (area is IDamagable)
		{
			area.QueueFree();
		}
	}


	/// <summary>
	/// Reduce the number of lives the player has
	/// </summary>
	private void ReducePlayerLives()
	{
		_currentLives--;
		EmitSignal(SignalName.OnUpdateLivesUI, _currentLives);

		if (_currentLives < 1)
		{
			_playerExplodeSound.Play();
			GD.Print("Game Over!");
			_player.Visible = false;
			_player.ProcessMode = ProcessModeEnum.Disabled;
			
			GetTree().CreateTimer(1.5f).Timeout += () => _gameOverScreen.Visible = true;
			return;
		}
	}

	private void OnPlayerCollision()
	{
		ReducePlayerLives();
	}


	/// <summary>
	/// Increase the player score by fixed amount 100
	/// for each ship destroyed
	/// </summary>
	/// <param name="enemyShip"></param>
	private void IncreaseScore(EnemyShip enemyShip)
	{
		_gameScore += 100;
		
		EmitSignal(SignalName.OnScoreIncreased, _gameScore);

		enemyShip.OnDeath -= IncreaseScore;
	}

	private void OnNewEnemySpawned(EnemyShip newSpawnedEnemy)
	{
		// newSpawnedEnemy.Connect(EnemyShip.SignalName.OnDeath, Callable.From(IncreaseScore));
		newSpawnedEnemy.OnDeath += IncreaseScore;
	}

	private void PlayDeathSound()
    {
        _enemyHitSound.Play();
    }


	private void PrintLivesToConsole() => GD.Print($"Lives: {_currentLives} of {_playerLivesLimit}");


	private void OnEnemySpawnerPathEnemySpawned(PathEnemy pathEnemy)
	{
		AddChild(pathEnemy);
		pathEnemy.Enemy.OnDeath += IncreaseScore;
	}


	/// <summary>
	/// Current game score
	/// </summary>
	public int GameScore { get => _gameScore; }
}
