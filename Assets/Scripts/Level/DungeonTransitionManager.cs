using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonTransitionManager : MonoBehaviour {

    Vector2 dungeonEnterLocation = Vector2.zero;
    Vector2 offsetVector;
    Vector3 center;
    Vector3 shift;
    public GameObject player;

    void Awake() {
        player = GameObject.Find("Player");
        DontDestroyOnLoad(player);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (dungeonEnterLocation == Vector2.zero) {
            dungeonEnterLocation = other.transform.position;
            center = other.bounds.center;
            shift = 2*other.bounds.extents;
            EnterDungeon();
        } else {
            dungeonEnterLocation = Vector2.zero;
            LeaveDungeon();
        }
    }

    void EnterDungeon() {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        offsetVector = player.GetComponent<Rigidbody2D>().velocity.normalized;
        player.transform.position = new Vector2(center.x + offsetVector.x * shift.x * 1.5f, center.y + offsetVector.y * shift.y * 1.5f);
        //generate Dungeon
    }

    void LeaveDungeon() {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
        offsetVector = player.GetComponent<Rigidbody2D>().velocity.normalized;
        player.transform.position = new Vector2(center.x + offsetVector.x * shift.x * 1.5f, center.y + offsetVector.y * shift.y * 1.5f);
    }
}
