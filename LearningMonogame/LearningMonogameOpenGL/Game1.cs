using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LearningMonogameOpenGL
{
	public class Game1 : Game
	{
		readonly GraphicsDeviceManager _graphics;
		Vector2                        _ballPosition;
		float                          _ballSpeed;
		Texture2D                      _ballTexture;
		SpriteBatch                    _spriteBatch;

		public Game1()
		{
			_graphics             = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible        = true;
		}

		// ReSharper disable once RedundantOverriddenMember
		protected override void Initialize()
		{
			_ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
			_ballSpeed    = 100f;

			base.Initialize();
		}

		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_ballTexture = Content.Load<Texture2D>("ball");
		}

		protected override void Update(GameTime gameTime)
		{
			if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			var kState = Keyboard.GetState();

			if (kState.IsKeyDown(Keys.Up))
			{
				_ballPosition.Y -= _ballSpeed * (float) gameTime.ElapsedGameTime.TotalSeconds;
			}

			if (kState.IsKeyDown(Keys.Down))
			{
				_ballPosition.Y += _ballSpeed * (float) gameTime.ElapsedGameTime.TotalSeconds;
			}

			if (kState.IsKeyDown(Keys.Left))
			{
				_ballPosition.X -= _ballSpeed * (float) gameTime.ElapsedGameTime.TotalSeconds;
			}

			if (kState.IsKeyDown(Keys.Right))
			{
				_ballPosition.X += _ballSpeed * (float) gameTime.ElapsedGameTime.TotalSeconds;
			}

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();
			_spriteBatch.Draw(_ballTexture, _ballPosition, null, Color.White, 0f, new Vector2(_ballTexture.Width / 2, _ballTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}