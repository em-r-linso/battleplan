using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Battleplan
{
	/// <inheritdoc cref="Game" />
	public class GameManager : Game
	{
		public GameManager()
		{
			ContentManager        = Content;
			Graphics              = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible        = true;
		}

		public static ContentManager ContentManager { get; set; }
		GraphicsDeviceManager        Graphics       { get; }
		SpriteBatch                  Batch          { get; set; }
		GameObject                   Cursor         { get; set; }
		bool                         MouseDown      { get; set; }
		Formation                    Party          { get; set; }
		Line                         PartyLine      { get; set; }
		Vector2                      CameraPosition { get; set; }

		void UpdateScreenSize()
		{
			Graphics.PreferredBackBufferWidth  = Graphics.GraphicsDevice.DisplayMode.Width;
			Graphics.PreferredBackBufferHeight = Graphics.GraphicsDevice.DisplayMode.Height;
			Graphics.IsFullScreen              = false;
			Graphics.ApplyChanges();
		}

		protected override void Initialize()
		{
			UpdateScreenSize();

			var screenCenter = new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight) / 2f;
			screenCenter = screenCenter.ScreenToWorld();

			// create Cursor
			Cursor = new GameObject(new Sprite("tCharacterRing"), screenCenter, GameData.Config.CursorSpeed);

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

			Party = new Formation(3, 3);
			Party.AddUnit(A1, 1, 0);
			Party.AddUnit(A2, 0, 2);
			Party.AddUnit(A3, 2, 2);

			PartyLine = new Line(new Sprite("tLineSegment"), Vector2.Zero, Vector2.Zero);

			base.Initialize();
		}

		protected override void LoadContent() { Batch = new SpriteBatch(GraphicsDevice); }

		protected override void Update(GameTime gameTime)
		{
			// update camera position
			CameraPosition =  (Party.FrontCenterUnitPosition + Cursor.Position) / 2;
			CameraPosition =  CameraPosition.WorldToScreen();
			CameraPosition -= new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight) / 2f;

			// set cursor destination to click location & move formation
			if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && !MouseDown)
			{
				MouseDown = true;
				var clickPosition = Mouse.GetState().Position.ToVector2() + CameraPosition;
				clickPosition = clickPosition.ScreenToWorld();

				// move Party
				Party.MoveFormation(clickPosition);

				// move Cursor
				Cursor.Destination = clickPosition;
				Cursor.IsMoving    = true;

				// move line
				PartyLine.RebuildLine(Party.FrontCenterUnitPosition, clickPosition);
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

			// draw every Line
			foreach (var line in Line.Registry)
			{
				foreach (var segment in line.Segments)
				{
					Batch.Draw(
						line.Sprite.Texture,
						segment.WorldToScreen() - CameraPosition,
						null,
						Color.White,
						0f,
						line.Sprite.OriginVector,
						Vector2.One,
						SpriteEffects.None,
						0f
					);
				}
			}

			// draw every object
			foreach (var gameObject in GameObject.Registry.OrderBy(o => o.Position.Y).ToList())
			{
				Batch.Draw(
					gameObject.Sprite.Texture,
					gameObject.Position.WorldToScreen() - CameraPosition,
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