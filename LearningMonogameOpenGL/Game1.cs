using System;
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

		GraphicsDeviceManager Graphics       { get; }
		SpriteBatch           Batch          { get; set; }
		List<GameObject>      GameObjects    { get; set; }
		GameObject            Cursor         { get; set; }
		GameObject[,]         FormationSpots { get; set; }
		bool                  MouseDown      { get; set; }
		Actor                 PartyLeader    { get; set; }

		protected override void Initialize()
		{
			var screenCenter = new Vector2(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight) / 2f;

			// initialize list of GameObjects
			GameObjects = new List<GameObject>();

			// create Cursor
			GameObjects.Add(
				Cursor = new GameObject(new Sprite("tCharacterRing"), screenCenter, GameData.Config.CursorSpeed)
			);

			// create formation positions
			FormationSpots = new GameObject[3, 3];
			for (var row = 0; row < FormationSpots.GetLength(0); row++)
			{
				for (var col = 0; col < FormationSpots.GetLength(1); col++)
				{
					GameObjects.Add(
						FormationSpots[row, col] = new GameObject(new Sprite("tCharacterRing"), screenCenter, 0)
					);
				}
			}

			// create background
			GameObjects.Add(new GameObject(new Sprite("tDebugBG", Sprite.OriginType.TopLeft), Vector2.Zero, 0));

			// create debug characters
			GameObjects.Add(
				PartyLeader = new Actor(
					new Sprite("tDebugCharacter", Sprite.OriginType.Bottom),
					screenCenter,
					300,
					0.5f,
					new[] {new Behavior(Behavior.DoType.Approach, Behavior.ToType.MyTarget)},
					"Test Guy"
				) {Target = FormationSpots[0, 1]}
			);
			GameObjects.Add(
				new Actor(
					new Sprite("tDebugCharacter", Sprite.OriginType.Bottom),
					screenCenter,
					300,
					0.5f,
					new[] {new Behavior(Behavior.DoType.Approach, Behavior.ToType.MyTarget)},
					"Test Guy"
				) {Target = FormationSpots[1, 2]}
			);
			GameObjects.Add(
				new Actor(
					new Sprite("tDebugCharacter", Sprite.OriginType.Bottom),
					screenCenter,
					300,
					0.5f,
					new[] {new Behavior(Behavior.DoType.Approach, Behavior.ToType.MyTarget)},
					"Test Guy"
				) {Target = FormationSpots[2, 0]}
			);

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

				// move Cursor
				Cursor.Destination = clickPosition;
				Cursor.IsMoving    = true;

				// find the angle to rotate the formation
				var distance = clickPosition - PartyLeader.Position;
				distance = new Vector2(distance.X, distance.Y * 2); // this corrects for 45deg view
				var angle = AngleBetween(distance, Vector2.UnitY);

				// assign a position for each formation spot
				for (var row = 0; row < FormationSpots.GetLength(0); row++)
				{
					for (var col = 0; col < FormationSpots.GetLength(1); col++)
					{
						// organize into rows/cols
						var offset = new Vector2(col - 1, row);

						// scale by FormationSpacing
						offset *= GameData.Config.FormationSpacing;

						// rotate
						// TODO: Total War style facing
						offset = Vector2.Transform(offset, Matrix.CreateRotationZ(angle));
						offset = new Vector2(offset.X, -offset.Y); // I do not know why, but Y must be inverted

						// correct for 45deg view
						offset *= new Vector2(1, 0.5f);

						// place formation spot
						FormationSpots[row, col].Position = clickPosition + offset;
					}
				}
			}
			else if (MouseDown && (Mouse.GetState().LeftButton == ButtonState.Released))
			{
				MouseDown = false;
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

		float AngleBetween(Vector2 a, Vector2 b)
		{
			double sin = (a.X * b.Y) - (b.X * a.Y);
			double cos = (a.X * b.X) + (a.Y * b.Y);

			// return (float)(Math.Atan2(sin, cos) * (180 / Math.PI));

			return (float)Math.Atan2(sin, cos);
		}
	}
}