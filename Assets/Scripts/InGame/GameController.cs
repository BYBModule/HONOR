using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

// 현재 턴인 플레이어 체크(PlayerCheck) > 주사위굴리기 > 주사위를 굴린 이후 30초 카운트시작(제한시간) >
// 행동 코스트 사용 (플레이어 이동(PlayerMove()), 직업 특수 능력 사용) > 턴 넘김 or 30초지남 > 이동위치에 플레이어 확인 > 
// 게임승리, 사망 확인 > 다음 턴 플레이어 확인 > 플레이어 전환 > 처음으로부터 반복
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
    private float elapsedTime = 0;
    private float diceDelay = 0;
    // 현재까지 진행된 턴
    private bool currentTurn = true;
    // 턴 종료 체크
    private bool isTurnEnd;
    // 턴 시작 체크
    private bool startTurn = false;
    

    
    // 1층 플로어
    public Transform startPosition;

    // 플레이어 리스트
    public List<Player> playerList;

    // 테스트용    
    public GameObject dummyPlayer;
    
    public List<Transform> playerStartField;

    CheckDiceNum dice;
    public FieldUtility fieldUtility;

    // 플레이어 목록 리스트
    private Dictionary<int, Player> players = new Dictionary<int, Player>();
    
    // 현재 Floor번호
    public int currentFloorCount = 0;
    public int currentPlayerCount = 0;
    
    public int preCost = 0;
    void Awake()
    {
        // turnEnd.onClick.AddListener(TurnEnd);
        for(int i = 0; i < playerList.Count; i++)
        {
            players.Add(i, playerList[i]);
        }
    }
    void Start()
    {
        dice = dicePrefab.GetComponentInChildren<CheckDiceNum>();
        fieldUtility.SetDefaultStartingPoint(playerList);
    }
    void Update()
    {   
        if(diceDelay == 0)
            StartTurn();
        if(startTurn && diceDelay >= 5.0f)
        {
            // 경과시간을 기록
            elapsedTime += Time.deltaTime;
            // 1초마다 턴 제한 시간을 1씩 증가
            if(elapsedTime > turnLimit)
            {
                turnLimit = (int)elapsedTime;
                UpdateTurnText();
                // Action(CostAction.Move, 5);
            }
            if(currentTurn)
            {
                // StartTurn();

            }
        }
    }
    // 턴 제한시간을 출력합니다.
    private void UpdateTurnText()
    {
    //    turnText.text = turnLimit.ToString();
    }
    // 행동 코스트를 사용
    private void Action(CostAction currentPlayerAction, int Cost)
    {
        // 시작 필드 체크
        switch(currentPlayerAction)
        {
            case CostAction.Move:
                if(Cost > 1)
                {
                    // fieldUtility.PlayerMove(player, 5);
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
        if(!startTurn)
        {
            Debug.Log("StartTurn");

            dice.DiceRolling();
            currentPlayer = players[currentPlayerCount];
            startTurn = true;
        }
        diceDelay += Time.deltaTime;
        if(diceDelay > 4)
        {
            actionCost = dice.GetDiceNumber();
        }
        //actionCost = int.Parse(dice.GetComponent<DiceRoll>().scoreText.text);
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
    private void TurnEnd(int currentPlayerCount)
    {
        if(currentPlayerCount < 3)
        {
            currentPlayerCount++;
        }
        else
        {
            currentPlayerCount = 0;
        }
    }

    // 턴 전환 플레이어 체크
    private void NextPlayerCheck(List<Player> players)
    {

    }

    // 플레이어 발판 이동 처리
    // private void PlayerMove(Transform parant, int cost)
    // {
        // int moveDistance;
        // if(preCost == 0)
            // preCost = cost;
        // moveDistance = currentFloorCount;
        // if(Input.GetKeyDown(KeyCode.RightArrow))
        // {
            // if(preCost > 0)
            // {
                // preCost -= 1;
                // moveDistance += 1;
            // }
        // }
        // else if(Input.GetKeyDown(KeyCode.LeftArrow))
        // {
            // if(preCost < cost)
            // {
                // preCost += 1;
                // moveDistance -= 1;
            // }
        // }
        // 
        // if(moveDistance < 1)
        // {
            // currentFloorCount = moveDistance + playerTransform.Count;
        // }
        // else if(moveDistance > playerTransform.Count)
        // {
            // currentFloorCount = moveDistance - playerTransform.Count;
        // }
        // else
        // {
            // currentFloorCount = moveDistance;
        // }
        // checkPoint.transform.parent = playerTransform[currentFloorCount];
        // checkPoint.transform.position = playerTransform[currentFloorCount].position;
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
            // dummyPlayer.transform.position = playerTransform[currentFloorCount].position;
            // dummyPlayer.transform.parent = playerTransform[currentFloorCount];
            // cost -= preCost;
            // return;
        // }    
    // }
    // 승리
    private void Victory()
    {

    }
}
