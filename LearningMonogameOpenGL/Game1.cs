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

		GraphicsDeviceManager Graphics       { get; }
		SpriteBatch           Batch          { get; set; }
		GameObject            Cursor         { get; set; }
		bool                  MouseDown      { get; set; }
		Formation             Party          { get; set; }

		protected override void Initialize()
		{
			var screenCenter = new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight) / 2f;

			// create Cursor
			Cursor = new GameObject(new Sprite("tCharacterRing"), screenCenter, GameData.Config.CursorSpeed);

			// create background
			new GameObject(new Sprite("tDebugBG", Sprite.OriginType.TopLeft), Vector2.Zero, 0);

			// create debug characters
			var A1 = new Actor(
				new Sprite("tDebugCharacter", Sprite.OriginType.Bottom),
				screenCenter,
				300,
				0.5f,
				new[] {new Behavior(Behavior.DoType.Approach, Behavior.ToType.FormationMarker)},
				"Test Guy"
			);
			var A2 = new Actor(
				new Sprite("tDebugCharacter", Sprite.OriginType.Bottom),
				screenCenter,
				300,
				0.5f,
				new[] {new Behavior(Behavior.DoType.Approach, Behavior.ToType.FormationMarker)},
				"Test Guy"
			);
			var A3 = new Actor(
				new Sprite("tDebugCharacter", Sprite.OriginType.Bottom),
				screenCenter,
				300,
				0.5f,
				new[] {new Behavior(Behavior.DoType.Approach, Behavior.ToType.FormationMarker)},
				"Test Guy"
			);

			Party = new Formation(3, 10);
			Party.AddUnit(A1, 1, 0);
			Party.AddUnit(A2, 0, 2);
			Party.AddUnit(A3, 2, 2);

			base.Initialize();
		}

		protected override void LoadContent() { Batch = new SpriteBatch(GraphicsDevice); }

		protected override void Update(GameTime gameTime)
		{
			// set cursor destination to click location & move formation
			if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && !MouseDown)
			{
				MouseDown = true;
				var clickPosition = Mouse.GetState().Position.ToVector2();

				// move Party
				Party.MoveFormation(clickPosition);

				// move Cursor
				Cursor.Position    = Party.FrontCenterPosition;
				Cursor.Destination = clickPosition;
				Cursor.IsMoving    = true;
			}
			else if (MouseDown && (Mouse.GetState().LeftButton == ButtonState.Released))
			{
				MouseDown = false;
			}

			// update every GameObject
			foreach (var gameObject in GameObject.Registry)
				gameObject.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

			Batch.Begin();

			// draw every GameObject (layered by Y)
			foreach (var gameObject in GameObject.Registry.OrderBy(o => o.Position.Y).ToList())
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