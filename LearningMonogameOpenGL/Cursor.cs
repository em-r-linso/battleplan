using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace LearningMonogameOpenGL
{
	public class Cursor : GameObject
	{
		public Cursor(ContentManager content, float x, float y) : base(
			content,
			"ball",
			0,
			0,
			GameData.Config.CursorSpeed
		)
		{
		}

		public override void OnClick()
		{
			Target   = Mouse.GetState().Position.ToVector2();
			IsMoving = true;
		}
	}
}