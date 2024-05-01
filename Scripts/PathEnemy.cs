using Godot;
using System;

public partial class PathEnemy : Path2D
{
    [Export]
    public EnemyShip Enemy { get; private set; }
    private PathFollow2D _pathFollow;
    private float _enemySpeed = 0.15f;



    public override void _Ready()
    {
        _pathFollow = GetNode<PathFollow2D>("PathFollow2D");
        _pathFollow.ProgressRatio = 0.0f;
    }

    
    public override void _Process(double delta)
    {
        if (_pathFollow.ProgressRatio >= 1.0f)
        {
            QueueFree();
        }

        _pathFollow.ProgressRatio += _enemySpeed * (float)delta;
    }
}
