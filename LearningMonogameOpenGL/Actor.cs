using System;
using Microsoft.Xna.Framework;

namespace LearningMonogameOpenGL
{
	/// <summary>GameObject that has and processes Behaviors.</summary>
	public class Actor : GameObject
	{
		readonly Random _r = new Random();

		public Actor(Sprite     sprite,
		             Vector2    position,
		             float      moveSpeed,
		             float      idleInterval,
		             Behavior[] idleBehaviors,
		             string     name) : base(sprite, position, moveSpeed)
		{
			IdleInterval  = idleInterval;
			IdleBehaviors = idleBehaviors;
			Name          = name;
		}

		float             IdleClock       { get; set; }
		float             IdleInterval    { get; }
		Behavior[]        IdleBehaviors   { get; }
		string            Name            { get; }
		public GameObject Target          { get; set; }
		public GameObject FormationMarker { get; set; }

		public override void Update(float time)
		{
			Move(time);
			Idle(time);
		}

		public void Idle(float time)
		{
			IdleClock += time;
			if (IdleClock >= IdleInterval)
			{
				IdleClock -= IdleInterval;
				foreach (var behavior in IdleBehaviors)
					DoBehavior(behavior);
			}
		}

		void DoBehavior(Behavior behavior)
		{
			GameObject to = null;
			switch (behavior.To)
			{
				case Behavior.ToType.Focus:
					// TODO: groups (or parties) of Actors with foci
					// to = Group.Focus;
					throw new NotImplementedException();
					break;
				case Behavior.ToType.MyTarget:
					to = Target;
					break;
				case Behavior.ToType.AnyAlly:
					throw new NotImplementedException();
					break;
				case Behavior.ToType.AnyEnemy:
					throw new NotImplementedException();
					break;
				case Behavior.ToType.FormationMarker:
					to = FormationMarker;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			switch (behavior.Do)
			{
				case Behavior.DoType.Approach:
					Destination = to.Position;
					IsMoving    = true;
					break;
				case Behavior.DoType.Attack:
					throw new NotImplementedException();
					break;
				case Behavior.DoType.Target:
					Target = to;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}