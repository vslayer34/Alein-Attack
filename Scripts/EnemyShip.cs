using Godot;
using System;
using System.ComponentModel;

public partial class EnemyShip : Area2D, IDamagable
{
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
	/// Called when the enemy is hit by the rocket
	/// </summary>
	public void Die()
    {
        QueueFree();
    }
}
