using System;
using Godot;
using static PaintColors;

public class Projectile : KinematicBody2D
{

	#region Nodes

	public Sprite node_sprite;
	public CollisionShape2D node_collisionShape2D;

	#endregion // Nodes



	#region Properties

	[Export] public EPaintColor PaintColor { get; set; } = EPaintColor.White;

	[Export] public PackedScene SpawnParticlesPackedScene { get; set; }
	[Export] public PackedScene LifetimeParticlesPackedScene { get; set; }
	[Export] public PackedScene DieParticlesPackedScene { get; set; }

	private float _lifetime = 4f;
	[Export]
	public float Lifetime
	{
		get => _lifetime;
		set => _lifetime = Mathf.Max(0, value);
	}
	private float _lifetimeTimer = 0f;
	[Export]
	public float LifetimeTimer
	{
		get => _lifetimeTimer;
		set => _lifetimeTimer = Mathf.Max(0, value);
	}

	[Export] public bool IsDieAfterDistanceLimitReached { get; set; } = false;
	[Export] public float DistanceLimit { get; set; } = 128f;
	[Export] public float DistanceTravelled { get; set; } = 0f;

	[Export] public Vector2 Velocity { get; set; } = new Vector2(64f, 0f);

	private float _explosionRadius = 8f;
	[Export]
	public float ExplosionRadius
	{
		get => _explosionRadius;
		set => _explosionRadius = Mathf.Max(1, value);
	}

	#endregion // Properties



	#region Godot methods

	public override void _EnterTree ()
	{
		node_sprite = GetNode<Sprite>("Sprite");
		node_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public override void _Ready ()
	{
		node_sprite.SelfModulate = PaintColors.GetColorFromPaintColor(PaintColor);
	}

	#endregion // Godot methods



	#region Public methods

	public virtual void SetPaintColor (EPaintColor paintColor)
	{
		PaintColor = paintColor;
		node_sprite.SelfModulate = PaintColors.GetColorFromPaintColor(paintColor);
	}

	#endregion // Public methods

}
