using UnityEngine;

public class Player : MonoBehaviour
{
    public enum ClassName
    {
        Warrior,
        Archor,
        Thief,
        Paladin,
        Priest,
        Millionaire,
    }
    public static Player Instance
    {
        get;
        private set;
    }
    public Status status;

    void Awake()
    {
        Instance = this;

    }
}
