using Godot;
using System;

public partial class GameManager : Node2D
{
	/// <summary>
	/// Detect escaped enemy ship and destroy it
	/// </summary>
	/// <param name="area">the enemy ship that entere the area</param>
	private void OnDeathZoneEntered(Area2D area)
	{
		if (area is IHitRocket escapedEnemyShip)
		{
			escapedEnemyShip.Die();
		}
	}
}
