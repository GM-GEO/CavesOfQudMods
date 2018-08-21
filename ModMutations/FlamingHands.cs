using System;
using System.Collections.Generic;
using XRL.UI;
using ConsoleLib.Console;
 
namespace XRL.World.Parts.Mutation
{
    [Serializable]
    class FlamingHands : BaseMutation
    {
        public FlamingHands()
        {
            Name = "FlamingHands";
            DisplayName = "Flaming Hands";
        }
 
        public Guid FlamingHandsActivatedAbilityID = Guid.Empty;
        public ActivatedAbilityEntry FlamingHandsActivatedAbility = null;
 
        public override void Register(GameObject Object)
        {
            Object.RegisterPartEvent(this, "BeginEquip");
            Object.RegisterPartEvent(this, "CommandFlamingHands");
            Object.RegisterPartEvent(this, "AIGetOffensiveMutationList");
        }
 
        public override string GetDescription()
        {
            return "You emit jets of flame from your hands.";
        }
 
        public override string GetLevelText(int Level)
        {
            string Ret = "Emits a 9-square ray of flame in the direction of your choice\n";
            Ret += "Cooldown: 10 rounds\n";
            Ret += "Damage: " + Level + "d6\n";
            Ret += "Cannot wear gloves";
            return Ret;
        }
 
        public void Flame(Cell C, ScreenBuffer Buffer)
        {
            string Damage = Level + "d6";
 
            Body pBody = ParentObject.GetPart("Body") as Body;
            int nHandCount = pBody.GetPart("Hands").Count-1;
            if (nHandCount > 0) Damage += "+" + nHandCount.ToString();
 
            if (C != null)
            {
                List<GameObject> Objects = C.GetObjectsInCell();
 
                foreach (GameObject GO in Objects)
                {
                    if( GO.PhasedMatches( ParentObject ) )
                    {
                        GO.FireEvent(Event.New("TemperatureChange", "Amount", 310 + (30 * Level), "Owner", ParentObject));
                        for (int x = 0; x < 5; x++) GO.ParticleText("&r" + (char)(219 + Rules.Stat.Random(0, 4)), 2.9f, 1);
                        for (int x = 0; x < 5; x++) GO.ParticleText("&R" + (char)(219 + Rules.Stat.Random(0, 4)), 2.9f, 1);
                        for (int x = 0; x < 5; x++) GO.ParticleText("&W" + (char)(219 + Rules.Stat.Random(0, 4)), 2.9f, 1);
                    }
                }
 
                foreach (GameObject GO in C.GetObjectsWithPart("Combat"))
                {
                    if( GO.PhasedMatches( ParentObject ) )
                    {
                        Damage Dmg = new Damage(Rules.Stat.Roll(Damage));
                        Dmg.AddAttribute("Fire");
                        Dmg.AddAttribute("Heat");
 
                        Event eTakeDamage = Event.New("TakeDamage");
                        eTakeDamage.AddParameter("Damage", Dmg);
                        eTakeDamage.AddParameter("Owner", ParentObject);
                        eTakeDamage.AddParameter("Attacker", ParentObject);
                        eTakeDamage.AddParameter("Message", "from %o flames!");
 
                        GO.FireEvent(eTakeDamage);
                    }
                }
            }
 
            Buffer.Goto(C.X, C.Y);
            string sColor = "&C";
            int r = Rules.Stat.Random(1, 3);
            if (r == 1) sColor = "&R";
            if (r == 2) sColor = "&r";
            if (r == 3) sColor = "&W";
 
            r = Rules.Stat.Random(1, 3);
            if (r == 1) sColor += "^R";
            if (r == 2) sColor += "^r";
            if (r == 3) sColor += "^W";
 
            if( C.ParentZone == XRL.Core.XRLCore.Core.Game.ZoneManager.ActiveZone )
            {
                r = Rules.Stat.Random(1, 3);
                Buffer.Write(sColor + (char)(219 + Rules.Stat.Random(0, 4)));
                Popup._TextConsole.DrawBuffer(Buffer);
                System.Threading.Thread.Sleep(10);
            }
        }
 
        public override bool FireEvent(Event E)
        {
            if (E.ID == "AIGetOffensiveMutationList")
            {
                int Distance = (int)E.GetParameter("Distance");
                GameObject Target = E.GetParameter("Target") as GameObject;
                List<XRL.World.AI.GoalHandlers.AICommandList> CommandList = (List<XRL.World.AI.GoalHandlers.AICommandList>)E.GetParameter("List");
 
                if (FlamingHandsActivatedAbility != null && FlamingHandsActivatedAbility.Cooldown <= 0 && Distance <= 9 && ParentObject.HasLOSTo(Target) ) CommandList.Add(new XRL.World.AI.GoalHandlers.AICommandList("CommandFlamingHands", 1));
                return true;
            }
 
            if (E.ID == "CommandFlamingHands")
            {
                ScreenBuffer Buffer = new ScreenBuffer(80, 25);
                Core.XRLCore.Core.RenderMapToBuffer(Buffer);
 
                List<Cell> TargetCell = PickLine(9, AllowVis.Any);
                if (TargetCell == null) return true;
                if (TargetCell.Count <= 0) return true;
 
                if (TargetCell != null)
                {
                    if (TargetCell.Count == 1)
                    {
                        if (ParentObject.IsPlayer())
                            if (UI.Popup.ShowYesNoCancel("Are you sure you want to target yourself?") != DialogResult.Yes)
                            {
                                return true;
                            }
                    }
 
                    if( FlamingHandsActivatedAbility != null ) FlamingHandsActivatedAbility.Cooldown = 110;
                    ParentObject.FireEvent(Event.New("UseEnergy", "Amount", 1000, "Type", "Physical Mutation"));
                     
                    for (int x = 0; x < 9 && x < TargetCell.Count; x++)
                    {
                        if (TargetCell.Count == 1 || TargetCell[x] != ParentObject.pPhysics.CurrentCell)
                        Flame(TargetCell[x],Buffer);
 
                        foreach( GameObject GO in TargetCell[x].GetObjectsWithPart("Physics") )
                        {
                            if (GO.pPhysics.Solid)
                            {
                                x = 999;
                                break;
                            }
                        }
                    }
                }
            }
 
            if (E.ID == "BeginEquip")
            {
                GameObject Equipment = E.GetParameter("Object") as GameObject;
                string BodyPartName = E.GetParameter("BodyPartName") as string;
 
                if (BodyPartName == "Hands")
                {
                    if (IsPlayer())
                    {
                        UI.Popup.Show("Your flaming hands prevents you from equipping " + Equipment.DisplayName + "!");
                    }
 
                    E.bCancelled = true;
                    return false;
                }
            }
 
            return true;
        }
 
 
        int OldFlame = -1;
        int OldVapor = -1;
 
        public override bool ChangeLevel(int NewLevel)
        {
            Physics pPhysics = ParentObject.GetPart("Physics") as Physics;
 
            TemperatureOnHit pTemp = FlamesObject.GetPart("TemperatureOnHit") as TemperatureOnHit;
            pTemp.Amount =  (Level*2) + "d8";
 
            return base.ChangeLevel(NewLevel);
        }
 
        public override bool Mutate(GameObject GO, int Level)
        {
            Unmutate(GO);
 
            ActivatedAbilities pAA = GO.GetPart("ActivatedAbilities") as ActivatedAbilities;
            Physics pPhysics = GO.GetPart("Physics") as Physics;
 
            if (pPhysics != null)
            {
                OldFlame = pPhysics.FlameTemperature;
                OldVapor = pPhysics.VaporTemperature;
            }
             
            Body pBody = GO.GetPart("Body") as Body;
            if (pBody != null)
            {
                GO.FireEvent(Event.New("CommandForceUnequipObject", "BodyPartName", "Hands"));
                FlamesObject = GameObjectFactory.Factory.CreateObject("Ghostly Flames");
                Event eCommandEquipObject = Event.New("CommandEquipObject");
                eCommandEquipObject.AddParameter("Object", FlamesObject);
                eCommandEquipObject.AddParameter("BodyPartName", "Hands");
                GO.FireEvent(eCommandEquipObject);
            }
 
            FlamingHandsActivatedAbilityID = pAA.AddAbility("Flaming Hands", "CommandFlamingHands", "Physical Mutation");
            FlamingHandsActivatedAbility = pAA.AbilityByGuid[FlamingHandsActivatedAbilityID];
            return true;
        }
 
        public GameObject FlamesObject = null;
 
        public override bool Unmutate(GameObject GO)
        {
            Physics pPhysics = GO.GetPart("Physics") as Physics;
 
            if (pPhysics != null)
            {
                if (OldFlame != -1) pPhysics.FlameTemperature = OldFlame;
                if (OldVapor != -1) pPhysics.BrittleTemperature = OldVapor;
                OldFlame = -1;
                OldVapor = -1;
 
                pPhysics.Temperature = 25;
            }
             
            Body pBody = GO.GetPart("Body") as Body;
            if (pBody != null)
            {
                BodyPart pMainBody = pBody.GetPartByName("Hands");
                if( pMainBody != null )
                if (pMainBody.Equipped != null)
                {
                    if (pMainBody.Equipped.Blueprint == "Ghostly Flames")
                    {
                        pMainBody.Equipped.FireEvent(Event.New("Unequipped", "UnequippingObject", ParentObject, "BodyPart", pMainBody));
                        pMainBody.Unequip();
                    }
                }
            }
 
            if (FlamingHandsActivatedAbilityID != Guid.Empty)
            {
                ActivatedAbilities pAA = GO.GetPart("ActivatedAbilities") as ActivatedAbilities;
                pAA.RemoveAbility(FlamingHandsActivatedAbilityID);
                FlamingHandsActivatedAbilityID = Guid.Empty;
            }
 
            return true;
        }
    }
}
