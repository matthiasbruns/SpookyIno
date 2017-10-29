using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour {

    public static GameSceneManager Instance;
    public GameSceneManager() {
        Instance = this;
    }

    public int DungeonSeed { get; private set; }
    public DungeonBoss LoadingBoss { get; private set; }

    private Scene GameScene;
    private Scene CurrentScene;

    public RectTransform SceneTransition;

    private PlayerActor Player;
    private Vector3 PrevPlayerPos;

    void Awake() {
        if (SceneTransition == null)
            SceneTransition = GameObject.Find("SceneTransition").GetComponent<RectTransform>();
        GameScene = SceneManager.GetActiveScene();
        SceneManager.sceneLoaded += OnSceneLoad;
        GameManager.Instance.InDungeon = false;
        SwitchToOverworldScene();

        if (Player == null)
            Player = FindObjectOfType<PlayerActor>();
    }

    void Update() {

    }

    public void SwitchToOverworldScene() {
        StartCoroutine(_SwitchToScene("Scenes/GameOutside", false));
    }
    public void SwitchToDungeonScene(int seed, DungeonBoss boss) {
        DungeonSeed = seed;
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
        SceneTransition.anchoredPosition = new Vector2(0f, -2000f);

        Time.timeScale = 0f;

        if (dungeon)
            PrevPlayerPos = Player.transform.position;

        if (!string.IsNullOrEmpty(CurrentScene.name))
            yield return SceneManager.UnloadSceneAsync(CurrentScene);
        yield return SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        GameManager.Instance.InDungeon = dungeon;

        if (dungeon) {
            while (DungeonGeneratorNeo.Instance == null || !DungeonGeneratorNeo.Instance.Done)
                yield return null;
        }

        if (!dungeon)
            Player.transform.position = PrevPlayerPos + new Vector3(0f, -4f, 0f);
        else
            Player.transform.position = new Vector3(0f, 0f, 0f);

        Time.timeScale = 1f;

        for (float t = 0f; t < dur; t += Time.unscaledDeltaTime) {
            float f = t / dur;
            SceneTransition.anchoredPosition = new Vector2(
                0f,
                Mathf.Lerp(-2000f, -6000f, f)
            );
            yield return null;
        }
        SceneTransition.anchoredPosition = new Vector2(0f, -6000f);
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Game")
            return;

        loading = null;
        CurrentScene = scene;
        SceneManager.SetActiveScene(scene);
    }

}
