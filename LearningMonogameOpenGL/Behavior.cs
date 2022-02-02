namespace LearningMonogameOpenGL
{
	public class Behavior
	{
		public enum DoType { Attack, Target, Approach }

		public enum IfType
		{
			None,    // unconditional behavior ("if" clause not used)
			Subject, // the GameObject indicated by To
			Focus,
			MyTarget,
			AnyAlly,
			AnyEnemy
		}

		public enum IsType
		{
			None, // unconditional behavior ("if" clause not used)
			Wounded,
			AffectedByStatus
		}

		public enum ToType
		{
			FormationMarker,
			Focus,
			MyTarget,
			AnyAlly,
			AnyEnemy
		}

		// shorthand constructor for unconditional behaviors
		public Behavior(DoType @do, ToType to)
		{
			Do = @do;
			To = to;
			If = IfType.None;
			Is = IsType.None;
		}

		// full constructor for conditional behaviors
		public Behavior(DoType @do,
		                ToType to,
		                IfType @if,
		                IsType @is)
		{
			Do = @do;
			To = to;
			If = @if;
			Is = @is;
		}

		public DoType Do { get; }
		public ToType To { get; }
		public IfType If { get; }
		public IsType Is { get; }
	}
}