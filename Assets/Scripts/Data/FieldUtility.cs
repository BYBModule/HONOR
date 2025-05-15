using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Mono.Cecil.Cil;
using Unity.VisualScripting.FullSerializer;
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
    public List<Transform> privateDefaultNormalField;
    // 플레이어 개인필드 (보스)
    public List<Transform> privateDefaultBossField;
    
    // 일반 필드 시작 지점
    public List<Transform> defaultNormalStart;
    // 보스 필드 시작 지점
    public List<Transform> defaultBossStart;
    public List<Transform> defaultPrivateNormalPoint;
    public List<Transform> defaultPrivateBossPoint;
    
    // 일반 필드 (1층)
    [HideInInspector] public Dictionary<int, Transform> normalField = new Dictionary<int, Transform>();
    // 보스 필드 (2층)
    [HideInInspector] public Dictionary<int, Transform> bossField = new Dictionary<int, Transform>();
    // 개인필드
    [HideInInspector] public Dictionary<int, Transform> privateField = new Dictionary<int, Transform>();
    public int normalFieldCount = 0;
    public int bossFieldCount = 0;
    public int privateFieldCount = 0;
    // 특수필드
    public List<Transform> specialField;

    void Awake()
    {
        InitializedField(normalStartingPoint, normalFieldCount, normalField);
        InitializedField(bossStartingPoint, bossFieldCount, bossField);
        var normal = privateDefaultNormalField;
        foreach(Transform input in normal)
        {
            InitializedField(input, privateFieldCount, privateField);
            privateFieldCount += 3;
        }
        var boss = privateDefaultBossField;
        foreach(Transform input in boss)
        {
            InitializedField(input, privateFieldCount, privateField);
            privateFieldCount += 3;
        }
    }
    void InitializedField(Transform field, int count, Dictionary<int, Transform> getField)
    {
        var input = field.GetComponentInChildren<Transform>();
        foreach(Transform inputField in input)
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
        // 
        int privateNormalField = 0;
        // 개인필드에 일반, 보스필드를 통합해서 초기화 했기때문에 보스개인필드는 중앙값부터 시작 
        int privateBossField = privateField.Count % 2 == 0 ? privateField.Count/2 : privateField.Count/2 + 1; 

        List<Transform> defaultPosition = new List<Transform>();
        foreach(Transform playerSpawnPoint in privateDefaultNormalField)
        {
            defaultPosition.Add(playerSpawnPoint);
        }
        for(int i = 0; i < 4; i++)
        {
            players[i].transform.parent = privateField[privateNormalField];
            players[i].transform.position = privateField[privateNormalField].position;
            defaultNormalStart.Add(normalField[normal]);
            defaultBossStart.Add(bossField[boss]);
            defaultPrivateNormalPoint.Add(privateField[privateNormalField]);
            defaultPrivateBossPoint.Add(privateField[privateBossField]);
            players[i].playerDefaultStartingPoint[0] = privateNormalField;
            players[i].playerDefaultStartingPoint[1] = privateBossField;
            players[i].playerPosition = privateNormalField;
            normal += 6;
            boss += 4;
            privateNormalField += 3;
            privateBossField += 3;
            players[i].ChangeFloor(Player.Floor.PrivateNormal);
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
        return privateField;
    }
    // 플레이어의 위치를 되돌려주는 메서드
    public Transform GetPlayerTransform(Player player)
    {
        return CurrentFloor(player)[player.playerPosition];
    }
    public void PlayerStart(Player player)
    {
        if(player.CheckFloor() == Player.Floor.Normal)
        {
            player.transform.parent = normalField[player.playerPosition];
            player.transform.position = normalField[player.playerPosition].position;
        }
        else if(player.CheckFloor() == Player.Floor.Boss)
        {
            player.transform.parent = bossField[player.playerPosition];
            player.transform.position = bossField[player.playerPosition].position;
        }
        else if(player.CheckFloor() == Player.Floor.PrivateNormal || player.CheckFloor() == Player.Floor.PrivateBoss)
        {
            player.transform.parent = privateField[player.playerPosition];
            player.transform.position = privateField[player.playerPosition].position;
        }
    }
    
    public Transform FieldCameraLookAt(Player player)
    {
        Transform result;
        if(player.CheckFloor() == Player.Floor.Normal || player.CheckFloor() == Player.Floor.PrivateNormal)
        {
            result = fieldCameraLook.GetComponent<Transform>().GetChild(0);
            return result;
        }
        else if(player.CheckFloor() == Player.Floor.Boss || player.CheckFloor() == Player.Floor.PrivateBoss)
        {
            result = fieldCameraLook.GetComponent<Transform>().GetChild(1);
            return result;
        }
        return null;
    }

}
