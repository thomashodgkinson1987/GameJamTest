using System;
using Godot;

public class PickUp : Area2D
{

	#region Enums

	public enum EPickUpType
	{
		None,
		Grenade,
		Rifle, MachineGun, SniperRifle,
		PaintGloves, PaintShoes, BigBadBooties
	}

	#endregion // Enums



	#region Nodes

	public Sprite node_sprite;
	public CollisionShape2D node_collisionShape2D;

	#endregion // Nodes



	#region Properties

	[Export] public EPickUpType PickUpType { get; set; } = EPickUpType.None;

	public bool HasBeenCollected { get; set; } = false;
	public bool HasBeenQueuedFree { get; set; } = false;

	#endregion // Properties



	#region Godot methods

	public override void _EnterTree ()
	{
		node_sprite = GetNode<Sprite>("Sprite");
		node_collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public override void _Process (float delta)
	{
		if (HasBeenCollected && !HasBeenQueuedFree)
		{
			HasBeenQueuedFree = true;
			QueueFree();
		}
	}

	#endregion // Godot methods



	#region Callback methods

	private void OnBodyEnteredArea (PhysicsBody2D body)
	{
		if (HasBeenCollected) return;

		if (body is Player player)
		{
			HasBeenCollected = true;
			switch (PickUpType)
			{
				case EPickUpType.None:
					break;
				case EPickUpType.Grenade:
					player.GrenadesCount = Mathf.Clamp(player.GrenadesCount + 1, 0, 99);
					break;
				case EPickUpType.Rifle:
					player.GunEquipSlot = Player.EGunEquipSlot.Rifle;
					player.RifleBulletsCount = Mathf.Clamp(player.RifleBulletsCount + 16, 0, 99);
					break;
				case EPickUpType.MachineGun:
					player.GunEquipSlot = Player.EGunEquipSlot.MachineGun;
					player.MachineGunBulletsCount = Mathf.Clamp(player.MachineGunBulletsCount + 16, 0, 99);
					break;
				case EPickUpType.SniperRifle:
					player.GunEquipSlot = Player.EGunEquipSlot.SniperRifle;
					player.SniperRifleBulletsCount = Mathf.Clamp(player.SniperRifleBulletsCount + 16, 0, 99);
					break;
				case EPickUpType.PaintGloves:
					player.HasPaintGloves = true;
					break;
				case EPickUpType.PaintShoes:
					player.HasPaintShoes = true;
					break;
				case EPickUpType.BigBadBooties:
					player.HasBigBadBooties = true;
					break;
				default:
					break;
			}
		}
	}

	#endregion // Callback methods

}
