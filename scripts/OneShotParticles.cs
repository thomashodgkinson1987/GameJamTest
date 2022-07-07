using System;
using Godot;

public class OneShotParticles : CPUParticles2D
{

	#region Godot methods

	public override void _Ready ()
	{
		Emitting = true;
	}

	public override void _Process (float delta)
	{
		if (!Emitting)
		{
			QueueFree();
		}
	}

	#endregion // Godot methods

}
