using System.Collections;
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
    }
    // 주사위 프리팹
    public GameObject dicePrefab;
    // 상점 프리팹
    public GameObject shopPrefab;
    // 턴 종료 버튼
    public Button turnEnd;
    // 현재 턴을 기록해주는 텍스트
    public TextMeshProUGUI turnText;
    // 플레이어 목록 리스트
    public List<Player> players;
    // 현재 턴인 플레이어
    private Player currentPlayer;
    // 행동 코스트
    private int actionCost;
    // 스텟 코스트
    private int statusCost;
    // 게임이 진행된 턴 카운트
    private int turnCount;
    // 턴 제한 시간
    private int turnLimit;
    // 경과 시간
    private float elapsedTime;
    // 현재까지 진행된 턴
    private int currentTurn;
    // 턴 종료 체크
    private bool isTurnEnd;
    // 턴 시작 체크
    private bool startTurn;

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
            }
            // 턴 제한시간이 끝나거나, 턴 종료 버튼을 눌렀을 때
            if(turnLimit >= 30 || isTurnEnd == true)
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
        }
    }

    // 승리
    private void Victory()
    {

    }
    
    // 행동 코스트를 사용
    private void Action(CostAction currentPlayerAction, int Cost)
    {
        StartCoroutine(StartTurn());
        switch(currentPlayerAction)
        {
            case CostAction.Move:
                if(Cost > 1)
                {
                    PlayerMove();
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

                }
                break;
        }

    }
    IEnumerator StartTurn()
    {
        GameObject diceRoll = Instantiate(dicePrefab, transform.position, Quaternion.identity, currentPlayer.transform);

        Destroy(diceRoll);
        yield return null;
    }

    // 스텟 코스트로 능력치를 조절
    private void StatusAdjustment(Player player, int Cost)
    {

    }
    
    // 플레이어 상점 이용
    private void Shopping()
    {

    }

    // 직업 능력 사용
    private void ActiveClassAbility()
    {
        currentPlayer.player_Class.ClassAbility();
    }

    // 플레이어 이동 처리
    private void PlayerMove()
    {

    }
}
