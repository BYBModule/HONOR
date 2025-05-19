using System.Collections.Generic;
using UnityEngine;

public class FieldUtility : MonoBehaviour
{    
    [HideInInspector] public GameObject checkPoint;
    // 이동할 체크포인트
    public GameObject checkPointPrefab;
    // 필드 카메라 전환 시점
    public GameObject fieldCameraLook;
    // 일반 필드 기준
    public Transform normalStartingPoint;
    // 보스 필드 기준
    public Transform bossStartingPoint;

    // 플레이어 개인필드 (일반)
    public List<Transform> privateDefaultNormalField;
    // 플레이어 개인필드 (보스)
    public List<Transform> privateDefaultBossField;

    // 일반 필드 시작 지점
    public List<Transform> defaultNormalStart;
    // 보스 필드 시작 지점
    public List<Transform> defaultBossStart;
    // 개인(일반) 필드 시작 지점
    public List<Transform> defaultPrivateNormalPoint;
    // 개인(보스) 필드 시작 지점
    public List<Transform> defaultPrivateBossPoint;

    // 일반 필드 (1층)
    [HideInInspector] public Dictionary<int, Transform> normalField = new Dictionary<int, Transform>();
    // 보스 필드 (2층)
    [HideInInspector] public Dictionary<int, Transform> bossField = new Dictionary<int, Transform>();
    // 개인 필드 (일반 1층)
    [HideInInspector] public Dictionary<int, Transform> privateNormalField = new Dictionary<int, Transform>();
    // 개인 필드 (보스 2층)
    [HideInInspector] public Dictionary<int, Transform> privateBossField = new Dictionary<int, Transform>();

    public int normalFieldCount = 0;
    public int bossFieldCount = 0;
    public int privateNormalFieldCount = 0;
    public int privateBossFieldCount = 0;
    // 특수필드
    public List<Transform> specialField;

    void Awake()
    {
        InitializedField(normalStartingPoint, normalFieldCount, normalField);
        InitializedField(bossStartingPoint, bossFieldCount, bossField);
        for (int i = 0; i < privateDefaultNormalField.Count; i++)
        {
            InitializedField(privateDefaultNormalField[i], privateNormalFieldCount, privateNormalField);
            privateNormalFieldCount = privateNormalField.Count;
        }
        for (int i = 0; i < privateDefaultBossField.Count; i++)
        {
            InitializedField(privateDefaultBossField[i], privateBossFieldCount, privateBossField);
            privateBossFieldCount = privateBossField.Count;
        }
        
    }
    // 필드를 초기화 하기위한 메서드
    void InitializedField(Transform field, int count, Dictionary<int, Transform> getField)
    {
        var input = field.GetComponentInChildren<Transform>();
        foreach (Transform inputField in input)
        {
            getField.Add(count, inputField);
            count++;
        }
    }
    // 플레이어의 기본위치를 할당합니다.
    public void SetDefaultStartingPoint(List<Player> players)
    {
        int normal = 0;
        int boss = 0;
        int privateNormal = 0;
        int privateBoss = 0;

        for (int i = 0; i < 4; i++)
        {
            // 개인필드(일반) 할당
            players[i].transform.parent = privateNormalField[privateNormal];
            players[i].transform.position = privateNormalField[privateNormal].position;
            // 플레이어 일반, 보스필드 할당
            defaultNormalStart.Add(normalField[normal]);
            defaultBossStart.Add(bossField[boss]);
            // 플레이어 개인필드 시작지점 할당
            defaultPrivateNormalPoint.Add(privateNormalField[privateNormal]);
            defaultPrivateBossPoint.Add(privateBossField[privateBoss]);
            // 플레이어의 일반, 보스필드 시작지점 할당
            players[i].playerDefaultStartingPoint = privateNormal;
            // 게임이 시작될 때 플레이어의 시작지점을 할당
            players[i].playerPosition = privateNormal;
            normal += 6;
            boss += 4;
            privateNormal += 3;
            privateBoss += 3;
            // 게임이 시작될 때 플레이어의 Floor 값을 할당
            players[i].ChangeFloor(Player.Floor.PrivateNormal);
        }
    }
    // 현재 플레이어가 있는 필드를 확인, 해당 필드를 리턴
    public Dictionary<int, Transform> CurrentFloor(Player player)
    {
        if (player.CheckFloor() == Player.Floor.Normal)
        {
            return normalField;
        }
        else if (player.CheckFloor() == Player.Floor.Boss)
        {
            return bossField;
        }
        else if (player.CheckFloor() == Player.Floor.PrivateNormal)
        {
            return privateNormalField;
        }
        return privateBossField;
    }
    // 플레이어의 위치를 되돌려주는 메서드
    public Transform GetPlayerTransform(Player player)
    {
        return CurrentFloor(player)[player.playerPosition];
    }
    // 플레이어의 시작지점을 정해주는 메서드
    public void PlayerStart(Player player)
    {    
        // 일반 필드
        if (player.CheckFloor() == Player.Floor.Normal)
        {
            player.PlayerTransform(normalField[player.playerPosition], normalField[player.playerPosition].position);
        }
        // 보스 필드
        else if (player.CheckFloor() == Player.Floor.Boss)
        {
            player.PlayerTransform(bossField[player.playerPosition], bossField[player.playerPosition].position);
        }
        // 개인 필드
        else if (player.CheckFloor() == Player.Floor.PrivateNormal)
        {
            player.PlayerTransform(privateNormalField[player.playerPosition], privateNormalField[player.playerPosition].position);
        }
        else if (player.CheckFloor() == Player.Floor.PrivateBoss)
        {
            player.PlayerTransform(privateBossField[player.playerPosition], privateBossField[player.playerPosition].position);
        }
    }


    // 현재 필드가 일반 필드인지 보스 필드인지 확인하고 해당 필드의 카메라 위치를 반환
    public Transform FieldCameraLookAt(Player player)
    {
        Transform result;
        if (player.CheckFloor() == Player.Floor.Normal || player.CheckFloor() == Player.Floor.PrivateNormal)
        {
            result = fieldCameraLook.GetComponent<Transform>().GetChild(0);
            return result;
        }
        else if (player.CheckFloor() == Player.Floor.Boss || player.CheckFloor() == Player.Floor.PrivateBoss)
        {
            result = fieldCameraLook.GetComponent<Transform>().GetChild(1);
            return result;
        }
        return null;
    }
    // 플레이어의 이동을 처리하는 메서드
    public void PlayerMove(Player player, int cost, int preCost)
    {
        Debug.Log(preCost);
        // 이동커리
        int moveDistance;
        // 이동할 거리를 체크하기위한 체크포인트 생성
        if (checkPoint == null)
        {
            checkPoint = Instantiate(checkPointPrefab, player.transform.position, Quaternion.identity, player.transform.parent);
        }
        // 이동 거리에 플레이어의 현재 위치를 저장
        moveDistance = player.playerPosition;
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            // 남은 코스트가 0이 아닐때(이동)
            if (preCost > 0)
            {
                preCost -= 1;
                moveDistance += 1;
            }
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // 남은 코스트가 코스트보다 작을때(되돌리기)
            if (preCost < cost)
            {
                preCost += 1;
                moveDistance -= 1;
            }
        }
        if(player.CheckFloor() == Player.Floor.Normal || player.CheckFloor() == Player.Floor.Boss)
        {
            // 플레이어의 현재위치에서 이동한 거리가 1보다 작다면(필드에 할당된 Dictionary의 최소범위)
            if (moveDistance < 1)
            {
                player.playerPosition = moveDistance + CurrentFloor(player).Count - 1;
            }
            // 플레이어의 현재위치에서 이동한 거리가 필드의 할당된 Dictionary의 최대범위 보다 크다면
            else if (moveDistance > CurrentFloor(player).Count)
            {
                player.playerPosition = moveDistance - CurrentFloor(player).Count - 1;
            }

            // 체크포인트를 플레이어가 이동할 위치로 이동합니다
            checkPoint.transform.parent = GetPlayerTransform(player);
            checkPoint.transform.position = GetPlayerTransform(player).position;
        }
        else
        {
            // 개인 필드는 총 3칸
            if (player.CheckFloor() == Player.Floor.PrivateNormal)
            {
                if (moveDistance < player.playerDefaultStartingPoint)
                {
                    player.playerPosition = moveDistance + player.playerDefaultStartingPoint;
                }
                else if (moveDistance > player.playerDefaultStartingPoint + 2)
                {
                    player.playerPosition = moveDistance - player.playerDefaultStartingPoint + 2;
                }
            }
            else if (player.CheckFloor() == Player.Floor.PrivateBoss)
            {
                if (player.playerPosition + moveDistance < player.playerDefaultStartingPoint)
                {
                    player.playerPosition = player.playerDefaultStartingPoint + 2;
                    moveDistance = 0;
                }
                else if (player.playerPosition + moveDistance > player.playerDefaultStartingPoint + 2)
                {
                    player.playerPosition = player.playerDefaultStartingPoint;
                    moveDistance = 0;
                }

            }
            
            checkPoint.transform.parent = GetPlayerTransform(player);
            checkPoint.transform.position = GetPlayerTransform(player).position;
        }

        // 플레이어 이동처리
        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.PlayerTransform(GetPlayerTransform(player), GetPlayerTransform(player).position);
            cost -= preCost;
            if(preCost == 0)
                return;
        }   
    }    

}
