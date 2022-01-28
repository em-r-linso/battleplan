using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LearningMonogameOpenGL
{
	/// <summary>An object physically in the world with a location and texture.</summary>
	public class GameObject
	{
		public GameObject(ContentManager content, string textureAssetName, float x, float y, float moveSpeed)
		{
			MoveSpeed = moveSpeed;

			// set texture
			try
			{
				Texture = content.Load<Texture2D>(textureAssetName);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}

			// initialize position and target to same point
			Position = Target = new Vector2(x, y);
		}

		/// <summary>Current screen position of the object.</summary>
		public Vector2 Position { get; set; }

		/// <summary>Point that the object moves toward every frame.</summary>
		public Vector2 Target { get; set; }

		/// <summary>Speed in pixels/second the object uses to move toward its target.</summary>
		public float MoveSpeed { get; set; }

		/// <summary>Texture drawn at the object's position.</summary>
		public Texture2D Texture { get; set; }

		public bool IsMoving { get; set; }

		public void Update(float time)
		{
			if (Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				OnClick();
			}

			if (IsMoving)
			{
				StepTowardTarget(time);
			}
		}

		void StepTowardTarget(float time)
		{
			// snap to target if close enough
			if (Vector2.Distance(Position, Target) <= (GameData.Config.CursorSpeed * time))
			{
				Position = Target;
				IsMoving = false;
			}

			// step toward target if not close enough to snap
			else
			{
				var difference = Target - Position;
				difference.Normalize();
				Position += difference * GameData.Config.CursorSpeed * time;
			}
		}

		public virtual void OnClick() {
		}
	}
}