using System;
using System.Collections.Generic;
using System.Text;
 
using XRL.Rules;
using XRL.Messages;
using ConsoleLib.Console;

namespace XRL.World.Parts.Mutation
{
	[Serializable]
	public class GEO_ThirdEye : BaseMutation
	{
		public int Bonus;

		public GEO_ThirdEye()
		{
		this.Name = "GEO_ThirdEye";
		this.DisplayName = "Third Eye";
		this.Type = "Mental";
		}

		public override void Register(GameObject Object)
		{
		}

		public override string GetDescription()
		{
			return "You possess a third eye.";
		}

		public override string GetLevelText(int Level)
		{
			string text = "+" + (2 + (Level - 1) / 2) + " Ego\n";
			
			return text;
		}

		public override bool ChangeLevel(int NewLevel)
		{
			ParentObject.Statistics["Ego"].BaseValue -= Bonus;
			Bonus = 2 + (base.Level - 1) / 2;
			ParentObject.Statistics["Ego"].BaseValue += Bonus;
			return base.ChangeLevel(NewLevel);
		}

		public override bool Mutate(GameObject GO, int Level)
		{
			ChangeLevel(Level);
			return base.Mutate(GO, Level);
		}

		public override bool Unmutate(GameObject GO)
		{
			GO.Statistics["Ego"].BaseValue -= Bonus;
			return base.Unmutate(GO);
		}
	}
}