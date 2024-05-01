using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Signal]
	public delegate void OnNewEnemySpawnedEventHandler(EnemyShip enemyShip);

	[Signal]
	public delegate void PathEnemySpawnedEventHandler(PathEnemy pathEnemy);
	
	[Export]
	private PackedScene _enemyShipScene;

	[Export]
	private PackedScene _pathEnemyScene;


	private Node2D _spawnPoints;

	// Reference to newly created enemy ship
    private EnemyShip _enemyShip;

	private Timer _pathEnemyTimer;

    public override void _Ready()
    {
        base._Ready();
		_spawnPoints = GetNode<Node2D>("SpawnPoints");
		_pathEnemyTimer = GetNode<Timer>("PathEnemyTimer");
		_pathEnemyTimer.Timeout += OnPathEnemyTimerTimeOut;
    }

    private void OnTimerTimeout()
	{
		_enemyShip = _enemyShipScene.Instantiate<EnemyShip>();
		EmitSignal(SignalName.OnNewEnemySpawned, _enemyShip);

		ChooseRandomSpawnPoint().AddChild(_enemyShip);
	}

	private Node ChooseRandomSpawnPoint()
	{
		var spawnPoints = _spawnPoints.GetChildren();

		return spawnPoints.PickRandom();
	}


	private void OnPathEnemyTimerTimeOut()
    {
        SpawnPathEnemy();
    }

	private void SpawnPathEnemy()
	{
		var pathEnemy = _pathEnemyScene.Instantiate<Path2D>();
		EmitSignal(SignalName.PathEnemySpawned, pathEnemy);
	}
}
