using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    // 서버에서 받을 플레이어 ID
    public int playerId;
    // 직업명
    public enum ClassName
    {
        // 전사
        Warrior,
        // 궁수
        Archor,
        // 도적
        Thief,
        // 성기사
        Paladin,
        // 사제
        Priest,
        // 대부호
        Millionaire,
    }
    public BattlePhase battlePhase;
    // 플레이어 스텟
    [HideInInspector] public Status status;
    // 플레이어 클래스
    [SerializeField] private ClassName playerClass;
    // 플레이어 데이터를 저장하는 클래스
    public IClass player_Class;
    // 플레이어 데이터
    private PlayerData playerData;
    // 플레이어 데이터 프로퍼티
    public PlayerData Player_Data => playerData;
    // 플레이어 인스턴스
    public static Player Instance
    {
        get;
        private set;
    }
    
    void Awake()
    {
        // 플레이어 인스턴스 생성
        Instance = this;
        // 플레이어 데이터 생성
        playerData = new PlayerData();
    }
    void Update()
    {
        // if(player_Class == null)
        // {
            switch(playerClass)
            {
                // 전사를 선택했다면
                case ClassName.Warrior :
                    player_Class = new Warrior(this);
                    break;
                // 궁수를 선택했다면
                case ClassName.Archor :
                    player_Class = new Archor(this);
                    break;
                // 도적을 선택했다면
                case ClassName.Thief :
                    player_Class = new Thief(this);
                    break;
                // 성기사를 선택했다면
                case ClassName.Paladin :
                    player_Class = new Paladin(this);
                    break;
                // 사제를 선택했다면
                case ClassName.Priest :
                    player_Class = new Priest(this);
                    break;
                // 대부호를 선택했다면
                case ClassName.Millionaire :
                    player_Class = new Millionaire(this);
                    break;
            }
            UpdateHp(0);
            //else
            //{    
            Debug.Log($" 직업 : {playerClass}\n 공격력 : {playerData.attackDamage}\n 체력 : {playerData.playerHp} / {status.maxHp}\n 힘 : {status.strength}\n 지능 : {status.intelligence}\n 적중 : {status.hitRate}\n 회피 : {status.evasion}\n 행운 : {status.luck}");
            if(battlePhase.gameObject.activeSelf == false)
            {
                battlePhase.gameObject.SetActive(true);
            }
            //}
        //}
        
    }

    // 수치를 업데이트 하기 위한 변수
    public void PlayerUpdate()
    {
        player_Class.Set_Status(this, status);
    }
    // 플레이어 사망
    public void PlayerDead()
    {
        if(playerData.playerGold != 0)
        playerData.playerGold -= (int)playerData.playerGold/10;
        UpdateHp(0);    
    }
    public void UpdateHp(int hp)
    {
        if(playerData.playerHp <= 0)
        {
            playerData.playerHp = status.maxHp;
        }
        else
        {
            playerData.playerHp += hp;
        }
    }
}
