using UnityEngine;
using System.Collections.Generic;

class QuestNPC : NPC {

    public QuestNPCConfig npcConfig;
    
    private QuestOutputComponent[] outputComponents;
    private bool didReadQuest;
    private bool didFinishQuest;

    void Awake(){
		outline = gameObject.GetComponent<SpriteOutline>();
        outputComponents = GameObject.FindObjectsOfType<QuestOutputComponent>();
        didReadQuest = didFinishQuest = false;
    }

    public override void Highlight(GameObject executer, bool activate){
        base.Highlight(executer, activate);

        if (!activate) {
            foreach(QuestOutputComponent output in outputComponents) {
                output.Content = null;
            }
        }
    }

    public override void Use(UnityEngine.GameObject executer){
        base.Use(executer);
        Debug.Log("Use", this);

        foreach(QuestOutputComponent output in outputComponents) {
            if(!didReadQuest && (npcConfig.startCondition == null || npcConfig.startCondition.Check(executer))) {
                Debug.Log("QUEST TEXT", this);
                output.Content = npcConfig.questText;
                didReadQuest = true;
                didFinishQuest = false;
                executer.GetComponent<ObjectivesComponent>().CurrentObjectiveId = npcConfig.questId;
            } else if (didReadQuest && !didFinishQuest && !npcConfig.finishCondition.Check(executer)){                
                Debug.Log("REMINDER TEXT", this);
                output.Content = npcConfig.questReminderTexts[Random.Range(0, npcConfig.questReminderTexts.Length)];
            } else if(didReadQuest && !didFinishQuest && npcConfig.finishCondition.Check(executer)) {
                Debug.Log("FINISH TEXT", this);
                output.Content = npcConfig.questSuccessText;
                didFinishQuest = true;
                executer.GetComponent<ObjectivesComponent>().FinishCurrentQuest();
            } else {                
                Debug.Log("CHIT CHAT TEXT", this);
                output.Content = npcConfig.chitChatTexts[Random.Range(0, npcConfig.chitChatTexts.Length)];
                if(npcConfig.nextConfig != null){
                    npcConfig = npcConfig.nextConfig;
                    didReadQuest = false;
                    didFinishQuest = false;
                }
            }
        }
    }
}