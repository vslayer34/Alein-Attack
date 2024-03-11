using Godot;
using System;

public partial class Rocket : Area2D
{
	public Action OnRocketGainSpeed;

	[ExportGroup("Rocked Properties")]
	[Export]
	private float _speed;
	[Export]
	private AudioStreamPlayer2D _rocketLaunchSound;
	
	[Export]
	private VisibleOnScreenNotifier2D _visibilityNotifier;



	private float _time = 0.0f;
	private float _accerelationDuration = 0.5f;

	private bool _isSoundPlayed = false;


    public override void _Ready()
    {
        base._Ready();
		_visibilityNotifier.ScreenExited += OnScreenExited;
		OnRocketGainSpeed += PlayLaunchSound;
    }

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
			_launchSpeed = Mathf.Lerp(0.0f, _speed / 2.0f, _time / _accerelationDuration);
			_time += delta;
			GlobalPosition += Vector2.Right * _launchSpeed * delta;
			return;
		}

		_launchSpeed = _speed;
		
		GlobalPosition += Vector2.Right * _launchSpeed * delta;
		OnRocketGainSpeed?.Invoke();
		
	}


	private void PlayLaunchSound()
	{
		if (!_isSoundPlayed)
		{
			_isSoundPlayed = true;
			_rocketLaunchSound.Play();
		}
	}


	private void OnScreenExited()
	{
		OnRocketGainSpeed -= PlayLaunchSound;
		QueueFree();
	}

	/// <summary>
	/// Connected signal to the Area 2d Node on the rocket
	/// </summary>
	/// <param name="area">CBody collided with the rocket</param>
	private void OnAreaEntered(Area2D area)
	{
		if (area is IDamagable hit)
		{
			hit?.Die(true);
			QueueFree();
		}
	}
}
