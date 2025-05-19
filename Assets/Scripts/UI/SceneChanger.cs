using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GoToScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void NextScene()
    {
        SceneManager.LoadScene("StoryScene");
    }

    public void TheNextScene()
    {
        SceneManager.LoadScene("InGameScene");
    }
}
