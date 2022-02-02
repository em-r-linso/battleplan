using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace LearningMonogameOpenGL
{
	public class Line
	{
		public static List<Line> Registry;

		public Line(Sprite sprite, Vector2 origin, Vector2 termination)
		{
			Sprite      = sprite;
			Origin      = origin;
			Termination = termination;

			RebuildLine(origin, termination);

			// register
			Registry ??= new List<Line>();
			Registry.Add(this);
		}

		public Sprite    Sprite      { get; set; }
		Vector2          Origin      { get; }
		Vector2          Termination { get; }
		public Vector2[] Segments    { get; set; }

	public	void RebuildLine(Vector2 origin, Vector2 termination)
		{
			var distance     = (termination - origin).Length();
			var segmentCount = (int)(distance / GameData.Config.LineSegmentSpacing);
			var rotation     = origin.RotationTo(termination);

			// TODO: reuse code with Formation (this is a lot like a 1xN formation)
			Segments = new Vector2[segmentCount];
			for (var i = 0; i < segmentCount; i++)
			{
				var offset = new Vector2(0, i * GameData.Config.LineSegmentSpacing);
				offset =  Vector2.Transform(offset, rotation);
				offset =  new Vector2(offset.X, -offset.Y);

				offset = -offset;

				Segments[i] = origin + offset;
				Debug.WriteLine(Segments[i]);
			}
		}
	}
}