using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LearningMonogameOpenGL
{
	/// <inheritdoc cref="Game" />
	public class Game1 : Game
	{
		public Game1()
		{
			ContentManager        = Content;
			Graphics              = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible        = true;
		}

		public static ContentManager ContentManager { get; set; }

		GraphicsDeviceManager Graphics    { get; }
		SpriteBatch           Batch       { get; set; }
		List<GameObject>      GameObjects { get; set; }
		GameObject            Cursor      { get; set; }

		protected override void Initialize()
		{
			var screenCenter = new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight) / 2f;

			// initialize GameObjects list with necessary GameObjects
			Cursor = new GameObject(new Sprite("tCharacterRing"), screenCenter, GameData.Config.CursorSpeed);
			var background = new GameObject(new Sprite("tDebugBG", Sprite.OriginType.TopLeft), Vector2.Zero, 0);
			var debugActor = new Actor(
				new Sprite("tDebugCharacter", Sprite.OriginType.Bottom),
				screenCenter,
				100,
				0.5f,
				new Behavior(Behavior.DoType.Approach, Behavior.ToType.MyTarget),
				"Test Guy"
			) {Target = Cursor};
			GameObjects = new List<GameObject> {Cursor, background, debugActor};

			base.Initialize();
		}

		protected override void LoadContent() { Batch = new SpriteBatch(GraphicsDevice); }

		protected override void Update(GameTime gameTime)
		{
			// set cursor target to click location
			if (Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				Cursor.Destination = Mouse.GetState().Position.ToVector2();
				Cursor.IsMoving    = true;
			}

			// move every GameObject
			foreach (var gameObject in GameObjects)
				gameObject.Move((float)gameTime.ElapsedGameTime.TotalSeconds);
			foreach (var gameObject in GameObjects.OfType<Actor>())
				gameObject.Idle((float)gameTime.ElapsedGameTime.TotalSeconds);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			Batch.Begin();

			// draw every GameObject (layered by Y)
			foreach (var gameObject in GameObjects.OrderBy(o => o.Position.Y).ToList())
			{
				Batch.Draw(
					gameObject.Sprite.Texture,
					gameObject.Position,
					null,
					Color.White,
					0f,
					gameObject.Sprite.OriginVector,
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