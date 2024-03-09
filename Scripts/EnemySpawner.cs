using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Signal]
	public delegate void OnNewEnemySpawnedEventHandler(EnemyShip enemyShip);
	[Export]
	private PackedScene _enemyShipScene;

	private Node2D _spawnPoints;

	// Reference to newly created enemy ship
    private EnemyShip _enemyShip;

    public override void _Ready()
    {
        base._Ready();
		_spawnPoints = GetNode<Node2D>("SpawnPoints");
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
}
