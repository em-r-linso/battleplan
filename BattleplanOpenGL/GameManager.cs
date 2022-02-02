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
		Line                         PartyPath      { get; set; }
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

			// create Cursor
			Cursor = new GameObject(new Sprite("tCharacterRing"), Vector2.Zero, GameData.Config.CursorSpeed);

			// create debug characters
			Party = new Formation(3, 3, Vector2.Zero);
			var debugCharacters = new Actor[3];
			for (var i = 0; i < 3; i++)
			{
				debugCharacters[i] = new Actor(
					new Sprite("tDebugCharacter", Sprite.OriginType.Bottom),
					Vector2.Zero,
					300,
					0.5f,
					new[] {new Behavior(Behavior.DoType.Approach, Behavior.ToType.FormationMarker)},
					"Test Guy"
				);
			}

			Party.AddUnit(debugCharacters[0], 1, 0);
			Party.AddUnit(debugCharacters[1], 0, 2);
			Party.AddUnit(debugCharacters[2], 2, 2);
			Party.Initialize();

			// create party path line
			PartyPath = new Line(new Sprite("tLineSegment"), Vector2.Zero, Vector2.Zero);

			base.Initialize();
		}

		protected override void LoadContent() { Batch = new SpriteBatch(GraphicsDevice); }

		protected override void Update(GameTime gameTime)
		{
			// update camera position
			CameraPosition =  (Party.FrontCenterUnitPosition + Cursor.Position) / 2;
			CameraPosition =  CameraPosition.WorldToScreen();
			CameraPosition -= new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight) / 2f;

			// move line
			PartyPath.RebuildLine(Party.FrontCenterMarkerPosition, Party.FrontCenterUnitPosition);

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