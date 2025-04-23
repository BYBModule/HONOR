using System.Collections.Generic;
using UnityEngine;

public class BattlePhase : MonoBehaviour
{
    // 카드 게임 플레이어
    class CardGamePlayer
    {
        // 체력
        public int hp;
        
        // 공격력
        public int attackDamage;
        
        // 현재 가지고 있는 카드
        public List<Card> cards;

        // 현재 플레이어 턴
        bool isPlayerturn = false;
        
        // CardGamePlayer 생성자
        public CardGamePlayer(int hp, int attackDamage)
        {
            this.hp = hp;
            this.attackDamage = attackDamage;
            isPlayerturn = !isPlayerturn;
        }
    }

    // 주사위 프리팹
    GameObject dicePrefab;
    // 카드 덱 리스트
    public List<Card> deck = new List<Card>(20);
    // 플레이어 리스트
    List<Player> players;
    // 카드 게임을 진행할 플레이어 리스트
    List<CardGamePlayer> playerList;

    

    void Awake()
    {
        // 플레이어를 받아 플레이어 리스트를 할당
    }
    void Start()
    {
        // 초기화
        Initialize();
    }
    void Update()
    {
        // 전투
        Battle();
    }
    
    private void Initialize()
    {
       // players를 순회하며 player를 찾아낼 때
        foreach(Player player in players)
        {
            // 플레이어 데이터 리스트를 하나씩 추가함
            playerList.Add(new CardGamePlayer(player.Player_Data.playerHp, player.Player_Data.attackDamage));
        }
        // 카드에 공격/방어효과를 부여
        foreach(Card cardList in deck)
        {
            // 카드에 공격 또는 방어효과를 할당
            cardList.Card_Effect = (Card.CardEffect)Random.Range(0,1);
        }
    }
    // 전투
    private void Battle()
    {
        // 플레이어hp가 0이 될때 까지 카드게임을 진행
        while(playerList[0].hp <= 0 || playerList[1].hp <= 0)
        {
            CardGameLogic(playerList[0], playerList[1]);
            // 두 플레이어가 남은 카드가 없을 경우
            if(CardCheck())
            {
                // 다시 카드를 나눠가짐
                CardReset();
            }
        }
    }
    // 카드 게임 로직
    private void CardGameLogic(CardGamePlayer upPlayer, CardGamePlayer downPlayer)
    {
        int index = 0;
        if(upPlayer != null && downPlayer !=null)
        {
            // 공격 <-> 공격
            if(upPlayer.cards[index].Card_Effect == Card.CardEffect.Attack && downPlayer.cards[index].Card_Effect == Card.CardEffect.Attack)
            {
                // 주사위를 굴려 숫자가 높은 쪽이 공격

                return;
            }
            // 공격 <-> 방어, 방어 <-> 방어
            else if((upPlayer.cards[index].Card_Effect == Card.CardEffect.Attack && downPlayer.cards[index].Card_Effect == Card.CardEffect.Defense)||
                    (upPlayer.cards[index].Card_Effect == Card.CardEffect.Defense && downPlayer.cards[index].Card_Effect == Card.CardEffect.Attack)||
                    (upPlayer.cards[index].Card_Effect == Card.CardEffect.Defense && downPlayer.cards[index].Card_Effect == Card.CardEffect.Defense))
            {
                // 두 플레이어 전부 카드를 잃음


                return;
            }
            // 블러핑 <-> 공격
            else if((upPlayer.cards[index].Card_Effect == Card.CardEffect.Attack && downPlayer.cards[index].Card_Effect == Card.CardEffect.Bluffing)||
                    (upPlayer.cards[index].Card_Effect == Card.CardEffect.Bluffing && downPlayer.cards[index].Card_Effect == Card.CardEffect.Attack))
            {
                // Bluffing을 낸 플레이어가 공격당함


                return;
            }
            // 블러핑 <-> 방어
            else if((upPlayer.cards[index].Card_Effect == Card.CardEffect.Defense && downPlayer.cards[index].Card_Effect == Card.CardEffect.Bluffing)||
                    (upPlayer.cards[index].Card_Effect == Card.CardEffect.Bluffing && downPlayer.cards[index].Card_Effect == Card.CardEffect.Defense))
            {
                // Defense를 낸 플레이어가 카드를 잃음

                return;
            }
        }
    }
    // 플레이어의 남은 카드를 확인
    private bool CardCheck()
    {
        int count = 0;
        
        // 플레이어의 카드수를 확인 하고 count에 값을 더함
        foreach(CardGamePlayer player in playerList)
        {
            count += player.cards.Count;
        }
        
        // 남은 카드가 없다면 true 하나라도 있다면 false를 반환
        return (count == 0) ? true : false;
    }
    // 카드 5장을 새로 나눠 가짐
    private void CardReset()
    {
        //남은 카드가 없을 경우 카드를 나눠가짐
    }
}
