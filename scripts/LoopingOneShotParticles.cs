using System;
using Godot;

public class LoopingOneShotParticles : CPUParticles2D
{

	#region Properties

	public float Timer { get; set; } = 0f;
	public bool IsReadyToBeDestroyed { get; set; } = false;
	public bool WasReadyToBeDestroyed { get; set; } = false;

	#endregion // Properties



	#region Godot methods

	public override void _Ready ()
	{
		Emitting = true;
	}

	public override void _Process (float delta)
	{
		if (IsReadyToBeDestroyed)
		{
			if (!WasReadyToBeDestroyed)
			{
				WasReadyToBeDestroyed = true;
				Emitting = false;
			}
			Timer += delta;
			if (Timer >= Lifetime)
			{
				QueueFree();
			}
		}
	}

	#endregion // Godot methods

}
