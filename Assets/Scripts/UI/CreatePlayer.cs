using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreatePlayer : MonoBehaviour
{
    [SerializeField] private GameObject classImage;
    [SerializeField] private List<GameObject> classPrefab;
    [SerializeField] private Button selectedButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    public Player.ClassName className;
    public string currentName;
    public GameObject currentClassPrefab;
    public static CreatePlayer Instance { get; private set; }
    public int classIndex;
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        if (selectedButton != null)
        {
            selectedButton.onClick.AddListener(() =>
            {
                CreatePlayerClass();
            });
            nextButton.onClick.AddListener(() =>
            {
                NextButtonClick();
            });
            backButton.onClick.AddListener(() =>
            {
                BackButtonClick();
            });
        }
        
    }
    public void NextButtonClick()
    {
        ClassChange(false);
        if (classIndex == 5)
        {
            classIndex = 0;
        }
        else
        {
            classIndex++;
        }
        ClassChange(true);
    }
    public void BackButtonClick()
    {
        ClassChange(false);
        if (classIndex == 0)
        {
            classIndex = 5;
        }
        else
        {
            classIndex--;
        }
        ClassChange(true);
    }
    public void ClassChange(bool on)
    {
        classImage.transform.GetChild(classIndex).gameObject.SetActive(on);   
    }
    private void CreatePlayerClass()
    {
        currentClassPrefab = classPrefab[classIndex];
        Instantiate(currentClassPrefab, transform.position, Quaternion.identity);
        className = (Player.ClassName)(classIndex + 1);
        SceneLoader.Instance.SettingNextScene(SceneLoader.Scene.ConnectingScene);
        SceneLoader.Instance.StartNextScene();
    }
}
