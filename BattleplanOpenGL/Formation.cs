using Microsoft.Xna.Framework;

namespace Battleplan
{
	public class Formation
	{
		public Formation(int width, int depth)
		{
			Width            = width;
			Depth            = depth;
			FormationMarkers = new GameObject[width, depth];
			Units            = new Actor[width, depth];

			for (var file = 0; file < FormationMarkers.GetLength(0); file++)
			{
				for (var rank = 0; rank < FormationMarkers.GetLength(1); rank++)
				{
					FormationMarkers[file, rank] = new GameObject(new Sprite("tCharacterRing"), Vector2.Zero, 0);
				}
			}
		}

		int                  Width                     { get; }
		int                  Depth                     { get; }
		public GameObject[,] FormationMarkers          { get; set; }
		Actor[,]             Units                     { get; }
		public Vector2       FrontCenterUnitPosition   { get; set; }
		public Vector2       FrontCenterMarkerPosition { get; set; }

		public void AddUnit(Actor newUnit, int file, int rank)
		{
			Units[file, rank]       = newUnit;
			newUnit.FormationMarker = FormationMarkers[file, rank];
		}

		public void MoveFormation(Vector2 destination)
		{
			// BUG: this will crash if there's no unit in the front center
			// 	formations with no front unit should probably automatically adjust:
			// 	1. trim edges
			// 	2. if there's still no frontcenter, add edges to either side to center a unit
			FrontCenterUnitPosition   = Units[Width            / 2, 0].Position;
			FrontCenterMarkerPosition = FormationMarkers[Width / 2, 0].Position;

			// find the angle to rotate the formation
			var angle = FrontCenterUnitPosition.RotationTo(destination);

			// assign a position for each formation spot
			for (var file = 0; file < FormationMarkers.GetLength(0); file++)
			{
				for (var rank = 0; rank < FormationMarkers.GetLength(1); rank++)
				{
					// organize into rows/cols
					var offset = new Vector2(file - (Width / 2), rank);

					// scale by FormationSpacing
					offset *= GameData.Config.FormationSpacing;

					// rotate
					// TODO: Total War style facing
					offset = Vector2.Transform(offset, angle);
					offset = new Vector2(offset.X, -offset.Y); // I do not know why, but Y must be inverted

					// place formation spot
					FormationMarkers[file, rank].Position = destination + offset;
				}
			}
		}
	}
}