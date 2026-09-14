using System;
using Sims3.SimIFace;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Autonomy;
using Sims3.UI;
using Sims3.Gameplay.Utilities;
using Sims3.Gameplay.Socializing;

namespace MonoChaos
{
	public class RelValueInteraction
	{
		[Tunable]
		protected static bool kInstantiator = false;

		static RelValueInteraction ()
		{
			World.OnWorldLoadFinishedEventHandler += new EventHandler(OnWorldLoadFinishedHandler);
		}
		public static void OnWorldLoadFinishedHandler(object sender, System.EventArgs e)
		{
			foreach (Sim sim in Sims3.Gameplay.Queries.GetObjects<Sim>())
			{
				if (sim != null)
				{
					AddInteractions(sim);
				}
			}
		}
		protected static ListenerAction OnSimInstantiated(Event e)
		{
			return ListenerAction.Keep;
		}
		public static void AddInteractions(Sim sim)
		{
			sim.AddInteraction(ShowRelationshipValue.Singleton);
		}
		private sealed class ShowRelationshipValue : ImmediateInteraction<Sim, Sim>
		{
			public static readonly InteractionDefinition Singleton = new Definition();
			protected override bool Run()
			{
				Relationship relationship = Relationship.Get(base.Actor, base.Target, createIfNone: true);
				string message = (base.Actor.Name + " + " + base.Target.Name + " = " + relationship.CurrentLTR.ToString());
				StyledNotification.Show(new StyledNotification.Format(message, StyledNotification.NotificationStyle.kGameMessagePositive));
				return true;
			}
			[DoesntRequireTuning]
			private sealed class Definition : ImmediateInteractionDefinition<Sim, Sim, ShowRelationshipValue>
			{
				protected override string GetInteractionName(Sim actor, Sim target, InteractionObjectPair interaction)
				{
					return Localization.LocalizeString("MonoChaos/RelValueInteraction:InteractionName", new object[0]);
				}
				protected override bool Test(Sim actor, Sim target, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					return !isAutonomous;
				}
			}
		}
	}
}

