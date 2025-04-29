
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class BattlePhase : MonoBehaviour
{
    // 카드 게임 매니저저
    public CardGameManager cardGameManager;
    // 1번 플레이어가 선택한 카드
    private Card upPlayerSelect = null;
    // 2번 플레이어가 선택한 카드
    private Card downPlayerSelect = null;
    public TextMeshPro turnCountText;
    public Transform Player1CardDropPoint;
    public Transform player2CardDropPoint;
    
    public Button button;
    // 턴 실행을 위한 변수
    public int selectCount = 0;
    // 턴 카운트
    public int turnCount = 0;
    public float timeCount = 0;
    void Awake()
    {
        // 턴 종료 버튼 생성
        button.onClick.AddListener(TurnEndButton);
    }
    void Start()
    {
        cardGameManager.StartCardGame();
    }
    private void TurnEndButton()
    {
        TurnChange(cardGameManager.players[0], cardGameManager.players[1]);
    }
    void Update()
    {
        if(cardGameManager.players[0].hp <=0 || cardGameManager.players[1].hp <= 0)
        {
            Debug.Log("GameOver");
            selectCount = 0;
            turnCount = 0;
            timeCount = 0;
            gameObject.SetActive(false);
        }
        else
        {
            timeCount += Time.deltaTime;
            if(turnCount < 5)
            {
                
            }
            else
            {
                CardSelect(cardGameManager.players[0], cardGameManager.players[1]);
                
                turnCount = 0;
                timeCount = 0;
            }

            if(timeCount > turnCount)
            {
                TextUpdate(timeCount);
            }
        }
    }
    void TextUpdate(float deltaTime)
    {
        if(deltaTime > turnCount)
        {
            turnCount = (int)deltaTime;
            turnCountText.text = $"Turn Count : {20 - turnCount}";
        }
    }   
    // 전투
    public void Battle()
    {

    }
    public void TurnChange(CardGamePlayer upPlayer, CardGamePlayer downPlayer)
    {
        upPlayer.isPlayerturn = !upPlayer.isPlayerturn;
        downPlayer.isPlayerturn = !downPlayer.isPlayerturn;
        selectCount++;
    }

    // 카드 선택 메서드
    public void CardSelect(CardGamePlayer upPlayer, CardGamePlayer downPlayer)
    {
        Debug.Log("Card Select");
        if(selectCount < 2)
        {
            // 매 턴 두 플레이어가 카드를 선택
            if(upPlayer.isPlayerturn == true)
            {
                if(upPlayer.cards.Count > 0)
                {
                    // 1번 플레이어가가 선택한 카드
                    upPlayerSelect = SelectedCard(upPlayer.cards);
                }
                // 카드가 없다면 
                else
                {
                    // 선택된 카드를 null로 만듬듬
                    upPlayerSelect = null;
                }
                TurnChange(cardGameManager.players[0], cardGameManager.players[1]);
                Debug.Log("Up Player Select" + upPlayerSelect.Card_Effect);
            }
            else// if(downPlayer.isPlayerturn == true)
            {
                if(downPlayer.cards.Count > 0)
                {

                    // 2번 플레이어가 선택한 카드
                    downPlayerSelect = SelectedCard(downPlayer.cards);
                }
                // 남은 카드가 없다면
                else
                {
                    // 선택된 카드를 null로 만듬
                    downPlayerSelect = null;
                }
                TurnChange(cardGameManager.players[0], cardGameManager.players[1]);
                Debug.Log("Down Player Select" + downPlayerSelect.Card_Effect);
                // 턴을 넘김
            }
        }
        else if(selectCount >= 2)
        {
            cardGameManager.CardGameLogic(upPlayerSelect, downPlayerSelect);
            Debug.Log($"Player 1 : {cardGameManager.players[0].hp}, Player 2 : {cardGameManager.players[1].hp}");
            Debug.Log($"Player 1 : {cardGameManager.players[0].cards.Count}, Player 2 : {cardGameManager.players[1].cards.Count}");
            selectCount = 0;
        }
    }

    // 선택된 카드를 반환하는 메서드
    private Card SelectedCard(List<Card> cards)
    {
        Debug.Log("Selected Card");
        // while(turnCount > 30)
        // {
        //     button.gameObject.SetActive(true);
        //     LastUpdate(timeCount);    
        // }
        // 미구현
        // UI를 씬창에서 먼저 구현해야함

        // 플레이어가 카드를 선택하는 UI를 보여줌

        // 플레이어가 카드를 선택했다면 턴을 넘김
        // (테스트) 현재는 0번 인덱스 카드만 넘김
        return cards[0];
    }   

}
