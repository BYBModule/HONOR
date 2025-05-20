using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum Scene
    {
        MainMenuScene,
        StoryScene,
        LobbyScene,
        ConnectingScene,
        InGameScene,
    }
    private static Scene nextScene;
    public static SceneLoader Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void SettingNextScene(Scene next)
    {
        nextScene = next;
    }
    public void StartNextScene()
    {
        SceneManager.LoadScene(nextScene.ToString());
    }
    public void GameStart()
    {
        SettingNextScene(Scene.LobbyScene);
        StartNextScene();
    }
}
