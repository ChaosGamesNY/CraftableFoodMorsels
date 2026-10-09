using System;
using Sims3.Gameplay.Objects.FoodObjects;
using Sims3.SimIFace;
using Sims3.Gameplay.Objects.CookingObjects;
using Sims3.Gameplay.Utilities;
using Sims3.Gameplay.Actors;
using Sims3.Gameplay.Interactions;
using Sims3.Gameplay.Autonomy;

namespace Sims3.Gameplay.Objects.FoodObjects.MonoChaos.CraftableFoodMorsels
{
	public class CraftedMorsel : Morsel
	{
		private Quality mQuality;
		private CookingProcess mCookingProcess;
		private string morselType;

		public override bool EatMorselTest(Sim sim, bool bIsAutonomous)
		{
			return true;
		}


		public override void PreConsumptionBehavior(Sim sim, InteractionInstance ii, out bool checkForStuffed) {
			ii.AddMotiveArrow (CommodityKind.Hunger, isPositive: true);
			checkForStuffed = true;
		}

		public override void PostConsumptionBehavior(Sim sim) {
			sim.Motives.ForceSetMax(CommodityKind.Hunger);
		}


		public static CraftedMorsel CreateMorsel(CookingProcess cookingProcess)
		{
			CraftedMorsel craftedMorsel = GlobalFunctions.CreateObjectOutOfWorld("craftedMorsel", cookingProcess.Recipe.CodeVersion) as CraftedMorsel;
			if (craftedMorsel != null)
			{
				craftedMorsel.mCookingProcess = cookingProcess;
				craftedMorsel.mQuality = cookingProcess.Quality;
				craftedMorsel.SetMaterialState();
			}
			return craftedMorsel;
		}

		public override string CatalogName {
			get {
				switch (morselType)
				{
				case "Meat":
					return Localization.LocalizeString ("MonoChaos/CraftableFoodMorsels:MeatMorsel");
				case "Fruit":
					return Localization.LocalizeString ("MonoChaos/CraftableFoodMorsels:FruitMorsel");
				case "Veggie":
					return Localization.LocalizeString ("MonoChaos/CraftableFoodMorsels:VeggieMorsel");
				default:
					return Localization.LocalizeString ("MonoChaos/CraftableFoodMorsels:Morsel");
				};
			}
		}

		private string GetMaterialState()
		{
			foreach (CookedIngredient item in mCookingProcess.ChosenIngredients) {
				if (ItemTypes.IsOfType (item.GetType().ToString(), "Meat")) {
					morselType = "Meat";
					return "meat";
				} else if (ItemTypes.IsOfType (item.GetType().ToString(), "Fruit")) {
					morselType = "Fruit";
					return "fruit";
				} else if (ItemTypes.IsOfType (item.GetType().ToString(), "Vegetable")) {
					morselType = "Veggie";
					return "veggie";
				}
			}
			morselType = "none";
			return "default";
		}

		private void SetMaterialState()
		{
			SetMaterial(GetMaterialState());
		}

		public CraftedMorsel ()
		{
		}
	}
}

