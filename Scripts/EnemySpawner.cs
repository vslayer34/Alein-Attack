using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	[Export]
	private PackedScene _enemyShipScene;

	[Export]
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
		GD.Print("Time is out");
		_enemyShip = _enemyShipScene.Instantiate<EnemyShip>();

		ChooseRandomSpawnPoint().AddChild(_enemyShip);
	}

	private Node ChooseRandomSpawnPoint()
	{
		var spawnPoints = _spawnPoints.GetChildren();

		return spawnPoints.PickRandom();
	}
}
