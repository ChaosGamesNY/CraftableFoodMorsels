using System;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Objects.CookingObjects;
using Sims3.SimIFace;
using Sims3.Gameplay.Utilities;
using Sims3.Gameplay.ActorSystems;
using Sims3.Gameplay.Objects.FoodObjects;
using Sims3.Gameplay;
using Sims3.Gameplay.Objects.FoodObjects.MonoChaos.CraftableFoodMorsels;
using Sims3.Gameplay.Autonomy;
using Sims3.Gameplay.EventSystem;
using Sims3.Gameplay.Skills;

namespace MonoChaos
{
	public class CraftableFoodMorsels
	{
		[Tunable]
		protected static bool kInstantiator = false;

		static CraftableFoodMorsels ()
		{
			World.OnWorldLoadFinishedEventHandler += new EventHandler(OnWorldLoadFinishedHandler);
		}

		public static void OnWorldLoadFinishedHandler(object sender, System.EventArgs e)
		{
			foreach (ServingContainerGroup groupServingContainer in Sims3.Gameplay.Queries.GetObjects<ServingContainerGroup>())
			{
				if (groupServingContainer != null)
				{
					AddInteractions(groupServingContainer);
				}
			}
			sCookedMealListener = EventTracker.AddListener (EventTypeId.kCookedMeal, new ProcessEventDelegate (OnCookedMeal));
		}

		private static EventListener sCookedMealListener = null;

		protected static ListenerAction OnCookedMeal(Event e)
		{
			ServingContainerGroup groupServingContainer = e.TargetObject as ServingContainerGroup;
			if (groupServingContainer != null)
			{
				AddInteractions(groupServingContainer);
			}
			return ListenerAction.Keep;
		}

		public static void AddInteractions(ServingContainerGroup groupServingContainer)
		{
			groupServingContainer.AddInteraction(CraftFoodMorsels.Singleton);
			groupServingContainer.AddInventoryInteraction (CraftFoodMorsels.Singleton);
		}

		private sealed class CraftFoodMorsels : Interaction<Sim, ServingContainer>
		{
			private sealed class Definition : InteractionDefinition<Sim, ServingContainer, CraftFoodMorsels>
			{
				protected override string GetInteractionName(Sim actor, ServingContainer food, InteractionObjectPair interaction)
				{
					return Localization.LocalizeString("MonoChaos/CraftableFoodMorsels:InteractionName", new object[0]);
				}
				protected override bool Test(Sim actor, ServingContainer food, bool isAutonomous, ref GreyedOutTooltipCallback greyedOutTooltipCallback)
				{
					if (!isAutonomous)
					{
						return true;
					}
					return false;
				}
			}

			public static readonly InteractionDefinition Singleton = new Definition();

			protected override bool Run()
			{
				int num = Target.GetNumServingsLeft();
				for (var i = 0; i < num; i++) {
					CraftedMorsel craftedMorsel = CraftedMorsel.CreateMorsel(Target.CookingProcess);
					Actor.Inventory.TryToAdd (craftedMorsel);
				}
				Target.Destroy ();
				return true;
			}

			protected override bool RunFromInventory()
			{
				return Run();
			}

		}
	}
}

