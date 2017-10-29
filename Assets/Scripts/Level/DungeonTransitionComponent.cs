using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonTransitionComponent : MonoBehaviour {

    public int Seed;
    public DungeonBoss Boss = DungeonBoss.Scarecrow;

    void Awake() {
        if (Seed == 0)
            Seed = Random.Range(int.MinValue, int.MaxValue);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (!enabled || !collision.gameObject.CompareTag("Player"))
            return;
        if (GameManager.Instance.InDungeon)
            GameSceneManager.Instance.SwitchToOverworldScene();
        else
            GameSceneManager.Instance.SwitchToDungeonScene(Seed, Boss);
        enabled = false;
    }

}
