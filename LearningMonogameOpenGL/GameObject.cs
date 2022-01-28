using Microsoft.Xna.Framework;

namespace LearningMonogameOpenGL
{
	/// <summary>An object physically in the world with a location and texture.</summary>
	public class GameObject
	{
		public GameObject(Sprite sprite, Vector2 position, float moveSpeed)
		{
			MoveSpeed = moveSpeed;
			Sprite    = sprite;

			// initialize position and target to same point
			Position = Target = position;
		}

		/// <summary>Current screen position of the object.</summary>
		public Vector2 Position { get; set; }

		/// <summary>Point that the object moves toward every frame.</summary>
		public Vector2 Target { get; set; }

		/// <summary>Speed in pixels/second the object uses to move toward its target.</summary>
		public float MoveSpeed { get; set; }

		/// <summary>Texture drawn at the object's position.</summary>
		public Sprite Sprite { get; set; }

		public bool IsMoving { get; set; }

		public void Move(float time)
		{
			if (!IsMoving)
			{
				return;
			}

			// snap to target if close enough
			if (Vector2.Distance(Position, Target) <= (MoveSpeed * time))
			{
				Position = Target;
				IsMoving = false;
			}

			// step toward target if not close enough to snap
			else
			{
				var difference = Target - Position;
				difference.Normalize();
				Position += difference * MoveSpeed * time;
			}
		}
	}
}