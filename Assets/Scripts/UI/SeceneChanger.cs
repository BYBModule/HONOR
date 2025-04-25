using UnityEngine;
using UnityEngine.SceneManagement;

public class SeceneChanger : MonoBehaviour
{
    public void GoToScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    public void NectScene()
    {
        SceneManager.LoadScene("InGameScene");
    }
}
