using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

		GraphicsDeviceManager Graphics     { get; }
		SpriteBatch           Batch        { get; set; }
		Cursor                Cursor       { get; set; }
		Texture2D             DebugTexture { get; set; }

		// ReSharper disable once RedundantOverriddenMember
		protected override void Initialize()
		{
			Cursor = new Cursor(Content, 0, 0);
			base.Initialize();
		}

		protected override void LoadContent()
		{
			Batch = new SpriteBatch(GraphicsDevice);

			DebugTexture  = Content.Load<Texture2D>("tDebug");
		}

		protected override void Update(GameTime gameTime)
		{
			var time = (float) gameTime.ElapsedGameTime.TotalSeconds;

			Cursor.Update(time);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			Batch.Begin();
			Batch.Draw(
				Cursor.Texture,
				Cursor.CursorPosition,
				null,
				Color.White,
				0f,
				new Vector2(Cursor.Texture.Width / 2f, Cursor.Texture.Height / 2f),
				Vector2.One,
				SpriteEffects.None,
				0f
			);
			Batch.Draw(
				DebugTexture,
				new Vector2(Graphics.PreferredBackBufferWidth / 2f, Graphics.PreferredBackBufferHeight / 2f),
				null,
				Color.White,
				0f,
				new Vector2(DebugTexture.Width / 2f, DebugTexture.Height / 2f),
				Vector2.One,
				SpriteEffects.None,
				0f
			);
			Batch.End();

			base.Draw(gameTime);
		}
	}
}