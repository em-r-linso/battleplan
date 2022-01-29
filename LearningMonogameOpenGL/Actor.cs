using System;
using Microsoft.Xna.Framework;

namespace LearningMonogameOpenGL
{
	public class Actor : GameObject
	{
		readonly Random _r = new Random();

		public Actor(Sprite sprite, Vector2 position, float moveSpeed, float actInterval) : base(sprite, position, moveSpeed)
		{
			ActInterval = actInterval;
		}

		float ActClock    { get; set; }
		float ActInterval { get; }

		public void Act(float time)
		{
			ActClock += time;
			if (ActClock >= ActInterval)
			{
				ActClock -= ActInterval;
				IsMoving =  true;
				Target   += Vector2.UnitX * _r.Next(-99, 100);
				Target   += Vector2.UnitY * _r.Next(-99, 100);
			}
		}
	}
}