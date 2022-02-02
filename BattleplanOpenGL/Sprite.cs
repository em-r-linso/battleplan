using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battleplan
{
	public class Sprite
	{
		public enum OriginType
		{
			TopLeft,
			Top,
			TopRight,
			Left,
			Center,
			Right,
			BottomLeft,
			Bottom,
			BottomRight
		}

		public Sprite(string textureName = "tDebug", OriginType origin = OriginType.Center)
		{
			Texture = GameManager.ContentManager.Load<Texture2D>(textureName);
			Origin  = origin;
		}

		public Texture2D Texture { get; }

		OriginType Origin { get; }

		public Vector2 OriginVector
		{
			get
			{
				var (x, y) = Origin switch
				{
					OriginType.TopLeft     => (0.0f, 0.0f),
					OriginType.Top         => (0.5f, 0.0f),
					OriginType.TopRight    => (1.0f, 0.0f),
					OriginType.Left        => (0.0f, 0.5f),
					OriginType.Center      => (0.5f, 0.5f),
					OriginType.Right       => (1.0f, 0.5f),
					OriginType.BottomLeft  => (0.0f, 1.0f),
					OriginType.Bottom      => (0.5f, 1.0f),
					OriginType.BottomRight => (1.0f, 1.0f),
					_                      => (0.0f, 0.0f)
				};
				return new Vector2(Texture.Width * x, Texture.Height * y);
			}
		}
	}
}