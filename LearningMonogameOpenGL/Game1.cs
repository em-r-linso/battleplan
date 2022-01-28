using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LearningMonogameOpenGL
{
	/// <inheritdoc cref="Game" />
	public class Game1 : Game
	{
		public Game1()
		{
			Graphics              = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible        = true;
		}

		GraphicsDeviceManager Graphics { get; }

		SpriteBatch Batch { get; set; }

		/// <summary>Number of seconds since the last frame. Useful for multiplying into speeds and durations.</summary>
		float Time { get; set; }

		/// <summary>Current screen position of the cursor.</summary>
		Vector2 CursorPosition { get; set; }

		/// <summary>The point that the cursor moves toward every frame.</summary>
		Vector2 CursorTarget { get; set; }

		/// <summary>Texture for the cursor.</summary>
		Texture2D CursorTexture { get; set; }

		// ReSharper disable once RedundantOverriddenMember
		protected override void Initialize() { base.Initialize(); }

		protected override void LoadContent()
		{
			Batch = new SpriteBatch(GraphicsDevice);

			CursorTexture = Content.Load<Texture2D>("ball");
		}

		protected override void Update(GameTime gameTime)
		{
			Time = (float) gameTime.ElapsedGameTime.TotalSeconds;

			var mState = Mouse.GetState();

			if (mState.LeftButton == ButtonState.Pressed)
			{
				CursorTarget = mState.Position.ToVector2();
			}

			if (Vector2.Distance(CursorPosition, CursorTarget) <= (GameData.Config.CursorSpeed * Time))
			{
				CursorPosition = CursorTarget;
			}
			else
			{
				var temp = CursorTarget - CursorPosition;
				temp.Normalize();
				CursorPosition += temp * GameData.Config.CursorSpeed * Time;
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			Batch.Begin();
			Batch.Draw(
				CursorTexture,
				CursorPosition,
				null,
				Color.White,
				0f,
				new Vector2(CursorTexture.Width / 2f, CursorTexture.Height / 2f),
				Vector2.One,
				SpriteEffects.None,
				0f
			);
			Batch.End();

			base.Draw(gameTime);
		}
	}
}