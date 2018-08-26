### Warden Gunner Vargmunr

A frozen warden from out of space and time, content in Qud for the time being.

### Steps
1. Create `Quests.xml` and define three quests with corrosponding CS classes.
    1. "Iced Man" retrieve items for Gunnar and be rewarded with XP
    2. "Warriors Path" kill random enemies and be rewarded with a random artifact.
    3. "A Worthy Foe" duel the Warden until either participant is badly wounded. If failure player must wait until the warden is healed, if success given a custom weapon and enough reputation to convince the warden to join the player.
2. Create `ObjectBlueprints.xml`with additional entries
    1. Add Warden Gunnar Vargmunr with Armor, Weapons, Mutations, and Water.
    2. Add Spider Prismgun laser rifle weapon that Warden Gunnar Vargmunr will carry.
3. Create `Conversations.xml` and define dialog trees.
    1. Start Trees: Start, Start after all three quests started
    2. End Trees: End, End after all three quests started
    3. Personal Trees: Himself, His Rifle.

#### Conversation.xml References

1. **Node Tags** make up the bulk of conversations. They can have specialized attributes for complex conversations or quests, as well as inner child tags that define conversation choices and the text itself.
   1. **ID Attribute** is the primary linkage beteen conversation choices, with special  attributes to both ID and send the codebase to a particular ID. Multiple nodes can have the same ID in conjunction with other attributes for complex quests and conversations.
   2. **IfNotHaveQuest Attribute** is used to display a node if you have not started a specific quest. It is also the linkage between a conversation and a quest in your `Quests.xml` file. The Name attribute of the quest tag must be the same as this attribute.
   3. **IfHaveQuest Attribute** is used to display an alternate node if you have already started a quest. Like IfNotHaveQuest it is the linkage between a conversation and a quest in your `Quests.xml` file. The Name attribute of the quest tag must be the same as this attribute.
   3. **CompleteQuestStep Attribute** This will complete the step defined in your `Quest.xml` file. This attribute will trigger the `OnStepComplete(string StepName)` method in your CS quest class if you have it implemented.
   4. **IfNotFinishedQuest Attribute** is used to display a node when the player has started a quest but has not finished it. Used in conjunction with the IfHaveQuest attribute.
2. **Text Tags** define the text that is displayed in the dialog screen. They respect the color formatting rules outlined in Text Color Codes & Object Rendering on the 
Caves of Qud - Technical Guidebook.
3. **Choice Tags** define the choices that can be selected in a coversation.
    1. **GotoID Attribute** is used to define what the next node tag is for the conversation. There are two specialized GotoID tag values, Start and End. Start is the first node that is displayed in the conversation, which End will stop the conversation. End also has the EndFight option, which makes the NPC the PC was conversing with hostile.
    2. **StartQuest Attribute** is used to start a quest that is defined in the `Quests.xml` file. The class defined on the Manager attribute will then have the `OnQuestAdded` method. This method can be implemented if you have specific logic to run (the BringArgyveAKnicknack class checks if the player is holding a knicknack, for example).
    3. **CompleteQuestStep Attribute** will complete the step defined in your `Quest.xml` file. This attribute will trigger the `OnStepComplete(string StepName)` method in your CS quest class if you have it implemented.
    4. **StartQuest Attribute** is used to start the quest. This will also trigger the `OnQuestAdded()` method in your quest CS file.

#### Quests.xml References

1. **Quest Tags** define the body of the quest and have steps primarily as their inner tags.
    1. **Manager Attribute** is used to define the class is used for various events with the quest. It must be named the same as the quest CS class, and the class must extend the QuestManager class.
    2. **Accomplishment Attribute** is used to define the text displayed on your death screen.
    3. **Level Attribute**
2. **Step Tags** are used to define the steps that are involved in a quest. The `OnStepComplete(string StepName)` method passes this attribute as the step name parameter.
    1. **XP Attribute** is used to define how much XP is awarded to the player upon completion.
    2. **Text Inner Tag** is used to define the text that is displayed in the journal for the quest. It is different from an Attribute because it is contained within the Step tag.

