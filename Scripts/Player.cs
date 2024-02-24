using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[ExportGroup("Input Map Actions")]
	[Export]
	public StringName MoveLeft { get; private set; }

	[Export]
	public StringName MoveRight { get; private set; }

	[Export]
	public StringName MoveUp { get; private set; }

	[Export]
	public StringName MoveDown { get; private set; }


	[ExportGroup("Player properties")]
	[Export]
	private float _playerSpeed;
	[Export]
	private Sprite2D _playerSprite;


	// adjust velocity vector
	private Vector2 _velocityVector;
	
	
	// get screen size for clamping player position
	private Vector2 _screenSize;
	private Vector2 _modifiedGlobalPosition;

	private float _playerSpriteWeidth;
	private float _playerSpriteHeight;


    public override void _Ready()
    {
        base._Ready();
		GD.Print(_playerSprite.GetRect().Size);
		float _playerSpriteScale = _playerSprite.Transform.Scale.X;

		_playerSpriteWeidth = _playerSprite.GetRect().Size.X * _playerSpriteScale / 2.0f;
		_playerSpriteHeight = _playerSprite.GetRect().Size.Y * _playerSpriteScale / 2.0f;
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
}
