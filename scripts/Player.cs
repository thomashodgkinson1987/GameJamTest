using System;
using Godot;
using static PaintColors;

public class Player : KinematicBody2D
{

	#region Enums

	public enum EFacingDirection { Left, Right }

	//public enum EBulletType { Standard, Rifle, MachineGun, SniperRifle }

	public enum EGunEquipSlot { None, Rifle, MachineGun, SniperRifle }

	#endregion // Enums



	#region Nodes

	public AnimatedSprite node_animatedSprite;
	public Line2D node_aimingLine;
	public CollisionShape2D node_collisionShape2D;
	public Position2D node_projectileSpawnPosition;

	#endregion // Nodes



	#region Properties

	[Export] public EPaintColor PaintColor { get; private set; } = EPaintColor.White;

	[Export] public Vector2 Velocity { get; set; } = Vector2.Zero;

	[Export] public float MaxMoveSpeed { get; set; } = 128f;
	[Export] public float MaxFallSpeed { get; set; } = 1024f;

	[Export] public float MoveAcceleration { get; set; } = 1024f;
	[Export] public float FallAcceleration { get; set; } = 256f;
	[Export] public float HoldDownFallAcceleration { get; set; } = 1024f;
	[Export] public float WallSlideFallAcceleration { get; set; } = 64f;

	[Export] public float MoveDeceleration { get; set; } = 1024f;

	[Export] public int JumpCount { get; set; } = 0;
	[Export] public int JumpLimit { get; set; } = 2;
	[Export] public float JumpHeight { get; set; } = 32f;

	[Export] public Vector2 WallJumpDirection { get; set; } = new Vector2(1, -1f).Normalized();
	[Export] public float WallJumpStrength { get; set; } = 256f;

	[Export] public Vector2 AimingDirection { get; set; } = Vector2.Zero;

	public bool IsOnGround { get; set; } = false;
	public bool WasOnGround { get; set; } = false;

	public bool IsOnWallLeft { get; set; } = false;
	public bool IsOnWallRight { get; set; } = false;

	public bool WasOnWallLeft { get; set; } = false;
	public bool WasOnWallRight { get; set; } = false;

	public bool IsMoveLeftPressed { get; set; } = false;
	public bool IsMoveRightPressed { get; set; } = false;
	public bool IsDownPressed { get; set; } = false;
	public bool IsJumpPressed { get; set; } = false;
	public bool IsShootPressed { get; set; } = false;
	public bool IsThrowGrenadePressed { get; set; } = false;

	public bool WasMoveLeftPressed { get; set; } = false;
	public bool WasMoveRightPressed { get; set; } = false;
	public bool WasDownPressed { get; set; } = false;
	public bool WasJumpPressed { get; set; } = false;
	public bool WasShootPressed { get; set; } = false;
	public bool WasThrowGrenadePressed { get; set; } = false;

	public bool IsMoveLeftReleased { get; set; } = false;
	public bool IsMoveRightReleased { get; set; } = false;
	public bool IsDownReleased { get; set; } = false;
	public bool IsJumpReleased { get; set; } = false;
	public bool IsShootReleased { get; set; } = false;
	public bool IsThrowGrenadeReleased { get; set; } = false;

	public bool WasMoveLeftReleased { get; set; } = false;
	public bool WasMoveRightReleased { get; set; } = false;
	public bool WasDownReleased { get; set; } = false;
	public bool WasJumpReleased { get; set; } = false;
	public bool WasShootReleased { get; set; } = false;
	public bool WasThrowGrenadeReleased { get; set; } = false;

	[Export] public bool IsInfiniteJumps { get; set; } = false;

	[Export] public bool IsInfinitePowerGauge { get; set; } = false;

	[Export] public bool IsInfiniteGrenades { get; set; } = false;

	[Export] public bool IsInfiniteRifleAmmunition { get; set; } = false;
	[Export] public bool IsInfiniteMachineGunAmmunition { get; set; } = false;
	[Export] public bool IsInfiniteSniperRifleAmmunition { get; set; } = false;

	[Export] public EGunEquipSlot GunEquipSlot { get; set; } = EGunEquipSlot.None;

	[Export] public bool HasPaintGloves { get; set; } = false;
	[Export] public bool HasPaintShoes { get; set; } = false;
	[Export] public bool HasBigBadBooties { get; set; } = false;

	[Export] public EPaintColor PowerGaugePaintColor { get; set; } = EPaintColor.White;

	[Export] public EPaintColor GrenadePaintColor { get; set; } = EPaintColor.White;

	[Export] public EPaintColor RiflePaintColor { get; set; } = EPaintColor.White;
	[Export] public EPaintColor MachineGunPaintColor { get; set; } = EPaintColor.White;
	[Export] public EPaintColor SniperRiflePaintColor { get; set; } = EPaintColor.White;

	[Export] public EPaintColor PaintGlovesPaintColor { get; set; } = EPaintColor.White;
	[Export] public EPaintColor PaintShoesPaintColor { get; set; } = EPaintColor.White;
	[Export] public EPaintColor BigBadBootiesPaintColor { get; set; } = EPaintColor.White;

	[Export] public float GrenadeThrowTimeLimit { get; set; } = 1f;
	[Export] public float GrenadeThrowTimer { get; set; } = 0f;

	[Export] public float AimingLineLength { get; set; } = 16f;

	[Export] public float PowerGaugeLimit { get; set; } = 100f;
	[Export] public float PowerGaugeCurrent { get; set; } = 100f;
	[Export] public float PowerGaugeRechargeRate { get; set; } = 0.25f;

	[Export] public float StandardBulletPowerCost { get; set; } = 10f;

	[Export] public int GrenadesCount { get; set; } = 3;

	[Export] public int RifleBulletsCount { get; set; } = 0;
	[Export] public int MachineGunBulletsCount { get; set; } = 0;
	[Export] public int SniperRifleBulletsCount { get; set; } = 0;

	[Export] public float MachineGunFireRate { get; set; } = 0.1f;
	[Export] public float MachineGunFireRateTimer { get; set; } = 0f;

	[Export] public float DisableOneWayCollisionTime { get; set; } = 0.5f;
	[Export] public float DisableOneWayCollisionTimer { get; set; } = 0f;

	#endregion // Properties



	#region Godot methods

	public override void _EnterTree ()
	{
		node_animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		node_aimingLine = GetNode<Line2D>("AimingLine");
		node_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
		node_projectileSpawnPosition = GetNode<Position2D>("ProjectileSpawnPosition");
	}

	public override void _Ready ()
	{
		Color color = PaintColors.GetColorFromPaintColor(PaintColor);
		node_animatedSprite.SelfModulate = color;
		node_aimingLine.DefaultColor = color;

		PowerGaugePaintColor = PaintColor;
		GrenadePaintColor = PaintColor;
		RiflePaintColor = PaintColor;
		MachineGunPaintColor = PaintColor;
		SniperRiflePaintColor = PaintColor;
		PaintGlovesPaintColor = PaintColor;
		PaintShoesPaintColor = PaintColor;
		BigBadBootiesPaintColor = PaintColor;

		node_animatedSprite.Play("idle");
	}

	#endregion // Godot methods



	#region Public methods

	public void SetPaintColor (EPaintColor paintColor)
	{
		PaintColor = paintColor;

		Color color = PaintColors.GetColorFromPaintColor(paintColor);
		node_animatedSprite.SelfModulate = color;
		node_aimingLine.DefaultColor = color;

		PowerGaugePaintColor = PaintColor;
		GrenadePaintColor = PaintColor;
		RiflePaintColor = PaintColor;
		MachineGunPaintColor = PaintColor;
		SniperRiflePaintColor = PaintColor;
		PaintGlovesPaintColor = PaintColor;
		PaintShoesPaintColor = PaintColor;
		BigBadBootiesPaintColor = PaintColor;
	}

	public void SetAimingLineDirection (Vector2 direction)
	{
		direction = direction.Normalized();
		direction *= AimingLineLength;
		node_aimingLine.SetPointPosition(1, direction);
	}
	public void SetAimingLineDirection (float x, float y)
	{
		SetAimingLineDirection(new Vector2(x, y));
	}

	public void EnableOneWayPlatformCollision ()
	{
		SetCollisionMaskBit(4, true);
	}
	public void DisableOneWayPlatformCollision ()
	{
		SetCollisionMaskBit(4, false);
	}

	#endregion // Public methods

}
