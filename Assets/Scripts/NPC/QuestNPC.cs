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

        foreach(QuestOutputComponent output in outputComponents) {
            if(!didReadQuest && (npcConfig.startCondition == null || npcConfig.startCondition.Check(gameObject))) {
                output.Content = npcConfig.questText;
                didReadQuest = true;
                executer.GetComponent<ObjectivesComponent>().CurrentObjectiveId = npcConfig.questId;
            } else if(didReadQuest && !didFinishQuest && npcConfig.finishCondition.Check(gameObject)) {
                output.Content = npcConfig.questSuccessText;
                didFinishQuest = true;
                executer.GetComponent<ObjectivesComponent>().FinishCurrentQuest();
            } else if (didReadQuest && !didFinishQuest){
                output.Content = npcConfig.questReminderTexts[Random.Range(0, npcConfig.questReminderTexts.Length)];
            } else {
                output.Content = npcConfig.chitChatTexts[Random.Range(0, npcConfig.chitChatTexts.Length)];
            }
        }
    }
}