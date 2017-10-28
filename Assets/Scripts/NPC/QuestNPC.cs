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
            if(!didReadQuest){
                output.Content = npcConfig.questText;
                didReadQuest = true;
            } else if(!didFinishQuest){
                // TODO check if quest can be finished
                output.Content = npcConfig.questReminderTexts[Random.Range(0, npcConfig.questReminderTexts.Length)];
            }
            
        }
    }

}