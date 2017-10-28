using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPCConfig : ScriptableObject {
	public int questId;
	public string questText;
    public string[] questReminderTexts;
    public string questSuccessText;
    public string[]  chitChatTexts;
    public ObjectiveCondition startCondition;
    public ObjectiveCondition finishCondition;

}
