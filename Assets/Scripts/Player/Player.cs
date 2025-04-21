using UnityEngine;

public class Player : MonoBehaviour
{
    // 직업명
    public enum ClassName
    {
        Warrior,
        Archor,
        Thief,
        Paladin,
        Priest,
        Millionaire,
    }
    // 플레이어 공격력
    public int attackDamage = 0;
    // 플레이어 클래스스
    public ClassName playerClass;
    // 플레이어 인스턴스 생성
    public static Player Instance
    {
        get;
        private set;
    }
    // 플레이어 스텟
    public Status status;
    void Awake()
    {
        Instance = this;
        switch(playerClass)
        {
            // 전사를 선택했다면
            case ClassName.Warrior :
                new Warrior(this);
                break;
            // 궁수를 선택했다면
            case ClassName.Archor :
                new Archor(this);
                break;
            // 도적을 선택했다면
            case ClassName.Thief :
                new Thief(this);
                break;
            // 성기사를 선택했다면
            case ClassName.Paladin :
                new Paladin(this);
                break;
            // 사제를 선택했다면
            case ClassName.Priest :
                new Priest(this);
                break;
            // 대부호를 선택했다면
            case ClassName.Millionaire :
                new Millionaire(this);
                break;
        }

    }
}
