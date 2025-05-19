using UnityEngine;
using UnityEngine.SceneManagement;

public class SeceneChanger : MonoBehaviour
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
