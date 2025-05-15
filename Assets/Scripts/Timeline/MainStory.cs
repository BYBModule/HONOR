using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStory : MonoBehaviour
{
    void OEnable()
    {
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }
}
