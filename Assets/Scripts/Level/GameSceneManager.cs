using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour {

    public static GameSceneManager Instance;
    public GameSceneManager() {
        Instance = this;
    }

    public DungeonBoss LoadingBoss { get; private set; }

    private Scene GameScene;
    private Scene CurrentScene;

    public RectTransform SceneTransition;

    void Awake() {
        if (SceneTransition == null)
            SceneTransition = GameObject.Find("SceneTransition").GetComponent<RectTransform>();
        GameScene = SceneManager.GetActiveScene();
        SceneManager.sceneLoaded += OnSceneLoad;
        GameManager.Instance.InDungeon = false;
        SwitchToOverworldScene();
    }

    void Update() {

    }

    public void SwitchToOverworldScene() {
        StartCoroutine(_SwitchToScene("Scenes/GameOutside", false));
    }
    public void SwitchToDungeonScene(DungeonBoss boss) {
        LoadingBoss = boss;
        StartCoroutine(_SwitchToScene("Scenes/GameDungeon", true));
    }

    private string loading;
    private IEnumerator _SwitchToScene(string name, bool dungeon) {
        loading = name;

        const float dur = 0.6f;
        for (float t = 0f; t < dur; t += Time.unscaledDeltaTime) {
            float f = t / dur;
            SceneTransition.anchoredPosition = new Vector2(
                0f,
                Mathf.Lerp(2000f, -2000f, f)
            );
            yield return null;
        }

        if (!string.IsNullOrEmpty(CurrentScene.name))
            yield return SceneManager.UnloadSceneAsync(CurrentScene.name);
        yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        GameManager.Instance.InDungeon = dungeon;

        for (float t = 0f; t < dur; t += Time.unscaledDeltaTime) {
            float f = t / dur;
            SceneTransition.anchoredPosition = new Vector2(
                0f,
                Mathf.Lerp(-2000f, -6000f, f)
            );
            yield return null;
        }
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        if (scene.name != loading)
            return;
        loading = null;
        CurrentScene = scene;
        SceneManager.SetActiveScene(scene);
    }

}
