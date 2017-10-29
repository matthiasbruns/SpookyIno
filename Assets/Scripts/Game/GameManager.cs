using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	private static GameManager instance;
	public static GameManager Instance {
		get{
			return instance;
		}
	}

	void Awake() {
        Resources.FindObjectsOfTypeAll<Canvas>()[0].gameObject.SetActive(true);

		if(instance != null){
			Debug.LogError("GameManager instance is already present");
			Destroy(gameObject);
			return;
		}

		instance = this;
	}

    private bool _InDungeon;
    public bool InDungeon {
        get {
            return _InDungeon;
        }
        set {
            _InDungeon = value;
            RenderSettings.ambientLight = value ? DungeonAmbientLight : OutsideAmbientLight;
        }
    }

    public Color OutsideAmbientLight;
    public Color DungeonAmbientLight;

    public InventoryItemList itemDatabase;
    public ObjectiveList objectiveDatabase;
    public AudioManager audioDatabase;

}
