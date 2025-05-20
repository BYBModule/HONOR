using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    // 현재 플레이어가 위치한 필드(개인(일반, 보스), 일반, 보스)
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
        Warrior = 1,
        // 사제
        Priest = 2,
        // 성기사
        Paladin = 3,
        // 대부호
        Millionaire = 4,
        // 도적
        Thief = 5,
        // 궁수
        Archor = 6,
    }
    // 카드 게임 매니저
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
    // 플레이어 개인 필드 기본값
    public int playerDefaultStartingPoint;
    // 플레이어 현재 위치
    public int playerPosition = 0;
    public Floor currentFloor;
    // 플레이어 엘리트 킬
    public int eliteKillCount = 0;
    // 행동 코스트
    public int actionCost = 0;
    // 스텟 코스트
    public int statusCost = 0;
    // 현재 플레이어의 위치가 개인필드인지 확인
    public bool startPoint = true;
    public bool isInfo = true;
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
        Instantiate(FindAnyObjectByType<CreatePlayer>().currentClassPrefab, transform.position, quaternion.identity, transform);
        playerClass = FindAnyObjectByType<CreatePlayer>().className;
        // 플레이어 데이터 생성
        playerData = new PlayerData();
        SelectClass(playerClass);
    }
    private void SelectClass(ClassName playerClass)
    {
        switch (playerClass)
        {
            // 전사를 선택했다면
            case ClassName.Warrior:
                player_Class = new Warrior(this);
                break;
            // 궁수를 선택했다면
            case ClassName.Archor:
                player_Class = new Archor(this);
                break;
            // 도적을 선택했다면
            case ClassName.Thief:
                player_Class = new Thief(this);
                break;
            // 성기사를 선택했다면
            case ClassName.Paladin:
                player_Class = new Paladin(this);
                break;
            // 사제를 선택했다면
            case ClassName.Priest:
                player_Class = new Priest(this);
                break;
            // 대부호를 선택했다면
            case ClassName.Millionaire:
                player_Class = new Millionaire(this);
                break;
        }
        if (isInfo)
        {
            UpdateHp(0);
            Debug.Log($" 직업 : {playerClass}\n 공격력 : {playerData.attackDamage}\n 체력 : {playerData.playerHp} / {status.maxHp}\n 힘 : {status.strength}\n 지능 : {status.intelligence}\n 적중 : {status.hitRate}\n 회피 : {status.evasion}\n 행운 : {status.luck}");
            isInfo = false;
        }
    }
    void Update()
    {

    }
    public bool Info()
    {
        return !isInfo;
    }
    public void ChangeFloor(Floor floor)
    {
        this.currentFloor = floor;
    }
    public int Attack()
    {
        return -playerData.attackDamage;
    }
    public bool IsPlayerAlive()
    {
        return playerData.playerHp < 0 ? true : false;
    }
    // 수치를 업데이트 하기 위한 변수
    public void PlayerUpdate()
    {
        player_Class.Set_Status(this, status);
    }
    // 플레이어 사망처리
    public void PlayerDead()
    {
        if (IsPlayerAlive() == false)
        {
            if (playerData.playerGold != 0)
                playerData.playerGold -= (int)playerData.playerGold / 10;
            UpdateHp(0);
        }
    }
    // 체력 업데이트
    public void UpdateHp(int hp)
    {
        if (playerData.playerHp > 0)
        {
            playerData.playerHp += hp;
        }
        else
        {

            playerData.playerHp = status.maxHp;
        }
    }
    // 플레이어가 있는 위치를 체크하는 메서드
    public Floor CheckFloor()
    {
        if (currentFloor == Floor.Normal)
            return Floor.Normal;
        else if (currentFloor == Floor.Boss)
            return Floor.Boss;
        else if (currentFloor == Floor.PrivateNormal)
            return Floor.PrivateNormal;
        return Floor.PrivateBoss;
    }
    public void PlayerTransform(Transform transform, Vector3 position)
    {
        transform.parent = transform;
        transform.position = position;
    }
}
