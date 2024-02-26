using Godot;
using System;

public partial class Rocket : Area2D
{
	[ExportGroup("Rocked Properties")]
	[Export]
	private float _speed;

	float _time = 0.0f;
	float _accerelationDuration = 1.0f;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		LaunchRocket((float)delta);
    }


	/// <summary>
	/// Launch a rocket with lazy launch effect
	/// </summary>
	/// <param name="delta"></param>
	private void LaunchRocket(float delta)
	{
		float _launchSpeed;

		while (_time <= _accerelationDuration)
		{
			_launchSpeed = Mathf.Lerp(0.0f, _speed / 4.0f, _time / _accerelationDuration);
			_time += delta;
			GlobalPosition += Vector2.Right * _launchSpeed * delta;
			return;
		}

		_launchSpeed = _speed;
		
		GlobalPosition += Vector2.Right * _launchSpeed * delta;
	}
}
