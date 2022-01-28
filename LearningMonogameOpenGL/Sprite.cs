using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LearningMonogameOpenGL
{
	public class Sprite
	{
		public enum OriginType
		{
			Topleft,
			Top,
			Topright,
			Left,
			Center,
			Right,
			Bottomleft,
			Bottom,
			Bottomright
		}

		public Sprite(string textureName = "tDebug", OriginType origin = OriginType.Center)
		{
			Texture = Game1.ContentManager.Load<Texture2D>(textureName);
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
					OriginType.Topleft     => (0.0f, 0.0f),
					OriginType.Top         => (0.5f, 0.0f),
					OriginType.Topright    => (1.0f, 0.0f),
					OriginType.Left        => (0.0f, 0.5f),
					OriginType.Center      => (0.5f, 0.5f),
					OriginType.Right       => (1.0f, 0.5f),
					OriginType.Bottomleft  => (0.0f, 1.0f),
					OriginType.Bottom      => (0.5f, 1.0f),
					OriginType.Bottomright => (1.0f, 1.0f),
					_                      => (0.0f, 0.0f)
				};
				return new Vector2(Texture.Width * x, Texture.Height * y);
			}
		}
	}
}