using System;
using XRL.Core;
using XRL.Messages;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.QuestManagers
{
	[Serializable]
	public class GEO_TheIcedMan : QuestManager
	{
		public override void OnQuestAdded()
		{
			base.Name = "QMGEO_TheIcedMan";
			CountBlazeInjectors();
			XRLCore.Core.Game.Player.Body.AddPart(this);
			XRLCore.Core.Game.Player.Body.RegisterPartEvent(this, "Took");
		}
		public override void OnQuestComplete()
		{
			XRLCore.Core.Game.Player.Body.RemovePart(this);
		}
		public override bool FireEvent(Event E)
		{
			if (E.ID == "Took")
			{
				CountBlazeInjectors();
			}
			return true;
		}
		private static void CountBlazeInjectors()
		{
			int blazeInjectorCount = 0;

			XRLCore.Core.Game.Player.Body.GetPart<Inventory>().ForeachObject(delegate(GameObject GO)
			{
				if (GO.HasPart("BlazeTonic"))
				{
					blazeInjectorCount++;
				}
			});
			if (blazeInjectorCount >= 3)
			{
				XRLCore.Core.Game.FinishQuestStep("The Iced Man", "Find Three Blaze Injectors");
			}
			else
			{
				MessageQueue.AddPlayerMessage("&cYou now have " + blazeInjectorCount + " Blaze Injectors.");
				XRLCore.Core.Game.Quests["The Iced Man"].StepsByID["Find Three Blaze Injectors"].Text = "Find Three Blaze Injectors. " + blazeInjectorCount + "/3 obtained";
			}
		}
	}
}
