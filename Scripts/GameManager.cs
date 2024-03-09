using Godot;
using System;

public partial class GameManager : Node2D
{
	[Signal]
	public delegate void OnScoreIncreasedEventHandler(int score);

	[Signal]
	public delegate void OnUpdateLivesUIEventHandler(int amount);

	[Export]
	private int _playerLivesLimit = 3;

	private int _currentLives;
	private int _gameScore;


    public override void _Ready()
    {
        base._Ready();
		_currentLives = _playerLivesLimit;
		PrintLivesToConsole();
		// EnemyShip.OnEnemyShipDestroyed += IncreaseScore;

		EmitSignal(SignalName.OnScoreIncreased, _gameScore);
		EmitSignal(SignalName.OnUpdateLivesUI, _currentLives);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
		// EnemyShip.OnEnemyShipDestroyed -= IncreaseScore;
    }


    /// <summary>
    /// Detect escaped enemy ship and destroy it
    /// </summary>
    /// <param name="area">the enemy ship that entere the area</param>
    private void OnDeathZoneEntered(Area2D area)
	{
		if (area is IDamagable escapedEnemyShip)
		{
			escapedEnemyShip.Die();
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
			GD.Print("Game Over!");
			GetTree().Paused = true;
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


	private void PrintLivesToConsole() => GD.Print($"Lives: {_currentLives} of {_playerLivesLimit}");
}
