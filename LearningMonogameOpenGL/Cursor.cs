using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LearningMonogameOpenGL
{
	public class Cursor
	{
		public Cursor(ContentManager content, float x, float y)
		{
			// set texture
			Texture = content.Load<Texture2D>("ball");

			// initialize position and target to same point
			CursorPosition = CursorTarget = new Vector2(x, y);
		}

		/// <summary>Current screen position of the cursor.</summary>
		public Vector2 CursorPosition { get; private set; }

		/// <summary>The point that the cursor moves toward every frame.</summary>
		Vector2 CursorTarget { get; set; }

		public Texture2D Texture { get; }

		public void Update(float time)
		{
			SetTarget();
			MoveToTarget(time);
		}

		void SetTarget()
		{
			var mState = Mouse.GetState();

			// set target on left click
			if (mState.LeftButton == ButtonState.Pressed)
			{
				CursorTarget = mState.Position.ToVector2();
			}
		}

		void MoveToTarget(float time)
		{
			// snap to target if close enough
			if (Vector2.Distance(CursorPosition, CursorTarget) <= (GameData.Config.CursorSpeed * time))
			{
				CursorPosition = CursorTarget;
			}

			// step toward target if not close enough to snap
			else
			{
				var difference = CursorTarget - CursorPosition;
				difference.Normalize();
				CursorPosition += difference * GameData.Config.CursorSpeed * time;
			}
		}
	}
}