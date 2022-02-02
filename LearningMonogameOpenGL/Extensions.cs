using System;
using Microsoft.Xna.Framework;

namespace LearningMonogameOpenGL
{
	public static class Vector2Extensions
	{
		public static Matrix RotationTo(this Vector2 from, Vector2 to, bool scaleFor45Deg = true)
		{
			// difference between vectors
			var (x1, y1) = to - from;
			if (scaleFor45Deg)
			{
				y1 *= 2;
			}

			// reference vector (straight up)
			var (x2, y2) = Vector2.UnitY;

			// angle (in rads)
			var sin   = (x1 * y2) - (x2 * y1);
			var cos   = (x1 * x2) + (y1 * y2);
			var angle = (float)Math.Atan2(sin, cos);

			return Matrix.CreateRotationZ(angle);
		}
	}
}