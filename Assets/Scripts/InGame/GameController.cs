using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public CinemachineCamera cinemachine;
    // 행동 코스트를 사용해서 할 수 있는 행동
    enum CostAction
    {
        // 이동
        Move,
        // 상점 이용
        Shopping,
        // 직업 특수 능력사용
        ActiveClassAbility,
        
        Default,
    }
    // 주사위 프리팹
    public GameObject dicePrefab;
    // 상점 프리팹
    public GameObject shopPrefab;
    // 턴 종료 버튼
    public Button turnEnd;
    // 현재 턴을 기록해주는 텍스트
    public TextMeshProUGUI turnText;
    
    // 현재 턴인 플레이어
    private Player currentPlayer;
    // 행동 코스트
    [SerializeField] private int actionCost = 0;
    // 스텟 코스트
    [SerializeField] private int statusCost = 0;
    // 게임이 진행된 턴 카운트
    private int turnCount;
    // 턴 제한 시간
    private int turnLimit;
    // 경과 시간
    private float elapsedTime;
    // 현재까지 진행된 턴
    private bool currentTurn = true;
    // 턴 종료 체크
    private bool isTurnEnd;
    // 턴 시작 체크
    private bool startTurn = true;
    
    // 이동할 체크포인트
    public GameObject moveCheckPoint;
    
    // 1층 플로어
    public Transform startPosition;
    
    // 플레이어 시작지점
    [SerializeField] private List<Transform> firstFloorPosition;
    // 플레이어 목록 리스트
    public List<Player> players;

    // 테스트용    
    public GameObject dummyPlayer;
    GameObject checkPoint;
    private Dictionary<int, Transform> playerTransform = new Dictionary<int, Transform>();
    public int currentCount = 0;
    public int preCost = 0;
    void Awake()
    {
        turnEnd.onClick.AddListener(TurnEnd);
        var firstFloor = startPosition.GetComponentInChildren<Transform>();
        foreach(Transform floor in firstFloor)
        {
            firstFloorPosition.Add(floor);
            playerTransform.Add(currentCount, floor);
            currentCount++;
        }
        currentCount = 1;
       
    }
    void Start()
    {
        dummyPlayer.transform.position = playerTransform[currentCount].position + Vector3.up;
        dummyPlayer.transform.parent = playerTransform[currentCount]; 
        //Instantiate(dummyPlayer, playerTransform[currentCount + 1].position + Vector3.up, Quaternion.identity, playerTransform[currentCount + 1]);    
        checkPoint = Instantiate(moveCheckPoint, dummyPlayer.transform.position, Quaternion.identity, dummyPlayer.transform);
    }
    void Update()
    {    
        if(startTurn)
        {
            // 경과시간을 기록
            elapsedTime += Time.deltaTime;
            // 1초마다 턴 제한 시간을 1씩 증가
            if(elapsedTime > turnLimit)
            {
                turnLimit = (int)elapsedTime;
                UpdateTurnText();
                Action(CostAction.Move, 5);
            }
            if(currentTurn)
            {
                // StartTurn();

            }
        }
    }

    // 승리
    private void Victory()
    {

    }
    // 턴 제한시간을 출력합니다.
    private void UpdateTurnText()
    {
    //    turnText.text = turnLimit.ToString();
    }
    // 행동 코스트를 사용
    private void Action(CostAction currentPlayerAction, int Cost)
    {
        switch(currentPlayerAction)
        {
            case CostAction.Move:
                if(Cost > 1)
                {
                    PlayerMove(dummyPlayer.transform, Cost);
                }
                else
                {
                    Debug.Log("Not enough cost");
                    return;
                }
                break;
            case CostAction.Shopping:
                if(Cost > 2)
                {
                    Cost -= 2;
                    Shopping();
                }
                else
                {
                    Debug.Log("Not enough cost");
                    return;
                }
                break;
            case CostAction.ActiveClassAbility:
                if(Cost > 2)
                {
                    Cost -= 2;
                    ActiveClassAbility();
                }
                else
                {
                    Debug.Log("Not enough cost");
                    return;
                }
                break;
            case CostAction.Default :
                break;
        }

    }
    void StartTurn()
    {
        GameObject dice = Instantiate(dicePrefab, transform.position, Quaternion.identity, currentPlayer.transform);
        actionCost = int.Parse(dice.GetComponent<DiceRoll>().scoreText.text);
        startTurn = true;
    }

    // 스텟 코스트로 능력치를 조절
    private void StatusAdjustment(Player player, int Cost)
    {
        
    }
    
    // 플레이어 상점 이용
    private void Shopping()
    {
        shopPrefab.SetActive(true);
    }

    // 직업 능력 사용
    private void ActiveClassAbility()
    {
        currentPlayer.player_Class.ClassAbility();
    }

    // 턴 종료 처리
    private void TurnEnd()
    {
        // 턴 제한시간이 끝나거나, 턴 종료 버튼을 눌렀을 때
        if(isTurnEnd)
        {
            turnCount += 1;
            actionCost = 0;
            statusCost = 0;
            turnLimit = 0;
            isTurnEnd = false;
            startTurn = false;
        }
        else
        {
        }
        isTurnEnd = true;
    }

    // 플레이어 발판 이동 처리
    private void PlayerMove(Transform parant, int cost)
    {
        int moveDistance;
        if(preCost == 0)
            preCost = cost;
        moveDistance = currentCount;
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
            currentCount = moveDistance + playerTransform.Count;
        }
        else if(moveDistance > playerTransform.Count)
        {
            currentCount = moveDistance - playerTransform.Count;
        }
        else
        {
            currentCount = moveDistance;
        }
        checkPoint.transform.parent = playerTransform[currentCount];
        checkPoint.transform.position = playerTransform[currentCount].position;
        if(Input.GetKeyDown(KeyCode.Space))
        {
            dummyPlayer.transform.position = playerTransform[currentCount].position;
            dummyPlayer.transform.parent = playerTransform[currentCount];
            cost -= preCost;
            return;
        }
        
    }
}
