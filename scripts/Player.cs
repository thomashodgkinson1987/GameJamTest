using System;
using Godot;
using static PaintColors;

public class Player : KinematicBody2D
{

	#region Enums

	public enum EFacingDirection { Left, Right }

	#endregion // Enums



	#region Nodes

	public Sprite node_sprite;
	public CollisionShape2D node_collisionShape2D;
	public Position2D node_projectileSpawnPosition;

	#endregion // Nodes



	#region Properties

	[Export] public EPaintColor PaintColor { get; private set; } = EPaintColor.White;

	public Vector2 Velocity { get; set; } = Vector2.Zero;

	[Export] public float MaxMoveSpeed { get; set; } = 128f;
	[Export] public float MaxFallSpeed { get; set; } = 1024f;

	[Export] public float MoveAcceleration { get; set; } = 1024f;
	[Export] public float FallAcceleration { get; set; } = 256f;

	[Export] public float MoveDeceleration { get; set; } = 1024f;

	public int JumpCount { get; set; } = 0;
	[Export] public int JumpLimit { get; set; } = 2;
	[Export] public float JumpHeight { get; set; } = 32f;

	public EFacingDirection FacingDirection { get; set; } = EFacingDirection.Right;

	public Vector2 AimingDirection { get; set; } = Vector2.Zero;

	public bool IsOnGround { get; set; } = false;
	public bool IsJumping { get; set; } = false;
	public bool IsFalling { get; set; } = false;

	public bool IsMoveLeftPressed { get; set; } = false;
	public bool IsMoveRightPressed { get; set; } = false;
	public bool IsJumpPressed { get; set; } = false;
	public bool IsShootPressed { get; set; } = false;
	public bool IsThrowGrenadePressed { get; set; } = false;

	public bool WasMoveLeftPressed { get; set; } = false;
	public bool WasMoveRightPressed { get; set; } = false;
	public bool WasJumpPressed { get; set; } = false;
	public bool WasShootPressed { get; set; } = false;
	public bool WasThrowGrenadePressed { get; set; } = false;

	public bool IsMoveLeftReleased { get; set; } = false;
	public bool IsMoveRightReleased { get; set; } = false;
	public bool IsJumpReleased { get; set; } = false;
	public bool IsShootReleased { get; set; } = false;
	public bool IsThrowGrenadeReleased { get; set; } = false;

	public bool WasMoveLeftReleased { get; set; } = false;
	public bool WasMoveRightReleased { get; set; } = false;
	public bool WasJumpReleased { get; set; } = false;
	public bool WasShootReleased { get; set; } = false;
	public bool WasThrowGrenadeReleased { get; set; } = false;

	[Export] public bool IsInfiniteJumps { get; set; } = true;
	[Export] public bool IsInfiniteGrenades { get; set; } = true;

	[Export] public bool HasPaintShoes { get; set; } = false;

	[Export] public int NumberOfGrenades { get; set; } = 3;
	[Export] public EPaintColor GunProjectileColor { get; set; } = EPaintColor.White;
	[Export] public EPaintColor GrenadeColor { get; set; } = EPaintColor.White;

	[Export] public float GrenadeThrowTimeLimit { get; set; } = 1f;
	[Export] public float GrenadeThrowTimer { get; set; } = 0f;

	#endregion // Properties



	#region Fields

	#endregion // Fields



	#region Godot methods

	public override void _EnterTree ()
	{
		node_sprite = GetNode<Sprite>("Sprite");
		node_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		node_projectileSpawnPosition = GetNode<Position2D>("ProjectileSpawnPosition");
	}

	public override void _Ready ()
	{
		node_sprite.SelfModulate = PaintColors.GetColorFromPaintColor(PaintColor);
	}

	#endregion // Godot methods



	#region Public methods

	public void SetPaintColor (EPaintColor paintColor)
	{
		PaintColor = paintColor;
		node_sprite.SelfModulate = PaintColors.GetColorFromPaintColor(paintColor);
	}

	#endregion // Public methods

}
