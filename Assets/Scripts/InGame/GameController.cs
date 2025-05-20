using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

// 현재 턴인 플레이어 체크(PlayerCheck) > 주사위굴리기 > 주사위를 굴린 이후 30초 카운트시작(제한시간) >
// 행동 코스트 사용 (플레이어 이동(PlayerMove()), 직업 특수 능력 사용) > 이동위치에 플레이어 및 몬스터 확인 > 전투 돌입 시 카운트 정지 > (전투를 했을시) 플레이어 정보를 저장
// 턴 넘김 or 30초지남 > 변환된 플레이어 정보를 각 플레이어에게 전달한 후 플레이어 전환
// 게임승리, 사망 확인 > 다음 턴 플레이어 확인 > 플레이어 전환 > 처음으로부터 반복
public class GameController : MonoBehaviour
{
    // 인게임 컨트롤러 인스턴스
    public static GameController Instance
    {
        get;
        private set;
    }
    public void ResetPlayData()
    {
        Instance = null;
    }
    // 주사위 클래스
    Dice dice;
    Monster boss;
    // 필드와 관련된 처리를 하기위한 클래스 
    public FieldUtility fieldUtility;
    // 몬스터 전투를 위한 클래스
    public MonsterBattle monsterBattle;

    // 행동 코스트를 사용해서 할 수 있는 행동
    enum CostAction
    {
        // 이동
        Move,
        // 상점 이용
        Shopping,
        // 직업 특수 능력사용
        ActiveClassAbility,

        UpFloor,
    }

    // 카메라
    public CinemachineCamera cinemachine;
    // 플레이어 UI
    public GameObject playerUI;
    // 주사위 프리팹
    public GameObject dicePrefab;
    // 상점 프리팹
    public GameObject shopPrefab;
    // 체크포인트 프리팹    
    public GameObject checkPointPrefab;
    // 턴 종료 버튼
    public Button turnEnd;
    // 현재 턴을 기록해주는 텍스트
    public TextMeshProUGUI turnText;
    //
    public TextMeshProUGUI currentTurnCount;
    // 현재 턴인 플레이어
    public Player currentPlayer;

    // 게임이 진행된 턴 카운트
    private int turnCount = 1;
    // 턴 제한 시간
    private int turnLimit;
    // 경과 시간
    private float elapsedTime = 0;

    // 현재까지 진행된 턴
    private bool currentTurn = true;
    // 턴 종료 체크
    private bool isTurnEnd = false;
    // 턴 시작 체크
    private bool startTurn = false;
    // 주사위 굴리기
    private bool diceRoll = true;
    // 턴 카메라 체크
    private bool isFieldCamera = false;

    // 1층 플로어
    public Transform startPosition;

    // 플레이어 리스트
    public List<Player> playerList;

    // 테스트용    
    public GameObject dummyPlayer;

    public List<Transform> playerStartField;


    // 플레이어 목록 리스트
    private Dictionary<int, Player> players = new Dictionary<int, Player>();

    // 현재 Floor번호
    public int currentFloorCount = 0;
    public int currentPlayerCount = 0;
    public int preCost = 0;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        turnEnd.onClick.AddListener(ClickEndButton);
        for (int i = 0; i < playerList.Count; i++)
        {
            players.Add(i, playerList[i]);
        }
    }
    void Start()
    {
        dice = dicePrefab.GetComponentInChildren<Dice>();
        fieldUtility.SetDefaultStartingPoint(playerList);
    }
    void Update()
    {
        if (turnCount % 3 == 0)
        {

        }
        if (diceRoll)
            StartTurn();
        if (startTurn)
        {
            // 경과시간을 기록
            elapsedTime += Time.deltaTime;
            // 1초마다 턴 제한 시간을 1씩 증가
            if (elapsedTime > turnLimit)
            {
                // 경과시간이 1초 지날때 마다 리미트에 1초를 더해 30 - Limit로 제한시간 UI에 출력
                turnLimit = (int)elapsedTime;
                UpdateTurnText();
                // 액션의 기본값은 Move로 처리
                Action(CostAction.Move, currentPlayer.actionCost);
                LookField(currentPlayer);
            }
            if (turnLimit >= 100 || isTurnEnd)
            {

                PlayerTurnEnd();
            }
        }
    }
    // 턴 제한시간을 출력합니다.
    private void UpdateTurnText()
    {
        turnText.text = (30 - turnLimit).ToString();
        currentTurnCount.text = "Turn : " + turnCount.ToString();
    }
    // 행동 코스트를 사용
    private void Action(CostAction currentPlayerAction, int cost)
    {
        // 시작 필드 체크
        switch (currentPlayerAction)
        {
            case CostAction.Move:
                if (cost > 0)
                {
                    fieldUtility.PlayerMove(currentPlayer, cost, this.preCost);
                }
                else
                {
                    Debug.Log("Not enough cost");
                    return;
                }
                break;
            case CostAction.Shopping:
                if (cost > 2)
                {
                    cost -= 2;
                    Shopping();
                }
                else
                {
                    Debug.Log("Not enough cost");
                    return;
                }
                break;
            case CostAction.ActiveClassAbility:
                if (cost > 2)
                {
                    cost -= 2;
                    ActiveClassAbility();
                }
                else
                {
                    Debug.Log("Not enough cost");
                    return;
                }
                break;
            case CostAction.UpFloor:
                UpFloor(currentPlayer);
                break;
        }
    }
    void ClickEndButton()
    {
        isTurnEnd = true;
    }
    IEnumerator TurnStart()
    {
        if (!startTurn)
        {
            // 주사위를 굴리고 눈금값을 플레이어 행동코스트에 저장
            diceRoll = false;
            dice.DiceRolling();
            yield return new WaitForSeconds(10.0f);
            currentPlayer = players[currentPlayerCount];
            currentPlayer.actionCost = dice.GetDiceNumber();
            // 다이스 UI > 플레이어 UI 전환
            ChangeUI(false);
            // 플레이어에게 카메라 위치 전환
            this.preCost = currentPlayer.actionCost;
            cinemachine.Lens.FieldOfView = 30;
            cinemachine.Target.TrackingTarget = currentPlayer.transform;
            monsterBattle.SpawnMonster(fieldUtility.normalField);
            startTurn = true;
        }
        yield return new WaitForSeconds(1.0f);
    }
    void StartTurn()
    {
        StartCoroutine(TurnStart());
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
    private void UpFloor(Player player)
    {
        if (player.CheckFloor() == Player.Floor.PrivateNormal)
        {
            player.ChangeFloor(Player.Floor.Normal);
        }
        else if (player.CheckFloor() == Player.Floor.Normal)
        {
            player.ChangeFloor(Player.Floor.PrivateBoss);
            player.playerPosition = player.playerDefaultStartingPoint;
        }
        else if (player.CheckFloor() == Player.Floor.PrivateBoss)
        {
            player.ChangeFloor(Player.Floor.Boss);
        }
        else
        {
            player.ChangeFloor(Player.Floor.PrivateNormal);
            player.playerPosition = player.playerDefaultStartingPoint;
        }
            isTurnEnd = true;
    }
    // 턴 종료 시 실행되는 메서드
    public void PlayerTurnEnd()
    {
        if (IsVictory())
        {
            Victory();
        }
        // 주사위 굴리기위한 상태값을 true로 전환
        diceRoll = true;
        // 현재 플레이어의 코스트를 0으로 전환
        currentPlayer.actionCost = 0;
        // 턴 시작을 체크하기 위한 상태값을 false로 전환
        startTurn = false;
        // 다음 플레이어의 턴종료 값을 false로 전환
        isTurnEnd = false;
        // 경과 시간 초기화
        elapsedTime = 0;
        // 턴 제한 시간 초기화 
        turnLimit = 0;
        // 현재 코스트 초기화
        preCost = 0;
        // 주사위를 바라보기위한 카메라값
        cinemachine.Lens.FieldOfView = 120;
        cinemachine.Target.TrackingTarget = dicePrefab.transform.parent;
        // 현재 턴인 플레이어를 다음 턴의 플레이어로 전환
        ChangePlayer(currentPlayerCount);
        // 플레이어 UI를 주사위 UI로 전환환
        ChangeUI(true);
    }

    // 턴 종료 처리
    private void ChangePlayer(int currentPlayerCount)
    {
        // 총 4명의 플레이어가 진행
        // 플레이어의 최대 카운트 수는 0~3이므로 
        // 3이 넘어가는 시점에서 다음 턴인 플레이어는 0번 플레이어
        if (currentPlayerCount < 3)
        {
            this.currentPlayerCount++;
        }
        else
        {
            this.currentPlayerCount = 0;
            // 모든 플레이어가 행동을 끝냈기 때문에 턴 카운트를 1 증가 시킴
            turnCount++;
            monsterBattle.UpdateMonsterState(turnCount);
        }
    }
    // 스텟 코스트로 능력치를 조절, 현재 플레이어의 정보를 출력
    private void StatusAdjustment(Player player, int Cost)
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            // 캐릭터 UI 출력
            // 버튼을 클릭하여 스테이터스 조정


        }
    }
    // 승리
    private void Victory()
    {
        // 승리 후 UI출력, 이펙트
        // 종료 후 씬 전환
    }
    // 승리 체크
    private bool IsVictory()
    {
        // currentPlayer 승리
        if (currentPlayer.eliteKillCount >= 30 || !boss.isActiveAndEnabled)
        {
            return true;
        }
        return false;
    }
    // 화면 전환
    private void LookField(Player player)
    {
        // 텝키로 현재 필드의 전체 맵 또는 플레이어 위치를 바라봅니다.
        if (isFieldCamera)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Transform cameraTarget = fieldUtility.FieldCameraLookAt(player);
                if (cameraTarget != null)
                {
                    cinemachine.Lens.FieldOfView = 40;
                    cinemachine.Target.TrackingTarget = cameraTarget;
                    isFieldCamera = false;
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                cinemachine.Lens.FieldOfView = 30;
                cinemachine.Target.TrackingTarget = fieldUtility.checkPoint.transform;
                isFieldCamera = true;
            }
        }
    }
    // 플레이어UI와 주사위UI 전환
    private void ChangeUI(bool toggle)
    {
        dicePrefab.SetActive(toggle);
        playerUI.SetActive(!toggle);
    }
}
