using System;
using Godot;

public class OneShotSound : AudioStreamPlayer
{

	#region Fields

	private bool m_isQueueFree = false;
	private bool m_isQueueFreeAlready = false;

	#endregion // Fields



	#region Godot methods

	public override void _Process (float delta)
	{
		if (m_isQueueFree && !m_isQueueFreeAlready)
		{
			m_isQueueFreeAlready = true;
			QueueFree();
		}
	}

	#endregion // Godot methods



	#region Callback methods

	private void OnAudioStreamPlayerFinished ()
	{
		Stop();
		m_isQueueFree = true;
	}

	#endregion // Callback methods

}
