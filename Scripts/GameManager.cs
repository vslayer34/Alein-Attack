using Godot;
using System;

public partial class GameManager : Node2D
{
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
		PrintLivesToConsole();

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


	private void IncreaseScore(EnemyShip enemyShip)
	{
		_gameScore += 100;
		GD.Print($"Score: {_gameScore}");
		enemyShip.OnDeath -= IncreaseScore;
	}

	private void OnNewEnemySpawned(EnemyShip newSpawnedEnemy)
	{
		// newSpawnedEnemy.Connect(EnemyShip.SignalName.OnDeath, Callable.From(IncreaseScore));
		newSpawnedEnemy.OnDeath += IncreaseScore;
	}


	private void PrintLivesToConsole() => GD.Print($"Lives: {_currentLives} of {_playerLivesLimit}");
}
