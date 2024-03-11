using Godot;
using MEC;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody2D, IDamagable
{

	[Signal]
	public delegate void OnPlayerCollisionEventHandler();

	[ExportGroup("Input Map Actions")]
	[Export]
	public StringName MoveLeft { get; private set; }

	[Export]
	public StringName MoveRight { get; private set; }

	[Export]
	public StringName MoveUp { get; private set; }

	[Export]
	public StringName MoveDown { get; private set; }

	[Export]
	public StringName Shoot { get; private set; }


	[ExportGroup("Player properties")]
	[Export]
	private float _playerSpeed;
	[Export]
	private Sprite2D _playerSprite;

	[ExportGroup("Rocket Scene")]
	[Export]
	private PackedScene _rocketScene;
	
	
	// Reference to the rocket script and node spawn position
	private Rocket _rocket;
	private Node _spawnNode;


	// adjust velocity vector
	private Vector2 _velocityVector;
	
	
	// get screen size for clamping player position
	private Vector2 _screenSize;
	private Vector2 _modifiedGlobalPosition;

	private float _playerSpriteWeidth;
	private float _playerSpriteHeight;


	private bool _isRocketReady = true;


    public override void _Ready()
    {
        base._Ready();
		_spawnNode = GetNode<Node>("SpawnPoint");

		float _playerSpriteScale = _playerSprite.Transform.Scale.X;

		_playerSpriteWeidth = _playerSprite.GetRect().Size.X * _playerSpriteScale / 2.0f;
		_playerSpriteHeight = _playerSprite.GetRect().Size.Y * _playerSpriteScale / 2.0f;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
		if (Input.IsActionJustPressed(Shoot) && _isRocketReady)
		{
			Timing.RunCoroutine(ShootRocket());
		}
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		// Reset the velocity vector when the player stop inout actions
		_velocityVector = Vector2.Zero;
		Velocity = Vector2.Zero;

		if (Input.IsActionPressed(MoveLeft))
		{
			_velocityVector.X = -1.0f * _playerSpeed;
			Velocity = _velocityVector;
		}

		if (Input.IsActionPressed(MoveRight))
		{
			_velocityVector.X = _playerSpeed;
			Velocity = _velocityVector;
		}

		if (Input.IsActionPressed(MoveUp))
		{
			_velocityVector.Y = -1.0f * _playerSpeed;
			Velocity = _velocityVector;
		}

		if (Input.IsActionPressed(MoveDown))
		{
			_velocityVector.Y = _playerSpeed;
			Velocity = _velocityVector;
		}


		ClampPlayerToViewport();
		MoveAndSlide();
    }



	private void ClampPlayerToViewport()
	{
		_screenSize = GetViewportRect().Size;
		_modifiedGlobalPosition = GlobalPosition;

		// one way to it
		// if (GlobalPosition.X <= 0.0f + _playerSpriteWeidth)
		// {
		// 	_modifiedGlobalPosition.X = 0.0f + _playerSpriteWeidth;
			
		// }
		// if (GlobalPosition.X >= _screenSize.X - _playerSpriteWeidth)
		// {
		// 	_modifiedGlobalPosition.X = _screenSize.X - _playerSpriteWeidth;
		// }
		// if (GlobalPosition.Y <= 0.0f + _playerSpriteHeight)
		// {
		// 	_modifiedGlobalPosition.Y = 0.0f + _playerSpriteHeight;
		// }
		// if (GlobalPosition.Y >= _screenSize.Y - _playerSpriteHeight)
		// {
		// 	_modifiedGlobalPosition.Y = _screenSize.Y - _playerSpriteHeight;
		// }

		// Another way
		// GlobalPosition = GlobalPosition.Clamp(Vector2.Zero, _screenSize);

		// Third Way
		_modifiedGlobalPosition.X = Math.Clamp(GlobalPosition.X, 0.0f + _playerSpriteWeidth, _screenSize.X - _playerSpriteWeidth);
		_modifiedGlobalPosition.Y = Math.Clamp(GlobalPosition.Y, 0.0f + _playerSpriteHeight, _screenSize.Y - _playerSpriteHeight);
		

		GlobalPosition = _modifiedGlobalPosition;
	}

	
	/// <summary>
	/// Instansiate the rocket and set its launch offset
	/// </summary>
	private IEnumerator<double> ShootRocket()
	{
		_isRocketReady = false;
		float launchOffset = 75.0f;
		Vector2 launchVector = new Vector2(GlobalPosition.X + launchOffset, GlobalPosition.Y);

		_rocket = _rocketScene.Instantiate<Rocket>();
		_spawnNode.AddChild(_rocket);
		_rocket.GlobalPosition = launchVector;

		yield return Timing.WaitForSeconds(0.2f);
		
		_isRocketReady = true;
	}


	/// <summary>
	/// implementation for take damge for hte player
	/// </summary>
    public void Die(bool isHitByRocket)
    {
		EmitSignal(SignalName.OnPlayerCollision);
    }

}
