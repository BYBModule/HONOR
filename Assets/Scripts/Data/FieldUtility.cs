using System.Collections.Generic;
using UnityEngine;

public class FieldUtility : MonoBehaviour
{
    // 이동할 체크포인트
    public GameObject checkPointPrefab;
    //
    public GameObject fieldCameraLook;  
    // 일반 필드 기준
    public Transform normalStartingPoint;   
    // 보스 필드 기준
    public Transform bossStartingPoint;
    
    // 플레이어 개인필드 (일반)
    public List<Transform> privateNormalField;
    // 플레이어 개인필드 (보스)
    public List<Transform> privateBossField;
    
    // 일반 필드 시작 지점
    public List<Transform> defaultNormalStart;
    // 보스 필드 시작 지점
    public List<Transform> defaultBossStart;
    
    // 일반 필드 (1층)
    [HideInInspector] public Dictionary<int, Transform> normalField = new Dictionary<int, Transform>();
    // 보스 필드 (2층)
    [HideInInspector] public Dictionary<int, Transform> bossField = new Dictionary<int, Transform>();

    public int normalFieldCount = 0;
    public int bossFieldCount = 0;

    // 현재 코스트
    private int preCost = 0;
    
    GameObject checkPoint;
    // 특수필드
    public List<Transform> specialField;

    void Awake()
    {
        var normal = normalStartingPoint.GetComponentInChildren<Transform>();
        foreach(Transform normalStart in normal)
        {
            normalField.Add(normalFieldCount, normalStart);
            normalFieldCount++;
        }
        var boss = bossStartingPoint.GetComponentInChildren<Transform>();
        foreach(Transform bossStart in boss)
        {
            bossField.Add(bossFieldCount, bossStart);
            bossFieldCount++;
        }
    }
    // 플레이어의 기본위치를 할당합니다.
    public void SetDefaultStartingPoint(List<Player> players)
    {
        int normal = 0;
        int boss = 0;
        List<Transform> defaultPosition = new List<Transform>();
        foreach(Transform playerSpawnPoint in privateNormalField)
        {
            defaultPosition.Add(playerSpawnPoint);
        }
        for(int i = 0; i < 4; i++)
        {
            players[i].transform.parent = defaultPosition[i];
            players[i].transform.position = defaultPosition[i].position;
            defaultNormalStart.Add(normalField[i]);
            defaultBossStart.Add(bossField[i]);
            players[i].playerDefaultStartingPoint[0] = normal;
            players[i].playerDefaultStartingPoint[1] = boss;
            normal += 6;
            boss += 4;
        }
    }
    // 현재 플레이어가 있는 필드를 확인, 해당 필드를 리턴
    public Dictionary<int, Transform> CurrentFloor(Player player)
    {
        if(player.CheckFloor() == Player.Floor.Normal)
        {
            return normalField;
        }
        else if(player.CheckFloor() == Player.Floor.Boss)
        {
            return bossField;
        }
        return null;
    }
    // 플레이어의 위치를 되돌려주는 메서드
    private Transform GetPlayerTransform(Player player)
    {
        return CurrentFloor(player)[player.playerPosition];
    }
    public void PlayerStart(Player player)
    {
        player.transform.parent = normalField[player.playerPosition];
        player.transform.position = normalField[player.playerPosition].position;
    }
    // 플레이어의 이동을 처리하는 메서드
    public void PlayerMove(Player player, int cost)
    {
        if(checkPoint == null)
        {
            checkPoint = Instantiate(checkPointPrefab, player.transform.position, Quaternion.identity, player.transform);
        }
        int moveDistance;
        if(preCost == 0)
            preCost = cost;
        moveDistance = player.playerPosition;
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(preCost > 0)
            {
                preCost -= 1;
                moveDistance += 1;
            }
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(preCost < cost)
            {
                preCost += 1;
                moveDistance -= 1;
            }
        }
        
        if(moveDistance < 1)
        {
            player.playerPosition = moveDistance + CurrentFloor(player).Count;
        }
        else if(moveDistance > CurrentFloor(player).Count)
        {
            player.playerPosition = moveDistance - CurrentFloor(player).Count;
        }
        else
        {
            player.playerPosition = moveDistance;
        }
        checkPoint.transform.parent = GetPlayerTransform(player);
        checkPoint.transform.position = GetPlayerTransform(player).position;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.transform.parent = GetPlayerTransform(player);            
            player.transform.position = GetPlayerTransform(player).position;
            cost -= preCost;
            return;
        }   
    }
    public Transform FieldCameraLookAt(Player player)
    {
        Transform result;
        if(player.CheckFloor() == Player.Floor.Normal)
        {
            result = fieldCameraLook.GetComponent<Transform>().GetChild(0);
            return result;
        }
        else if(player.CheckFloor() == Player.Floor.Boss)
        {
            result = fieldCameraLook.GetComponent<Transform>().GetChild(1);
            return result;
        }
        return null;
    }

}
