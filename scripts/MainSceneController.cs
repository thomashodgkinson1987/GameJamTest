using System.Collections.Generic;
using Godot;
using static PaintColors;

public class MainSceneController : Node2D
{

	#region Enums

	public enum ESound
	{
		PlayerJump, PlayerLand,
		StandardGunShoot, GrenadeExplode,
		RifleShoot, MachineGunShoot, SniperRifleShoot
	}

	#endregion // Enums



	#region Nodes

	private Node node_oneShotSounds;

	private TileMap node_tileMap;

	private Node2D node_lightMasks;

	private Node2D node_playerLandParticles;
	private Node2D node_playerJumpParticles;

	private Player node_player;

	private Node2D node_bullets;
	private Node2D node_grenades;

	private Node2D node_bulletSpawnParticles;
	private Node2D node_grenadeSpawnParticles;

	private Node2D node_bulletDieParticles;
	private Node2D node_grenadeDieParticles;

	// Player 1 UI

	private Control node_player1UI;

	private TextureRect node_player1UI_playerIcon;

	private TextureProgress node_player1UI_powerGauge;

	private Control node_player1UI_grenadeUI;
	private TextureRect node_player1UI_grenadeIcon;
	private Label node_player1UI_grenadeCount;

	private Control node_player1UI_rifleUI;
	private TextureRect node_player1UI_rifleIcon;
	private Label node_player1UI_rifleBulletCount;

	private Control node_player1UI_machineGunUI;
	private TextureRect node_player1UI_machineGunIcon;
	private Label node_player1UI_machineGunBulletCount;

	private Control node_player1UI_sniperRifleUI;
	private TextureRect node_player1UI_sniperRifleIcon;
	private Label node_player1UI_sniperRifleBulletCount;

	private Control node_player1UI_paintGlovesUI;
	private TextureRect node_player1UI_paintGlovesIcon;

	private Control node_player1UI_paintShoesUI;
	private TextureRect node_player1UI_paintShoesIcon;

	private Control node_player1UI_bigBadBootiesUI;
	private TextureRect node_player1UI_bigBadBootiesIcon;

	// Level data UI

	private Control node_levelDataUI;

	private Label node_levelDataUI_whiteTileCountLabel;
	private Label node_levelDataUI_redTileCountLabel;
	private Label node_levelDataUI_yellowTileCountLabel;
	private Label node_levelDataUI_greenTileCountLabel;
	private Label node_levelDataUI_aquaTileCountLabel;
	private Label node_levelDataUI_blueTileCountLabel;
	private Label node_levelDataUI_fuchsiaTileCountLabel;

	#endregion // Nodes



	#region Fields

	[Export] private PackedScene m_standardBulletPackedScene;
	[Export] private PackedScene m_grenadePackedScene;
	[Export] private PackedScene m_rifleBulletPackedScene;
	[Export] private PackedScene m_machineGunBulletPackedScene;
	[Export] private PackedScene m_sniperRifleBulletPackedScene;
	[Export] private PackedScene m_lightMaskPackedScene;

	[Export] private PackedScene m_playerJumpOneShotSoundPackedScene;
	[Export] private PackedScene m_playerLandOneShotSoundPackedScene;

	[Export] private PackedScene m_standardGunShootOneShotSoundPackedScene;
	[Export] private PackedScene m_grenadeExplodeOneShotSoundPackedScene;
	[Export] private PackedScene m_rifleShootOneShotSoundPackedScene;
	[Export] private PackedScene m_machineGunShootOneShotSoundPackedScene;
	[Export] private PackedScene m_sniperRifleShootOneShotSoundPackedScene;

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

	private int m_whiteTileCount;
	private int m_redTileCount;
	private int m_yellowTileCount;
	private int m_greenTileCount;
	private int m_aquaTileCount;
	private int m_blueTileCount;
	private int m_fuchsiaTileCount;

	#endregion // Fields



	#region Godot methods

	public override void _EnterTree ()
	{
		node_oneShotSounds = GetNode<Node>("OneShotSounds");

		node_tileMap = GetNode<TileMap>("TileMap");

		node_lightMasks = GetNode<Node2D>("LightMasks");

		node_playerLandParticles = GetNode<Node2D>("PlayerLandParticles");
		node_playerJumpParticles = GetNode<Node2D>("PlayerJumpParticles");

		node_player = GetNode<Player>("Player");

		node_bullets = GetNode<Node2D>("Bullets");
		node_grenades = GetNode<Node2D>("Grenades");

		node_bulletSpawnParticles = GetNode<Node2D>("BulletSpawnParticles");
		node_grenadeSpawnParticles = GetNode<Node2D>("GrenadeSpawnParticles");

		node_bulletDieParticles = GetNode<Node2D>("BulletDieParticles");
		node_grenadeDieParticles = GetNode<Node2D>("GrenadeDieParticles");

		// Player 1 UI

		node_player1UI = GetNode<Control>("UI/Player1UI");

		node_player1UI_playerIcon = GetNode<TextureRect>("UI/Player1UI/PlayerIcon");

		node_player1UI_powerGauge = GetNode<TextureProgress>("UI/Player1UI/PowerGaugeUI/PowerGaugeContainer/PowerGauge");

		node_player1UI_grenadeUI = GetNode<Control>("UI/Player1UI/GrenadeUI");
		node_player1UI_grenadeIcon = GetNode<TextureRect>("UI/Player1UI/GrenadeUI/GrenadeIcon");
		node_player1UI_grenadeCount = GetNode<Label>("UI/Player1UI/GrenadeUI/GrenadeCountContainer/GrenadeCount");

		node_player1UI_rifleUI = GetNode<Control>("UI/Player1UI/RifleUI");
		node_player1UI_rifleIcon = GetNode<TextureRect>("UI/Player1UI/RifleUI/RifleIcon");
		node_player1UI_rifleBulletCount = GetNode<Label>("UI/Player1UI/RifleUI/RifleBulletCountContainer/RifleBulletCount");

		node_player1UI_machineGunUI = GetNode<Control>("UI/Player1UI/MachineGunUI");
		node_player1UI_machineGunIcon = GetNode<TextureRect>("UI/Player1UI/MachineGunUI/MachineGunIcon");
		node_player1UI_machineGunBulletCount = GetNode<Label>("UI/Player1UI/MachineGunUI/MachineGunBulletCountContainer/MachineGunBulletCount");

		node_player1UI_sniperRifleUI = GetNode<Control>("UI/Player1UI/SniperRifleUI");
		node_player1UI_sniperRifleIcon = GetNode<TextureRect>("UI/Player1UI/SniperRifleUI/SniperRifleIcon");
		node_player1UI_sniperRifleBulletCount = GetNode<Label>("UI/Player1UI/SniperRifleUI/SniperRifleBulletCountContainer/SniperRifleBulletCount");

		node_player1UI_paintGlovesUI = GetNode<Control>("UI/Player1UI/PaintGlovesUI");
		node_player1UI_paintGlovesIcon = GetNode<TextureRect>("UI/Player1UI/PaintGlovesUI/PaintGlovesIcon");

		node_player1UI_paintShoesUI = GetNode<Control>("UI/Player1UI/PaintShoesUI");
		node_player1UI_paintShoesIcon = GetNode<TextureRect>("UI/Player1UI/PaintShoesUI/PaintShoesIcon");

		node_player1UI_bigBadBootiesUI = GetNode<Control>("UI/Player1UI/BigBadBootiesUI");
		node_player1UI_bigBadBootiesIcon = GetNode<TextureRect>("UI/Player1UI/BigBadBootiesUI/BigBadBootiesIcon");

		// Level data UI

		node_levelDataUI = GetNode<Control>("UI/LevelDataUI");

		node_levelDataUI_whiteTileCountLabel = GetNode<Label>("UI/LevelDataUI/WhiteTileDataUI/TileCount");
		node_levelDataUI_redTileCountLabel = GetNode<Label>("UI/LevelDataUI/RedTileDataUI/TileCount");
		node_levelDataUI_yellowTileCountLabel = GetNode<Label>("UI/LevelDataUI/YellowTileDataUI/TileCount");
		node_levelDataUI_greenTileCountLabel = GetNode<Label>("UI/LevelDataUI/GreenTileDataUI/TileCount");
		node_levelDataUI_aquaTileCountLabel = GetNode<Label>("UI/LevelDataUI/AquaTileDataUI/TileCount");
		node_levelDataUI_blueTileCountLabel = GetNode<Label>("UI/LevelDataUI/BlueTileDataUI/TileCount");
		node_levelDataUI_fuchsiaTileCountLabel = GetNode<Label>("UI/LevelDataUI/FuchsiaTileDataUI/TileCount");
	}

	public override void _Ready ()
	{
		m_lightMasksArray = new LightMask[m_tileMapHeight, m_tileMapWidth];
		m_lightMasksList = new List<LightMask>();

		m_bulletsList = new List<Bullet>();
		m_grenadesList = new List<Grenade>();

		PopulateLightMasks();
		UpdateUI();
	}

	public override void _Input (InputEvent @event)
	{
		if (@event is InputEventKey || @event is InputEventMouse)
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

		if (Input.IsPhysicalKeyPressed((int)KeyList.Q))
		{
			node_player.GunEquipSlot = Player.EGunEquipSlot.None;
		}
		else if (Input.IsPhysicalKeyPressed((int)KeyList.W))
		{
			node_player.GunEquipSlot = Player.EGunEquipSlot.Rifle;
		}
		else if (Input.IsPhysicalKeyPressed((int)KeyList.E))
		{
			node_player.GunEquipSlot = Player.EGunEquipSlot.MachineGun;
		}
		else if (Input.IsPhysicalKeyPressed((int)KeyList.R))
		{
			node_player.GunEquipSlot = Player.EGunEquipSlot.SniperRifle;
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
					lightMask.Position = new Vector2()
					{
						x = (x * m_tileWidth) + (m_tileWidth / 2f),
						y = (y * m_tileHeight) + (m_tileHeight / 2f)
					};
					lightMask.Color = new Color(1, 1, 1, 1);

					m_lightMasksArray[y, x] = lightMask;
					m_lightMasksList.Add(lightMask);

					m_whiteTileCount++;
				}
			}
		}
	}

	private void UpdateUI ()
	{
		UpdatePlayer1UI();
		UpdateLevelDataUI();
	}
	private void UpdatePlayer1UI ()
	{
		node_player1UI_playerIcon.SelfModulate = PaintColors.GetColorFromPaintColor(node_player.PaintColor);

		node_player1UI_powerGauge.SelfModulate = PaintColors.GetColorFromPaintColor(node_player.PowerGaugePaintColor);
		node_player1UI_powerGauge.Value = node_player.PowerGaugeCurrent;

		node_player1UI_grenadeIcon.SelfModulate = PaintColors.GetColorFromPaintColor(node_player.GrenadePaintColor);
		node_player1UI_grenadeCount.Text = node_player.GrenadesCount.ToString();

		node_player1UI_rifleUI.Visible = node_player.GunEquipSlot == Player.EGunEquipSlot.Rifle;
		node_player1UI_rifleIcon.SelfModulate = PaintColors.GetColorFromPaintColor(node_player.RiflePaintColor);
		node_player1UI_rifleBulletCount.Text = node_player.RifleBulletsCount.ToString();

		node_player1UI_machineGunUI.Visible = node_player.GunEquipSlot == Player.EGunEquipSlot.MachineGun;
		node_player1UI_machineGunIcon.SelfModulate = PaintColors.GetColorFromPaintColor(node_player.MachineGunPaintColor);
		node_player1UI_machineGunBulletCount.Text = node_player.MachineGunBulletsCount.ToString();

		node_player1UI_sniperRifleUI.Visible = node_player.GunEquipSlot == Player.EGunEquipSlot.SniperRifle;
		node_player1UI_sniperRifleIcon.SelfModulate = PaintColors.GetColorFromPaintColor(node_player.SniperRiflePaintColor);
		node_player1UI_sniperRifleBulletCount.Text = node_player.SniperRifleBulletsCount.ToString();

		node_player1UI_paintGlovesUI.Visible = node_player.HasPaintGloves;
		node_player1UI_paintGlovesIcon.SelfModulate = PaintColors.GetColorFromPaintColor(node_player.PaintGlovesPaintColor);

		node_player1UI_paintShoesUI.Visible = node_player.HasPaintShoes;
		node_player1UI_paintShoesIcon.SelfModulate = PaintColors.GetColorFromPaintColor(node_player.PaintShoesPaintColor);

		node_player1UI_bigBadBootiesUI.Visible = node_player.HasBigBadBooties;
		node_player1UI_bigBadBootiesIcon.SelfModulate = PaintColors.GetColorFromPaintColor(node_player.BigBadBootiesPaintColor);
	}
	private void UpdateLevelDataUI ()
	{
		node_levelDataUI_whiteTileCountLabel.Text = m_whiteTileCount.ToString();
		node_levelDataUI_redTileCountLabel.Text = m_redTileCount.ToString();
		node_levelDataUI_yellowTileCountLabel.Text = m_yellowTileCount.ToString();
		node_levelDataUI_greenTileCountLabel.Text = m_greenTileCount.ToString();
		node_levelDataUI_aquaTileCountLabel.Text = m_aquaTileCount.ToString();
		node_levelDataUI_blueTileCountLabel.Text = m_blueTileCount.ToString();
		node_levelDataUI_fuchsiaTileCountLabel.Text = m_fuchsiaTileCount.ToString();
	}

	private void SetLightMaskColor (int x, int y, EPaintColor paintColor)
	{
		Color color = PaintColors.GetColorFromPaintColor(paintColor);
		SetLightMaskColor(x, y, color);
	}
	private void SetLightMaskColor (int x, int y, Color color)
	{
		m_lightMasksArray[y, x].Color = color;
	}
	private void SetLightMaskColor (LightMask lightMask, EPaintColor paintColor)
	{
		Color color = PaintColors.GetColorFromPaintColor(paintColor);
		SetLightMaskColor(lightMask, color);
	}
	private void SetLightMaskColor (LightMask lightMask, Color color)
	{
		lightMask.Color = color;
	}

	private void TickPlayer (float delta)
	{
		node_player.WasMoveLeftPressed = node_player.IsMoveLeftPressed;
		node_player.WasMoveRightPressed = node_player.IsMoveRightPressed;
		node_player.WasDownPressed = node_player.IsDownPressed;
		node_player.WasJumpPressed = node_player.IsJumpPressed;
		node_player.WasShootPressed = node_player.IsShootPressed;
		node_player.WasThrowGrenadePressed = node_player.IsThrowGrenadePressed;

		node_player.WasMoveLeftReleased = node_player.IsMoveLeftReleased;
		node_player.WasMoveRightReleased = node_player.IsMoveRightReleased;
		node_player.WasDownReleased = node_player.IsDownReleased;
		node_player.WasJumpReleased = node_player.IsJumpReleased;
		node_player.WasShootReleased = node_player.IsShootReleased;
		node_player.WasThrowGrenadeReleased = node_player.IsThrowGrenadeReleased;

		node_player.IsMoveLeftPressed = false;
		node_player.IsMoveRightPressed = false;
		node_player.IsDownPressed = false;
		node_player.IsJumpPressed = false;
		node_player.IsShootPressed = false;
		node_player.IsThrowGrenadePressed = false;

		node_player.IsMoveLeftReleased = false;
		node_player.IsMoveRightReleased = false;
		node_player.IsDownReleased = false;
		node_player.IsJumpReleased = false;
		node_player.IsShootReleased = false;
		node_player.IsThrowGrenadeReleased = false;

		node_player.WasOnGround = node_player.IsOnGround;
		node_player.WasOnWallLeft = node_player.IsOnWallLeft;
		node_player.WasOnWallRight = node_player.IsOnWallRight;

		node_player.IsOnGround = false;
		node_player.IsOnWallLeft = false;
		node_player.IsOnWallRight = false;

		if (Input.IsActionPressed("PlayerMoveLeft"))
			node_player.IsMoveLeftPressed = true;
		else if (node_player.WasMoveLeftPressed)
			node_player.IsMoveLeftReleased = true;

		if (Input.IsActionPressed("PlayerMoveRight"))
			node_player.IsMoveRightPressed = true;
		else if (node_player.WasMoveRightPressed)
			node_player.IsMoveRightReleased = true;

		if (Input.IsActionPressed("PlayerDown"))
			node_player.IsDownPressed = true;
		else if (node_player.WasDownPressed)
			node_player.IsDownReleased = true;

		if (Input.IsActionPressed("PlayerJump"))
			node_player.IsJumpPressed = true;
		else if (node_player.WasJumpPressed)
			node_player.IsJumpReleased = true;

		if (Input.IsActionPressed("PlayerShoot"))
			node_player.IsShootPressed = true;
		else if (node_player.WasShootPressed)
			node_player.IsShootReleased = true;

		if (Input.IsActionPressed("PlayerThrowGrenade"))
			node_player.IsThrowGrenadePressed = true;
		else if (node_player.WasThrowGrenadePressed)
			node_player.IsThrowGrenadeReleased = true;

		Vector2 playerVelocity = node_player.Velocity;

		if (node_player.IsMoveLeftPressed && !node_player.IsMoveRightPressed)
		{
			if (node_player.WasOnWallRight)
			{
				node_player.WallHugTimeTimer += delta;
				if (node_player.WallHugTimeTimer >= node_player.WallHugTime)
				{
					node_player.WallHugTimeTimer = 0f;
					playerVelocity.x -= node_player.MoveAcceleration * delta;
				}
			}
			else if (playerVelocity.x > -node_player.MaxMoveSpeed)
			{
				playerVelocity.x -= node_player.MoveAcceleration * delta;
			}
		}
		else if (!node_player.IsMoveLeftPressed && node_player.IsMoveRightPressed)
		{
			if (node_player.WasOnWallLeft)
			{
				node_player.WallHugTimeTimer += delta;
				if (node_player.WallHugTimeTimer >= node_player.WallHugTime)
				{
					node_player.WallHugTimeTimer = 0f;
					playerVelocity.x += node_player.MoveAcceleration * delta;
				}
			}
			else if (playerVelocity.x < node_player.MaxMoveSpeed)
			{
				playerVelocity.x += node_player.MoveAcceleration * delta;
			}
		}
		else if ((node_player.IsMoveLeftPressed && node_player.IsMoveRightPressed) || (!node_player.IsMoveLeftPressed && !node_player.IsMoveRightPressed))
		{
			if (playerVelocity.x != 0)
			{
				int oldDirection = playerVelocity.x < 0 ? -1 : 1;
				playerVelocity.x += node_player.MoveDeceleration * -oldDirection * delta;
				int newDirection = playerVelocity.x < 0 ? -1 : playerVelocity.x > 0 ? 1 : 0;
				if (oldDirection != newDirection)
				{
					playerVelocity.x = 0;
				}
			}
		}

		//playerVelocity.x = Mathf.Clamp(playerVelocity.x, -node_player.MaxMoveSpeed, node_player.MaxMoveSpeed);

		if (!node_player.IsDownPressed)
		{
			bool wasOnWall = node_player.WasOnWallLeft || node_player.WasOnWallRight;
			float fallAcceleration = wasOnWall ? node_player.WallSlideFallAcceleration : node_player.FallAcceleration;
			playerVelocity.y += fallAcceleration * delta;
		}
		else
		{
			bool wasOnWall = node_player.WasOnWallLeft || node_player.WasOnWallRight;
			float fallAcceleration = wasOnWall ? node_player.WallSlideFallAcceleration * 2f : node_player.HoldDownFallAcceleration;
			playerVelocity.y += fallAcceleration * delta;
		}

		if (node_player.IsJumpPressed && !node_player.WasJumpPressed)
		{
			if (node_player.JumpCount < node_player.JumpLimit || node_player.IsInfiniteJumps)
			{
				if (!node_player.IsInfiniteJumps)
					node_player.JumpCount++;

				if (!node_player.WasOnGround && (node_player.WasOnWallLeft || node_player.WasOnWallRight))
				{
					int direction = 0;

					if (node_player.WasOnWallLeft)
						direction = 1;
					else if (node_player.WasOnWallRight)
						direction = -1;

					playerVelocity.x = node_player.WallJumpDirection.x * direction * node_player.WallJumpStrength;
					playerVelocity.y = node_player.WallJumpDirection.y * node_player.WallJumpStrength;
				}
				else
				{
					if (playerVelocity.y >= 0)
					{
						playerVelocity.y = -Mathf.Sqrt(2 * node_player.FallAcceleration * node_player.JumpHeight);
					}
					else
					{
						playerVelocity.y += -Mathf.Sqrt(2 * node_player.FallAcceleration * node_player.JumpHeight);
					}
					OneShotParticles jumpParticles = node_player.JumpParticlesPackedScene.Instance<OneShotParticles>();
					node_playerJumpParticles.AddChild(jumpParticles);
					jumpParticles.Position = node_player.Position;

					Color color;

					if (node_player.HasPaintShoes)
						color = PaintColors.GetColorFromPaintColor(node_player.PaintShoesPaintColor);
					else if (node_player.HasBigBadBooties)
						color = PaintColors.GetColorFromPaintColor(node_player.BigBadBootiesPaintColor);
					else
						color = PaintColors.GetColorFromPaintColor(node_player.PaintColor);

					jumpParticles.SelfModulate = color;
				}

				PlaySound(ESound.PlayerJump);
			}
		}
		else if (!node_player.IsJumpPressed && playerVelocity.y < 0)
		{
			playerVelocity.y += node_player.FallAcceleration * 1 * delta;
		}

		node_player.Velocity = node_player.MoveAndSlide(playerVelocity, Vector2.Up);

		if (node_player.IsOnFloor())
		{
			node_player.IsOnGround = true;
		}

		if (node_player.IsOnWall())
		{
			if (!node_player.WasOnWallLeft && !node_player.WasOnWallRight)
			{
				node_player.WallHugTimeTimer = 0f;
			}
			for (int i = 0; i < node_player.GetSlideCount(); i++)
			{
				KinematicCollision2D collision = node_player.GetSlideCollision(i);
				if (collision.Normal.x > 0)
					node_player.IsOnWallLeft = true;
				else if (collision.Normal.x < 0)
					node_player.IsOnWallRight = true;
			}
		}

		if (node_player.IsOnGround)
		{
			if (!node_player.WasOnGround)
			{
				node_player.JumpCount = 0;
				PlaySound(ESound.PlayerLand);

				OneShotParticles landParticles = node_player.LandParticlesPackedScene.Instance<OneShotParticles>();
				node_playerLandParticles.AddChild(landParticles);
				landParticles.Position = node_player.Position;

				Color color;

				if (node_player.HasPaintShoes)
					color = PaintColors.GetColorFromPaintColor(node_player.PaintShoesPaintColor);
				else if (node_player.HasBigBadBooties)
					color = PaintColors.GetColorFromPaintColor(node_player.BigBadBootiesPaintColor);
				else
					color = PaintColors.GetColorFromPaintColor(node_player.PaintColor);

				landParticles.SelfModulate = color;
			}

			if (node_player.IsDownPressed && !node_player.WasDownPressed)
			{
				node_player.DisableOneWayPlatformCollision();
				node_player.DisableOneWayCollisionTimer = 0f;
			}

			if (node_player.HasPaintShoes)
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
							Color oldColor = m_lightMasksArray[tileY, tileX].Color;
							Color newColor = PaintColors.GetColorFromPaintColor(node_player.PaintShoesPaintColor);

							if (oldColor != newColor)
							{
								if (oldColor == Colors.White)
									m_whiteTileCount--;
								else if (oldColor == Colors.Red)
									m_redTileCount--;
								else if (oldColor == Colors.Yellow)
									m_yellowTileCount--;
								else if (oldColor == Colors.Green)
									m_greenTileCount--;
								else if (oldColor == Colors.Aqua)
									m_aquaTileCount--;
								else if (oldColor == Colors.Blue)
									m_blueTileCount--;
								else if (oldColor == Colors.Fuchsia)
									m_fuchsiaTileCount--;

								if (newColor == Colors.White)
									m_whiteTileCount++;
								else if (newColor == Colors.Red)
									m_redTileCount++;
								else if (newColor == Colors.Yellow)
									m_yellowTileCount++;
								else if (newColor == Colors.Green)
									m_greenTileCount++;
								else if (newColor == Colors.Aqua)
									m_aquaTileCount++;
								else if (newColor == Colors.Blue)
									m_blueTileCount++;
								else if (newColor == Colors.Fuchsia)
									m_fuchsiaTileCount++;

								SetLightMaskColor(tileX, tileY, node_player.PaintShoesPaintColor);
							}
						}
					}
				}
			}

			node_player.node_animatedSprite.Play(node_player.Velocity.x != 0 ? "walk" : "idle");
		}
		else
		{
			node_player.node_animatedSprite.Play("jump");
		}

		if (node_player.DisableOneWayCollisionTimer < node_player.DisableOneWayCollisionTime)
		{
			node_player.DisableOneWayCollisionTimer += delta;
			if (node_player.DisableOneWayCollisionTimer >= node_player.DisableOneWayCollisionTime)
			{
				node_player.DisableOneWayCollisionTimer = node_player.DisableOneWayCollisionTime;
				node_player.EnableOneWayPlatformCollision();
			}
		}

		if (m_isUsingKeyboardAndMouse)
		{
			Vector2 diff = (GetGlobalMousePosition() - node_player.node_projectileSpawnPosition.GlobalPosition).Normalized();
			if (diff.x != 0 || diff.y != 0)
			{
				node_player.AimingDirection = diff;
			}
		}
		else if (m_isUsingController)
		{
			Vector2 inputDirection = Input.GetVector("PlayerAimLeft", "PlayerAimRight", "PlayerAimUp", "PlayerAimDown");
			Vector2 target = node_player.node_projectileSpawnPosition.GlobalPosition + inputDirection;
			Vector2 diff = (target - node_player.node_projectileSpawnPosition.GlobalPosition).Normalized();
			if (diff.x != 0 || diff.y != 0)
			{
				node_player.AimingDirection = diff;
			}
		}

		node_player.SetAimingLineDirection(node_player.AimingDirection);

		if (node_player.AimingDirection.x < 0)
			node_player.node_animatedSprite.FlipH = true;
		else if (node_player.AimingDirection.x > 0)
			node_player.node_animatedSprite.FlipH = false;

		if (node_player.PowerGaugeCurrent < node_player.PowerGaugeLimit)
		{
			node_player.PowerGaugeCurrent += node_player.PowerGaugeRechargeRate;
			if (node_player.PowerGaugeCurrent > node_player.PowerGaugeLimit)
			{
				node_player.PowerGaugeCurrent = node_player.PowerGaugeLimit;
			}
		}

		if (node_player.IsShootPressed && !node_player.WasShootPressed)
		{
			Vector2 position = node_player.node_projectileSpawnPosition.GlobalPosition;
			Vector2 direction = node_player.AimingDirection;

			switch (node_player.GunEquipSlot)
			{
				case Player.EGunEquipSlot.None:
					if (node_player.PowerGaugeCurrent >= node_player.StandardBulletPowerCost || node_player.IsInfinitePowerGauge)
					{
						if (!node_player.IsInfinitePowerGauge)
						{
							node_player.PowerGaugeCurrent -= node_player.StandardBulletPowerCost;
						}
						SpawnStandardBullet(position, direction, node_player.PowerGaugePaintColor);
					}
					break;
				case Player.EGunEquipSlot.Rifle:
					if (node_player.RifleBulletsCount > 0 || node_player.IsInfiniteRifleAmmunition)
					{
						if (!node_player.IsInfiniteRifleAmmunition)
						{
							node_player.RifleBulletsCount--;
							if (node_player.RifleBulletsCount == 0)
							{
								node_player.GunEquipSlot = Player.EGunEquipSlot.None;
							}
						}
						SpawnRifleBullet(position, direction, node_player.RiflePaintColor);
					}
					break;
				case Player.EGunEquipSlot.MachineGun:
					if (node_player.MachineGunBulletsCount > 0 || node_player.IsInfiniteMachineGunAmmunition)
					{
						if (!node_player.IsInfiniteMachineGunAmmunition)
						{
							node_player.MachineGunBulletsCount--;
							if (node_player.MachineGunBulletsCount == 0)
							{
								node_player.GunEquipSlot = Player.EGunEquipSlot.None;
							}
						}
						SpawnMachineGunBullet(position, direction, node_player.MachineGunPaintColor);
						node_player.MachineGunFireRateTimer = 0f;
					}
					break;
				case Player.EGunEquipSlot.SniperRifle:
					if (node_player.SniperRifleBulletsCount > 0 || node_player.IsInfiniteSniperRifleAmmunition)
					{
						if (!node_player.IsInfiniteSniperRifleAmmunition)
						{
							node_player.SniperRifleBulletsCount--;
							if (node_player.SniperRifleBulletsCount == 0)
							{
								node_player.GunEquipSlot = Player.EGunEquipSlot.None;
							}
						}
						SpawnSniperRifleBullet(position, direction, node_player.SniperRiflePaintColor);
					}
					break;
				default:
					if (node_player.PowerGaugeCurrent >= node_player.StandardBulletPowerCost || node_player.IsInfinitePowerGauge)
					{
						if (!node_player.IsInfinitePowerGauge)
						{
							node_player.PowerGaugeCurrent -= node_player.StandardBulletPowerCost;
						}
						SpawnStandardBullet(position, direction, node_player.PowerGaugePaintColor);
					}
					break;
			}
		}
		else if (node_player.IsShootPressed && node_player.WasShootPressed)
		{
			if (node_player.GunEquipSlot == Player.EGunEquipSlot.MachineGun)
			{
				node_player.MachineGunFireRateTimer += delta;
				if (node_player.MachineGunFireRateTimer >= node_player.MachineGunFireRate)
				{
					if (node_player.MachineGunBulletsCount > 0 || node_player.IsInfiniteMachineGunAmmunition)
					{
						if (!node_player.IsInfiniteMachineGunAmmunition)
						{
							node_player.MachineGunBulletsCount--;
							if (node_player.MachineGunBulletsCount == 0)
							{
								node_player.GunEquipSlot = Player.EGunEquipSlot.None;
							}
						}
						Vector2 position = node_player.node_projectileSpawnPosition.GlobalPosition;
						Vector2 direction = node_player.AimingDirection;
						SpawnMachineGunBullet(position, direction, node_player.MachineGunPaintColor);
					}
					node_player.MachineGunFireRateTimer = 0f;
				}
			}
		}
		else if (!node_player.IsShootPressed && node_player.WasShootPressed)
		{
			node_player.MachineGunFireRateTimer = 0f;
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
		else if (!node_player.IsThrowGrenadePressed && node_player.WasThrowGrenadePressed)
		{
			if (node_player.GrenadesCount > 0 || node_player.IsInfiniteGrenades)
			{
				if (!node_player.IsInfiniteGrenades)
				{
					node_player.GrenadesCount--;
				}

				float minStrength = 64f;
				float maxStrength = 512f;
				float per = node_player.GrenadeThrowTimer / node_player.GrenadeThrowTimeLimit;
				float strength = Mathf.Lerp(minStrength, maxStrength, per);

				Vector2 position = node_player.node_projectileSpawnPosition.GlobalPosition;
				Vector2 velocity = node_player.AimingDirection * strength;

				SpawnGrenade(position, velocity, node_player.GrenadePaintColor);

				node_player.GrenadeThrowTimer = 0f;
			}
		}

		UpdateUI();
	}

	private void TickBullets (float delta)
	{
		for (int i = 0; i < m_bulletsList.Count; i++)
		{
			Bullet bullet = m_bulletsList[i];

			bullet.LifetimeTimer += delta;

			if (bullet.LifetimeTimer < bullet.Lifetime)
			{
				Vector2 oldPosition = bullet.Position;

				KinematicCollision2D collision = bullet.MoveAndCollide(bullet.Velocity * delta);

				Vector2 newPosition = bullet.Position;

				float x = Mathf.Abs(newPosition.x - oldPosition.x);
				float y = Mathf.Abs(newPosition.y - oldPosition.y);
				float distance = new Vector2(x, y).Length();

				bullet.DistanceTravelled += distance;

				if (collision != null)
				{
					CauseExplosion(collision.Position, bullet.ExplosionRadius, bullet.PaintColor);

					bullet.QueueFree();
					node_bullets.RemoveChild(bullet);
					m_bulletsList.RemoveAt(i);
					i--;
				}
				else if (bullet.IsDieAfterDistanceLimitReached && bullet.DistanceTravelled >= bullet.DistanceLimit)
				{
					CauseExplosion(bullet.Position, bullet.ExplosionRadius, bullet.PaintColor);

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
				float gravity = 256f;

				velocity.y += gravity * delta;

				KinematicCollision2D collision = grenade.MoveAndCollide(velocity * delta);

				if (collision != null)
				{
					Vector2 normal = collision.Normal;

					if (normal.y != 0)
					{
						velocity.x /= 2f;
						velocity.y /= 2f;
						velocity.y *= -1;
						if (normal.x != 0)
						{
							velocity.x *= -1;
						}
					}
					else if (normal.x != 0)
					{
						velocity.x /= 2f;
						velocity.x *= -1;
						if (normal.y != 0)
						{
							velocity.y /= 2f;
							velocity.y *= -1;
						}
					}
				}

				grenade.Velocity = velocity;
			}
			else
			{
				CauseExplosion(grenade.Position, grenade.ExplosionRadius, grenade.PaintColor, ESound.GrenadeExplode);

				OneShotParticles particles = grenade.DieParticlesPackedScene.Instance<OneShotParticles>();
				node_grenadeDieParticles.AddChild(particles);

				particles.Position = grenade.Position;
				particles.SelfModulate = PaintColors.GetColorFromPaintColor(grenade.PaintColor);

				grenade.QueueFree();
				node_grenades.RemoveChild(grenade);
				m_grenadesList.RemoveAt(i);
				i--;
			}
		}
	}

	private void SpawnStandardBullet (Vector2 position, Vector2 direction, EPaintColor paintColor)
	{
		Bullet bullet = m_standardBulletPackedScene.Instance<Bullet>();

		node_bullets.AddChild(bullet);
		m_bulletsList.Add(bullet);

		bullet.Position = position;
		bullet.Velocity = direction * bullet.Speed;
		bullet.SetPaintColor(paintColor);

		PlaySound(ESound.StandardGunShoot);
	}

	private void SpawnRifleBullet (Vector2 position, Vector2 direction, EPaintColor paintColor)
	{
		Bullet bullet = m_rifleBulletPackedScene.Instance<Bullet>();

		node_bullets.AddChild(bullet);
		m_bulletsList.Add(bullet);

		bullet.Position = position;
		bullet.Velocity = direction * bullet.Speed;
		bullet.SetPaintColor(paintColor);

		PlaySound(ESound.RifleShoot);
	}

	private void SpawnMachineGunBullet (Vector2 position, Vector2 direction, EPaintColor paintColor)
	{
		Bullet bullet = m_machineGunBulletPackedScene.Instance<Bullet>();

		node_bullets.AddChild(bullet);
		m_bulletsList.Add(bullet);

		bullet.Position = position;
		bullet.Velocity = direction * bullet.Speed;
		bullet.SetPaintColor(paintColor);

		PlaySound(ESound.MachineGunShoot);
	}

	private void SpawnSniperRifleBullet (Vector2 position, Vector2 direction, EPaintColor paintColor)
	{
		Bullet bullet = m_sniperRifleBulletPackedScene.Instance<Bullet>();

		node_bullets.AddChild(bullet);
		m_bulletsList.Add(bullet);

		bullet.Position = position;
		bullet.Velocity = direction * bullet.Speed;
		bullet.SetPaintColor(paintColor);

		PlaySound(ESound.SniperRifleShoot);
	}

	private void SpawnGrenade (Vector2 position, Vector2 velocity, EPaintColor paintColor)
	{
		Grenade grenade = m_grenadePackedScene.Instance<Grenade>();

		node_grenades.AddChild(grenade);
		m_grenadesList.Add(grenade);

		grenade.Position = position;
		grenade.Velocity = velocity;
		grenade.SetPaintColor(paintColor);
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

		for (int i = 0; i < results.Count; i++)
		{
			object result = results[i];
			Godot.Collections.Dictionary dict = (Godot.Collections.Dictionary)result;

			if (dict.Contains("collider") && dict["collider"] is Area2D area)
			{
				if (area.GetParent() != null && area.GetParent() is LightMask lightMask)
				{
					Color newColor = PaintColors.GetColorFromPaintColor(paintColor);
					Color oldColor = lightMask.Color;

					if (oldColor != newColor)
					{
						switch (paintColor)
						{
							case EPaintColor.White:
								m_whiteTileCount++;
								break;
							case EPaintColor.Red:
								m_redTileCount++;
								break;
							case EPaintColor.Yellow:
								m_yellowTileCount++;
								break;
							case EPaintColor.Green:
								m_greenTileCount++;
								break;
							case EPaintColor.Aqua:
								m_aquaTileCount++;
								break;
							case EPaintColor.Blue:
								m_blueTileCount++;
								break;
							case EPaintColor.Fuchsia:
								m_fuchsiaTileCount++;
								break;
							default:
								break;
						}

						if (oldColor == Colors.White)
							m_whiteTileCount--;
						else if (oldColor == Colors.Red)
							m_redTileCount--;
						else if (oldColor == Colors.Yellow)
							m_yellowTileCount--;
						else if (oldColor == Colors.Green)
							m_greenTileCount--;
						else if (oldColor == Colors.Aqua)
							m_aquaTileCount--;
						else if (oldColor == Colors.Blue)
							m_blueTileCount--;
						else if (oldColor == Colors.Fuchsia)
							m_fuchsiaTileCount--;

						SetLightMaskColor(lightMask, paintColor);
					}
				}
			}
		}
	}
	private void CauseExplosion (Vector2 position, float radius, EPaintColor paintColor, ESound soundToPlay)
	{
		CauseExplosion(position, radius, paintColor);
		PlaySound(soundToPlay);
	}

	private void PlaySound (ESound sound)
	{
		OneShotSound oneShotSound;

		switch (sound)
		{
			case ESound.PlayerJump:
				oneShotSound = m_playerJumpOneShotSoundPackedScene.Instance<OneShotSound>();
				break;
			case ESound.PlayerLand:
				oneShotSound = m_playerLandOneShotSoundPackedScene.Instance<OneShotSound>();
				break;

			case ESound.StandardGunShoot:
				oneShotSound = m_standardGunShootOneShotSoundPackedScene.Instance<OneShotSound>();
				break;
			case ESound.GrenadeExplode:
				oneShotSound = m_grenadeExplodeOneShotSoundPackedScene.Instance<OneShotSound>();
				break;

			case ESound.RifleShoot:
				oneShotSound = m_rifleShootOneShotSoundPackedScene.Instance<OneShotSound>();
				break;
			case ESound.MachineGunShoot:
				oneShotSound = m_machineGunShootOneShotSoundPackedScene.Instance<OneShotSound>();
				break;
			case ESound.SniperRifleShoot:
				oneShotSound = m_sniperRifleShootOneShotSoundPackedScene.Instance<OneShotSound>();
				break;

			default:
				oneShotSound = m_playerJumpOneShotSoundPackedScene.Instance<OneShotSound>();
				break;
		}

		node_oneShotSounds.AddChild(oneShotSound);
	}

	#endregion // Private methods

}
