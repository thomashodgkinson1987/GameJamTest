using System;
using System.Collections.Generic;
using Godot;

public static class PaintColors
{

	#region Enums

	public enum EPaintColor { White, Red, Yellow, Green, Aqua, Blue, Fuchsia }

	#endregion // Enums



	#region Properties

	public static Color White => new Color(1.0f, 1.0f, 1.0f, 1.0f);
	public static Color Red => new Color(1.0f, 0.0f, 0.0f, 1.0f);
	public static Color Yellow => new Color(1.0f, 1.0f, 0.0f, 1.0f);
	public static Color Green => new Color(0.0f, 1.0f, 0.0f, 1.0f);
	public static Color Aqua => new Color(0.0f, 1.0f, 1.0f, 1.0f);
	public static Color Blue => new Color(0.0f, 0.0f, 1.0f, 1.0f);
	public static Color Fuchsia => new Color(1.0f, 0.0f, 1.0f, 1.0f);

	#endregion // Properties



	#region Fields

	private readonly static Dictionary<EPaintColor, Color> m_paintColorsDictionary;

	#endregion // Fields



	#region Constructors

	static PaintColors ()
	{
		m_paintColorsDictionary = new Dictionary<EPaintColor, Color>
		{
			{ EPaintColor.White, new Color(1.0f, 1.0f, 1.0f, 1.0f) },
			{ EPaintColor.Red, new Color(1.0f, 0.0f, 0.0f, 1.0f) },
			{ EPaintColor.Yellow, new Color(1.0f, 1.0f, 0.0f, 1.0f) },
			{ EPaintColor.Green, new Color(0.0f, 1.0f, 0.0f, 1.0f) },
			{ EPaintColor.Aqua, new Color(0.0f, 1.0f, 1.0f, 1.0f) },
			{ EPaintColor.Blue, new Color(0.0f, 0.0f, 1.0f, 1.0f) },
			{ EPaintColor.Fuchsia, new Color(1.0f, 0.0f, 1.0f, 1.0f) }
		};
	}

	#endregion // Constructors



	#region Public methods

	public static Color GetColorFromPaintColor (EPaintColor paintColor)
	{
		return m_paintColorsDictionary[paintColor];
	}

	#endregion // Public methods

}
