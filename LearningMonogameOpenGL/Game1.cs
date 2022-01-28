using System.Collections.Generic;
using System.Linq;
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
		List<GameObject>      GameObjects  { get; set; }

		protected override void Initialize()
		{
			// initialize GameObjects list with necessary GameObjects
			GameObjects = new List<GameObject>
			{
				new Cursor(Content, 0, 0),
				new GameObject(Content,"tDebug", Graphics.PreferredBackBufferWidth / 2f, Graphics.PreferredBackBufferHeight /2f, 0)
			};

			base.Initialize();
		}

		protected override void LoadContent()
		{
			Batch = new SpriteBatch(GraphicsDevice);

			DebugTexture = Content.Load<Texture2D>("tDebug");
		}

		protected override void Update(GameTime gameTime)
		{
			// update every GameObject
			foreach (var gameObject in GameObjects)
			{
				gameObject.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			Batch.Begin();

			// draw every GameObject (layered by Y)
			var gameObjectsByY = GameObjects.OrderBy(o => o.Position.Y).ToList();
			foreach (var gameObject in gameObjectsByY)
			{
				Batch.Draw(
					gameObject.Texture,
					gameObject.Position,
					null,
					Color.White,
					0f,
					new Vector2(gameObject.Texture.Width / 2f, gameObject.Texture.Height / 2f),
					Vector2.One,
					SpriteEffects.None,
					0f
				);
			}

			Batch.End();

			base.Draw(gameTime);
		}
	}
}