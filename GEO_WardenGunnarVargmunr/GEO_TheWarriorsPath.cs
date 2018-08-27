using System;
using XRL.Core;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.QuestManagers
{
	[Serializable]
	public class GEO_TheWarriorsPath : QuestManager
	{
		public int snapJawWarlordsKilled;

		public override void OnQuestAdded()
		{
			snapJawWarlordsKilled = 0;
			//Registers the quest as a listener to the game event engine. Listening for the PartEvent "Killed".
			XRLCore.Core.Game.Player.Body.RegisterPartEvent(this, "Killed");
		}

		public override void OnQuestComplete()
		{
			//Unregister the questmanager so it doesn't keep firing after the quest is completed.
			XRLCore.Core.Game.Player.Body.RemovePart(this);
		}

		public override bool FireEvent(Event E)
		{
			//E describes an event that the QuestManager was listening for. In this case, the QuestManager was listening for the "Killed" event.
			if (E.ID == "Killed")
			{
				//GameObject describes the thing (NPC, Equipment, Player, etc) that the event went off for.
				GameObject gameObject = E.GetParameter("Object") as GameObject;

				//The gameObject.Blueprint is a memory representation of the ObjectBlueprint.xml part loaded for the gameObject the event is firing for.
				if (gameObject.Blueprint.Contains("Snapjaw Warlord"))
				{
					snapJawWarlordsKilled++;

					//Check for number of warlords killed.
					if (snapJawWarlordsKilled >= 10)
					{
						//Finish the quest step once 
						XRLCore.Core.Game.FinishQuestStep("The Warriors Path", "Kill 10 Snapjaw Warlords");
					}
					else
					{
						//Update the quest step text to track the warlords.
						XRLCore.Core.Game.Quests["The Warriors Path"].StepsByID["Kill 10 Snapjaw Warlords"].Text = "Kill Ten Snapjaw Warlords. " + snapJawWarlordsKilled + "/10 defeated";
					}
				}
			}
			return true;

		}
	}
}