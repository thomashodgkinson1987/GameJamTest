using System;
using Godot;

public class Grenade : Projectile
{

	#region Nodes

	public CPUParticles2D node_particles;

	#endregion // Nodes



	#region Properties

	#endregion // Properties



	#region Godot methods

	public override void _EnterTree ()
	{
		base._EnterTree();
		node_particles = GetNode<CPUParticles2D>("CPUParticles2D");
	}

	#endregion // Godot methods



	#region Public methods

	public override void SetPaintColor (PaintColors.EPaintColor paintColor)
	{
		base.SetPaintColor(paintColor);

		node_particles.SelfModulate = PaintColors.GetColorFromPaintColor(paintColor);
	}

	#endregion // Public methods

}
