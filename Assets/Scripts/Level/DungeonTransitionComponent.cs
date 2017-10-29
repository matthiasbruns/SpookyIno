using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonTransitionComponent : MonoBehaviour {

    public DungeonBoss Boss = DungeonBoss.Scarecrow;

    void Awake() {
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!enabled || !collision.gameObject.CompareTag("Player"))
            return;
        if (GameManager.Instance.InDungeon)
            GameSceneManager.Instance.SwitchToOverworldScene();
        else
            GameSceneManager.Instance.SwitchToDungeonScene(Boss);
        enabled = false;
    }

}
