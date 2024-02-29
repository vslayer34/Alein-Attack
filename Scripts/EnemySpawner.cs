using Godot;
using System;

public partial class EnemySpawner : Node2D
{
	private void OnTimerTimeout()
	{
		GD.Print("Time is out");
	}
}
