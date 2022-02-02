using System;
using Microsoft.Xna.Framework;

namespace Battleplan
{
	public static class Vector2Extensions
	{
		public static Matrix RotationTo(this Vector2 from, Vector2 to)
		{
			// difference between vectors
			var (x1, y1) = to - from;

			// reference vector (straight up)
			var (x2, y2) = Vector2.UnitY;

			// angle (in rads)
			var sin   = (x1 * y2) - (x2 * y1);
			var cos   = (x1 * x2) + (y1 * y2);
			var angle = (float)Math.Atan2(sin, cos);

			return Matrix.CreateRotationZ(angle);
		}

		public static Vector2 WorldToScreen(this Vector2 vector) => vector * new Vector2(1, 0.5f);

		public static Vector2 ScreenToWorld(this Vector2 vector) => vector * new Vector2(1, 2);
	}
}