using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    public enum Floor
    {
        Normal,
        Boss,
        PrivateNormal,
        PrivateBoss,
    }
    
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
    public CardGameManager cardGameManager;
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
    // 플레이어 진입 필드 기본값 (0 : 일반필드, 1 : 보스필드)
    public int[] playerDefaultStartingPoint = new int [2];
    // 플레이어 현재 위치
    public int playerPosition = 0;
    public Floor currentFloor;
    // 행동 코스트
    public int actionCost = 0;
    // 스텟 코스트
    public int statusCost = 0;
    public bool startPoint = true;
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
            // if(cardGameManager.gameObject.activeSelf == false)
            // {
            //     cardGameManager.gameObject.SetActive(true);
            // }
            //}
        //}
        
    }
    public void ChangeFloor(Floor floor)
    {
        this.currentFloor = floor;
    }
    // 수치를 업데이트 하기 위한 변수
    public void PlayerUpdate()
    {
        player_Class.Set_Status(this, status);
    }
    // 플레이어 사망처리
    public void PlayerDead()
    {
        if(playerData.playerGold != 0)
            playerData.playerGold -= (int)playerData.playerGold/10;
        UpdateHp(0);    
    }
    // 체력 업데이트
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

    // 플로어에서 필드로 전환될 때 기본위치치
    public void FloorToField(Floor floor)
    {
        if(floor == Floor.Normal)
        {
            playerPosition = playerDefaultStartingPoint[1];
            this.currentFloor = Floor.PrivateBoss;
        }
        else if(floor == Floor.Boss)
        {
            playerPosition = playerDefaultStartingPoint[0];
            this.currentFloor = Floor.PrivateNormal;
        }
    }
    // 일반, 보스필드 전환될 때 기본위치
    public void FieldToFloor(Floor floor)
    {
        if(floor == Floor.PrivateNormal)
        {
            this.currentFloor = Floor.Normal;
        }
        else if(floor == Floor.PrivateBoss)
        {
            this.currentFloor = Floor.Boss;
        }
        startPoint = true;       
    }
    // 플레이어가 있는 위치를 체크하는 메서드
    public Floor CheckFloor()
    {
        if(currentFloor == Floor.Normal)
            return Floor.Normal;
        else if(currentFloor == Floor.Boss)
            return Floor.Boss;
        else if(currentFloor == Floor.PrivateNormal)
            return Floor.PrivateNormal;        
        return Floor.PrivateBoss;
    }
}
