using UnityEngine;

public class GameController : MonoBehaviour
{
    // 행동 코스트를 사용해서 할 수 있는 행동
    enum CostAction
    {
        // 이동
        Move,
        // 상점 이용
        Shopping,
        // 직업 특수 능력사용
        ClassAbility,
    }
    // 행동 코스트
    private int ActionCost;
    // 스텟 코스트
    private int statusCost;
    // 게임이 진행될 턴 카운트
    private int turnCount;
    // 현재까지 진행된 턴
    private int currentTurn;
    // 현재 턴인 플레이어
    private bool currentPlayer;
    
    // 승리
    private void Victory()
    {

    }
    
    // 행동 코스트를 사용
    private void Action(CostAction currentPlayerAction, int Cost)
    {

    }
    // 스텟 코스트로 능력치를 조절
    private void StatusAdjustment(Player player, int Cost)
    {

    }
}
