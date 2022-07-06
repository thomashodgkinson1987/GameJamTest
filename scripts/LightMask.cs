using System;
using Godot;

public class LightMask : Light2D
{

	#region Nodes

	public Area2D node_area2D;
	public CollisionShape2D node_collisionShape2D;

	#endregion // Nodes



	#region Godot methods

	public override void _EnterTree ()
	{
		node_area2D = GetNode<Area2D>("Area2D");
		node_collisionShape2D = node_area2D.GetNode<CollisionShape2D>("CollisionShape2D");
	}

	#endregion // Godot methods



	#region Callback methods

	#endregion // Callback methods

}
