using System;
using Godot;

public class Bullet : Projectile
{

	#region Properties

	[Export] public float Speed { get; set; } = 64f;

	#endregion // Properties

}
