using System.Collections.Generic;
using Godot;
using static PaintColors;

public class MainSceneController : Node2D
{

	#region Nodes

	private TileMap node_tileMap;

	private Player node_player;

	private Node2D node_bullets;
	private Node2D node_grenades;

	private Node2D node_lightMasks;

	private Node2D node_bulletSpawnParticles;
	private Node2D node_grenadeSpawnParticles;

	private Node2D node_bulletDieParticles;
	private Node2D node_grenadeDieParticles;

	#endregion // Nodes



	#region Fields

	[Export] private PackedScene m_bulletPackedScene;
	[Export] private PackedScene m_grenadePackedScene;
	[Export] private PackedScene m_lightMaskPackedScene;

	private int m_tileWidth = 16;
	private int m_tileHeight = 16;

	private int m_tileMapWidth = 32;
	private int m_tileMapHeight = 27;

	private LightMask[,] m_lightMasksArray;
	private List<LightMask> m_lightMasksList;

	private List<Bullet> m_bulletsList;
	private List<Grenade> m_grenadesList;

	[Export] private bool m_isUsingKeyboardAndMouse = true;
	[Export] private bool m_isUsingController = false;

	#endregion // Fields



	#region Godot methods

	public override void _EnterTree ()
	{
		node_tileMap = GetNode<TileMap>("TileMap");

		node_player = GetNode<Player>("Player");

		node_bullets = GetNode<Node2D>("Bullets");
		node_grenades = GetNode<Node2D>("Grenades");

		node_lightMasks = GetNode<Node2D>("LightMasks");

		node_bulletSpawnParticles = GetNode<Node2D>("BulletSpawnParticles");
		node_grenadeSpawnParticles = GetNode<Node2D>("GrenadeSpawnParticles");

		node_bulletDieParticles = GetNode<Node2D>("BulletDieParticles");
		node_grenadeDieParticles = GetNode<Node2D>("GrenadeDieParticles");

		m_lightMasksArray = new LightMask[m_tileMapHeight, m_tileMapWidth];
		m_lightMasksList = new List<LightMask>();

		m_bulletsList = new List<Bullet>();
		m_grenadesList = new List<Grenade>();
	}

	public override void _Ready ()
	{
		PopulateLightMasks();
	}

	public override void _Input (InputEvent @event)
	{
		if (@event is InputEventKey)
		{
			m_isUsingKeyboardAndMouse = true;
			m_isUsingController = false;
		}
		else if (@event is InputEventJoypadButton || @event is InputEventJoypadMotion)
		{
			m_isUsingKeyboardAndMouse = false;
			m_isUsingController = true;
		}
	}

	public override void _PhysicsProcess (float delta)
	{
		TickPlayer(delta);
		TickBullets(delta);
		TickGrenades(delta);
	}

	public override void _Process (float delta)
	{
		if (Input.IsPhysicalKeyPressed((int)KeyList.Key1))
		{
			node_player.SetPaintColor(EPaintColor.White);
		}
		else if (Input.IsPhysicalKeyPressed((int)KeyList.Key2))
		{
			node_player.SetPaintColor(EPaintColor.Red);
		}
		else if (Input.IsPhysicalKeyPressed((int)KeyList.Key3))
		{
			node_player.SetPaintColor(EPaintColor.Yellow);
		}
		else if (Input.IsPhysicalKeyPressed((int)KeyList.Key4))
		{
			node_player.SetPaintColor(EPaintColor.Green);
		}
		else if (Input.IsPhysicalKeyPressed((int)KeyList.Key5))
		{
			node_player.SetPaintColor(EPaintColor.Aqua);
		}
		else if (Input.IsPhysicalKeyPressed((int)KeyList.Key6))
		{
			node_player.SetPaintColor(EPaintColor.Blue);
		}
		else if (Input.IsPhysicalKeyPressed((int)KeyList.Key7))
		{
			node_player.SetPaintColor(EPaintColor.Fuchsia);
		}
	}

	#endregion // Godot methods



	#region Private methods

	private void PopulateLightMasks ()
	{
		for (int y = 0; y < m_tileMapHeight; y++)
		{
			for (int x = 0; x < m_tileMapWidth; x++)
			{
				if (node_tileMap.GetCell(x, y) != -1)
				{
					LightMask lightMask = m_lightMaskPackedScene.Instance<LightMask>();
					node_lightMasks.AddChild(lightMask);
					Vector2 position = Vector2.Zero;
					position.x = (x * m_tileWidth) + (m_tileWidth / 2f);
					position.y = (y * m_tileHeight) + (m_tileHeight / 2f);
					lightMask.Position = position;
					lightMask.Color = new Color(1, 1, 1, 1);

					m_lightMasksArray[y, x] = lightMask;
					m_lightMasksList.Add(lightMask);
				}
			}
		}
	}

	public void SetLightMaskColor (int x, int y, EPaintColor paintColor)
	{
		Color color = PaintColors.GetColorFromPaintColor(paintColor);
		SetLightMaskColor(x, y, color);
	}

	public void SetLightMaskColor (int x, int y, Color color)
	{
		m_lightMasksArray[y, x].Color = color;
	}

	public void SetLightMaskColor (LightMask lightMask, EPaintColor paintColor)
	{
		Color color = PaintColors.GetColorFromPaintColor(paintColor);
		SetLightMaskColor(lightMask, color);
	}

	public void SetLightMaskColor (LightMask lightMask, Color color)
	{
		lightMask.Color = color;
	}

	private void TickPlayer (float delta)
	{
		node_player.WasMoveLeftPressed = node_player.IsMoveLeftPressed;
		node_player.WasMoveRightPressed = node_player.IsMoveRightPressed;
		node_player.WasJumpPressed = node_player.IsJumpPressed;
		node_player.WasShootPressed = node_player.IsShootPressed;
		node_player.WasThrowGrenadePressed = node_player.IsThrowGrenadePressed;

		node_player.WasMoveLeftReleased = node_player.IsMoveLeftReleased;
		node_player.WasMoveRightReleased = node_player.IsMoveRightReleased;
		node_player.WasJumpReleased = node_player.IsJumpReleased;
		node_player.WasShootReleased = node_player.IsShootReleased;
		node_player.WasThrowGrenadeReleased = node_player.IsThrowGrenadeReleased;

		node_player.IsMoveLeftPressed = false;
		node_player.IsMoveRightPressed = false;
		node_player.IsJumpPressed = false;
		node_player.IsShootPressed = false;
		node_player.IsThrowGrenadePressed = false;

		node_player.IsMoveLeftReleased = false;
		node_player.IsMoveRightReleased = false;
		node_player.IsJumpReleased = false;
		node_player.IsShootReleased = false;
		node_player.IsThrowGrenadeReleased = false;

		if (Input.IsActionPressed("PlayerMoveLeft"))
		{
			node_player.IsMoveLeftPressed = true;
		}
		else
		{
			if (node_player.WasMoveLeftPressed)
			{
				node_player.IsMoveLeftReleased = true;
			}
		}
		if (Input.IsActionPressed("PlayerMoveRight"))
		{
			node_player.IsMoveRightPressed = true;
		}
		else
		{
			if (node_player.WasMoveRightPressed)
			{
				node_player.IsMoveRightReleased = true;
			}
		}

		if (Input.IsActionPressed("PlayerJump"))
		{
			node_player.IsJumpPressed = true;
		}
		else
		{
			if (node_player.WasJumpPressed)
			{
				node_player.IsJumpReleased = true;
			}
		}

		if (Input.IsActionPressed("PlayerShoot"))
		{
			node_player.IsShootPressed = true;
		}
		else
		{
			if (node_player.WasShootPressed)
			{
				node_player.IsShootReleased = true;
			}
		}

		if (Input.IsActionPressed("PlayerThrowGrenade"))
		{
			node_player.IsThrowGrenadePressed = true;
		}
		else
		{
			if (node_player.WasThrowGrenadePressed)
			{
				node_player.IsThrowGrenadeReleased = true;
			}
		}

		Vector2 playerVelocity = node_player.Velocity;

		if (node_player.IsMoveLeftPressed && !node_player.IsMoveRightPressed)
		{
			playerVelocity.x -= node_player.MoveAcceleration * delta;
		}
		else if (!node_player.IsMoveLeftPressed && node_player.IsMoveRightPressed)
		{
			playerVelocity.x += node_player.MoveAcceleration * delta;
		}
		else if ((node_player.IsMoveLeftPressed && node_player.IsMoveRightPressed) || (!node_player.IsMoveLeftPressed && !node_player.IsMoveRightPressed))
		{
			if (playerVelocity.x < 0)
			{
				playerVelocity.x += node_player.MoveDeceleration * delta;
				if (playerVelocity.x > 0)
				{
					playerVelocity.x = 0;
				}
			}
			else if (playerVelocity.x > 0)
			{
				playerVelocity.x -= node_player.MoveDeceleration * delta;
				if (playerVelocity.x < 0)
				{
					playerVelocity.x = 0;
				}
			}
		}

		playerVelocity.x = Mathf.Clamp(playerVelocity.x, -node_player.MaxMoveSpeed, node_player.MaxMoveSpeed);

		playerVelocity.y += node_player.FallAcceleration * delta;

		if (node_player.IsJumpPressed && !node_player.WasJumpPressed)
		{
			if (node_player.JumpCount < node_player.JumpLimit || node_player.IsInfiniteJumps)
			{
				node_player.JumpCount++;
				if (playerVelocity.y >= 0)
				{
					playerVelocity.y = -Mathf.Sqrt(2 * node_player.JumpHeight * node_player.FallAcceleration);
				}
				else
				{
					playerVelocity.y += -Mathf.Sqrt(2 * node_player.JumpHeight * node_player.FallAcceleration);
				}
				node_player.IsJumping = true;
			}
		}
		else if (!node_player.IsJumpPressed)
		{
			if (playerVelocity.y < 0)
			{
				playerVelocity.y += node_player.FallAcceleration * 1 * delta;
			}
		}

		node_player.Velocity = node_player.MoveAndSlide(playerVelocity, Vector2.Up);

		if (node_player.IsOnFloor())
		{
			node_player.IsOnGround = true;
			node_player.IsJumping = false;
			node_player.IsFalling = false;
		}
		else if (node_player.IsOnCeiling())
		{
			node_player.IsOnGround = false;
			node_player.IsJumping = false;
			node_player.IsFalling = true;
		}
		else
		{
			node_player.IsOnGround = false;
			node_player.IsJumping = false;
			node_player.IsFalling = true;
		}

		if (node_player.IsOnGround)
		{
			node_player.JumpCount = 0;
		}

		if (m_isUsingKeyboardAndMouse)
		{
			Vector2 mousePosition = GetGlobalMousePosition();
			Vector2 diff = mousePosition - node_player.node_projectileSpawnPosition.GlobalPosition;
			diff = diff.Normalized();
			node_player.AimingDirection = diff;
		}
		else if (m_isUsingController)
		{
			Vector2 rightAxisDirection = GetGlobalMousePosition();
			Vector2 target = node_player.node_projectileSpawnPosition.GlobalPosition + rightAxisDirection;
			Vector2 diff = target - node_player.node_projectileSpawnPosition.GlobalPosition;
			diff = diff.Normalized();
			node_player.AimingDirection = diff;
		}

		if (node_player.AimingDirection.x < 0)
		{
			node_player.FacingDirection = Player.EFacingDirection.Left;
			node_player.node_sprite.FlipH = true;
		}
		else if (node_player.AimingDirection.x > 0)
		{
			node_player.FacingDirection = Player.EFacingDirection.Right;
			node_player.node_sprite.FlipH = false;
		}

		if (node_player.HasPaintShoes && node_player.IsOnGround)
		{
			float x = node_player.Position.x;
			float y = node_player.Position.y;

			if (y <= (m_tileHeight * m_tileMapHeight) - m_tileHeight)
			{
				for (int i = -1; i < 2; i++)
				{
					int tileX = Mathf.FloorToInt((x + (i * 4)) / m_tileWidth);
					int tileY = Mathf.FloorToInt(y / m_tileHeight) + 1;

					if (node_tileMap.GetCell(tileX, tileY) != -1)
					{
						SetLightMaskColor(tileX, tileY, node_player.PaintColor);
					}
				}
			}
		}

		if (node_player.IsShootPressed && !node_player.WasShootPressed)
		{
			Vector2 position = node_player.node_projectileSpawnPosition.GlobalPosition;
			Vector2 velocity = node_player.AimingDirection * 128f;

			EPaintColor paintColor = node_player.PaintColor;
			SpawnBullet(position, velocity, paintColor);
		}

		if (node_player.IsThrowGrenadePressed)
		{
			if (node_player.GrenadeThrowTimer < node_player.GrenadeThrowTimeLimit)
			{
				node_player.GrenadeThrowTimer += delta;
				if (node_player.GrenadeThrowTimer > node_player.GrenadeThrowTimeLimit)
				{
					node_player.GrenadeThrowTimer = node_player.GrenadeThrowTimeLimit;
				}
			}
		}

		if (!node_player.IsThrowGrenadePressed && node_player.WasThrowGrenadePressed)
		{
			if (node_player.NumberOfGrenades > 0 || node_player.IsInfiniteGrenades)
			{
				node_player.NumberOfGrenades--;

				if (node_player.NumberOfGrenades < 0)
				{
					node_player.NumberOfGrenades = 0;
				}

				Vector2 position = node_player.node_projectileSpawnPosition.GlobalPosition;
				Vector2 velocity = Vector2.Zero;

				float per = node_player.GrenadeThrowTimer / node_player.GrenadeThrowTimeLimit;

				float minStrength = 64f;
				float maxStrength = 512f;
				float strength = Mathf.Lerp(minStrength, maxStrength, per);

				velocity = node_player.AimingDirection * strength;

				EPaintColor paintColor = node_player.PaintColor;
				SpawnGrenade(position, velocity, paintColor);

				node_player.GrenadeThrowTimer = 0f;
			}
		}
	}

	private void TickBullets (float delta)
	{
		for (int i = 0; i < m_bulletsList.Count; i++)
		{
			Bullet bullet = m_bulletsList[i];

			bullet.LifetimeTimer += delta;

			if (bullet.LifetimeTimer < bullet.Lifetime)
			{
				KinematicCollision2D collision = bullet.MoveAndCollide(bullet.Velocity * delta);

				if (collision != null)
				{
					CauseExplosion(collision.Position, bullet.ExplosionRadius, bullet.PaintColor);

					bullet.QueueFree();
					node_bullets.RemoveChild(bullet);
					m_bulletsList.RemoveAt(i);
					i--;
				}
			}
			else
			{
				CauseExplosion(bullet.Position, bullet.ExplosionRadius, bullet.PaintColor);

				bullet.QueueFree();
				node_bullets.RemoveChild(bullet);
				m_bulletsList.RemoveAt(i);
				i--;
			}
		}
	}

	private void TickGrenades (float delta)
	{
		for (int i = 0; i < m_grenadesList.Count; i++)
		{
			Grenade grenade = m_grenadesList[i];

			grenade.LifetimeTimer += delta;

			if (grenade.LifetimeTimer < grenade.Lifetime)
			{
				Vector2 velocity = grenade.Velocity;
				float deceleration = 64f;
				float gravity = 256f;

				if (velocity.x < 0)
				{
					velocity.x += deceleration * delta;
					if (velocity.x > 0)
					{
						velocity.x = 0;
					}
				}
				else if (velocity.x > 0)
				{
					velocity.x -= deceleration * delta;
					if (velocity.x < 0)
					{
						velocity.x = 0;
					}
				}

				velocity.y += gravity * delta;

				KinematicCollision2D collision = grenade.MoveAndCollide(velocity * delta);

				if (collision != null)
				{
					Vector2 normal = collision.Normal;

					if (normal.x != 0)
					{
						velocity.x *= -1;
					}
					if (normal.y != 0)
					{
						velocity.y /= 2f;
						velocity.y *= -1;
					}
				}

				grenade.Velocity = velocity;
			}
			else
			{
				CauseExplosion(grenade.Position, grenade.ExplosionRadius, grenade.PaintColor);

				grenade.QueueFree();
				node_grenades.RemoveChild(grenade);
				m_grenadesList.RemoveAt(i);
				i--;
			}
		}
	}

	private Bullet SpawnBullet (Vector2 position, Vector2 velocity, EPaintColor paintColor)
	{
		Bullet bullet = m_bulletPackedScene.Instance<Bullet>();
		node_bullets.AddChild(bullet);
		m_bulletsList.Add(bullet);

		bullet.Position = position;
		bullet.Velocity = velocity;
		bullet.SetPaintColor(paintColor);

		return bullet;
	}

	private Grenade SpawnGrenade (Vector2 position, Vector2 velocity, EPaintColor paintColor)
	{
		Grenade grenade = m_grenadePackedScene.Instance<Grenade>();
		node_grenades.AddChild(grenade);
		m_grenadesList.Add(grenade);

		grenade.Position = position;
		grenade.Velocity = velocity;
		grenade.SetPaintColor(paintColor);

		return grenade;
	}

	private void CauseExplosion (Vector2 position, float radius, EPaintColor paintColor)
	{
		Physics2DDirectSpaceState state = GetWorld2d().DirectSpaceState;

		Physics2DShapeQueryParameters foo = new Physics2DShapeQueryParameters();
		CircleShape2D shape = new CircleShape2D();
		shape.Radius = radius;
		foo.SetShape(shape);
		foo.Transform = new Transform2D(0f, position);
		foo.CollisionLayer = 0 | (1 << 3);
		foo.CollideWithAreas = true;
		foo.CollideWithBodies = false;

		Godot.Collections.Array results = state.IntersectShape(foo, 256);

		GD.Print(results.Count);

		for (int i = 0; i < results.Count; i++)
		{
			object result = results[i];
			Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)result;

			if (dict.Contains("collider") && dict["collider"] is Area2D area)
			{
				if (area.GetParent() != null)
				{
					if (area.GetParent() is LightMask lightMask)
					{
						SetLightMaskColor(lightMask, paintColor);
					}
				}
			}
		}
	}

	#endregion // Private methods

}
