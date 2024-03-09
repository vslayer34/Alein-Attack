using Godot;
using System;

public partial class EnemyShip : Area2D, IDamagable
{
	[Signal]
	public delegate void OnDeathEventHandler(EnemyShip enemyShip);
	// public static Action OnEnemyShipDestroyed;

	[ExportGroup("Enemy ship properties")]
	[Export]
	private float _speed;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		MoveTowardsPlayerSide((float)delta);
    }


	private void MoveTowardsPlayerSide(float delta)
	{
		Translate(Vector2.Left * delta * _speed);
	}


	/// <summary>
	/// Triggered when the enmey ship collide with the player<br/>
	/// Destroy itself<br/>
	/// Call the player implementaion of IDamagable
	/// </summary>
	/// <param name="body"></param>
	private void OnBodyEntered(Node2D body)
	{
		if (body is IDamagable player)
		{
			player.Die();
			Die();
		}
	}


	/// <summary>
	/// Called when the enemy is hit by the rocket
	/// </summary>
	public void Die()
    {
		// OnEnemyShipDestroyed?.Invoke();
		EmitSignal(SignalName.OnDeath, this);
        QueueFree();
    }
}
