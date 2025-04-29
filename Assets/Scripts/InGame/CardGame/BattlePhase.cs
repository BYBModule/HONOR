using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePhase : MonoBehaviour
{
    // 카드 생성 위치
    public Transform cardSpawnPoint;
    // 카드 원본
    public GameObject cardPrefab;
    // 주사위 프리팹
    public GameObject dicePrefab;
    public Button button;
    // 카드 덱 리스트
    public Card[] deck;
    
    // 플레이어 리스트
    public List<Player> players;
    
    // 카드 게임을 진행할 플레이어 리스트
    private List<CardGamePlayer> playerList;

    public List<CardGameUI> cardGameUI;

    // 1번 플레이어가 선택한 카드
    private Card upPlayerSelect = null;
    
    // 2번 플레이어가 선택한 카드
    private Card downPlayerSelect = null;

    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;
    // 턴 실행을 위한 변수
    public int selectCount = 0;

    // 턴 카운트
    private int turnCount = 0;
    
    

    void Awake()
    {
        playerList = new List<CardGamePlayer>(2);
        deck = new Card[20];
        button.onClick.AddListener(() => {
           TurnChange(playerList[0], playerList[1]);
        });
    }
    void Start()
    {
        // 초기화
        Initialize();
    }
    void Update()
    {
        Debug.Log($"{playerList[0].hp}, {playerList[0].attackDamage} // {playerList[1].hp}, {playerList[1].attackDamage} ");
        // 전투
        int count = 0;
        if(count < 1)
            Battle();
        
        count++;

    }    
    private void Initialize()
    {
        
        // players를 순회하며 player를 찾아낼 때
        foreach(Player player in players)
        {
            if(player.status != null)
            // 플레이어 데이터 리스트를 하나씩 추가함
            playerList.Add(new CardGamePlayer(player.Player_Data.playerHp, player.Player_Data.attackDamage, player.player_Class.ToString(), player.status));    
        }
        SettingUI(playerList);
        // 카드에 공격/방어효과를 부여
        for(int i = 0; i < deck.Length; i++)
        {
            // 카드덱 생성
            deck[i] = Instantiate(cardPrefab.GetComponent<Card>(), cardSpawnPoint.position, Quaternion.identity);
            deck[i].transform.position = cardSpawnPoint.position;
            deck[i].SetUp(false);
            // 카드에 공격 또는 방어효과를 할당
            deck[i].Card_Effect = (Card.CardEffect)Random.Range(0,2);
        }
    }
    // 게임이 시작할 때 UI를 세팅합니다.
    private void SettingUI(List<CardGamePlayer> playerList)
    {
        for(int index = 0; index < playerList.Count; index++)
        {
            cardGameUI[index].TextSetting(playerList[index]);
        }
    }
    // 전투
    private void Battle()
    {
        // 플레이어hp가 0이 될때 까지 카드게임을 진행
        Debug.Log($"{playerList[0].hp}, {playerList[1].hp}");
        while(playerList[0].hp > 0 && playerList[1].hp > 0)
        {
            // 두 플레이어가 남은 카드가 없을 경우
            if(CardCheck())
            {
                // 다시 카드를 나눠가짐
                CardReset(playerList[0], playerList[1]);
            }
            else
            {
                PlayerCardSelect(playerList[0], playerList[1]);
                // 두 플레이어가 카드를 선택했다면 로직 실행
                // CardGameLogic(upPlayerSelect, downPlayerSelect);
                playerList[0].hp = 0;
                return;
            }
        }
    }
    
    // 카드 게임 로직
    private void CardGameLogic(Card upPlayer, Card downPlayer)
    {
        Debug.Log("Card Game Logic");
        if(upPlayer != null && downPlayer !=null)
        {
            // 공격 <-> 공격
            if(upPlayer.Card_Effect == Card.CardEffect.Attack && downPlayer.Card_Effect == Card.CardEffect.Attack)
            {
                // 주사위를 굴려 숫자가 높은 쪽이 공격
                // while(selectCount <2)
                // {
                //     // 미구현
                //     // 이 부분을 diceroll 에서 가지고 와야힘
                //     // int upPlayerDice = 0;
                //     // int dowmPlayerDice = 0;
                    
                //     // GameObject dice = Instantiate(dicePrefab, Vector3.zero, Quaternion.identity);
                //     // dice.transform.position = Vector3.zero;    
                //     // dice.GetComponent<CheckDiceNum>().GetComponent<DiceRoll>().RollDice();
                // }
                // 주사위 숫자를 비교해서 공격
                
                
                return;
            }
            // 공격 <-> 방어, 방어 <-> 방어
            else if((upPlayer.Card_Effect == Card.CardEffect.Attack && downPlayer.Card_Effect == Card.CardEffect.Defense)||
                    (upPlayer.Card_Effect == Card.CardEffect.Defense && downPlayer.Card_Effect == Card.CardEffect.Attack)||
                    (upPlayer.Card_Effect == Card.CardEffect.Defense && downPlayer.Card_Effect == Card.CardEffect.Defense))
            {
                // 두 플레이어 전부 카드를 잃음
                playerList[0].cards.Remove(upPlayer);
                playerList[1].cards.Remove(downPlayer);
                return;
            }
            // 블러핑 <-> 공격
            else if((upPlayer.Card_Effect == Card.CardEffect.Attack && downPlayer.Card_Effect == Card.CardEffect.Bluffing)||
                    (upPlayer.Card_Effect == Card.CardEffect.Bluffing && downPlayer.Card_Effect == Card.CardEffect.Attack))
            {
                // Bluffing을 낸 플레이어가 공격당함
                if(upPlayer.Card_Effect == Card.CardEffect.Bluffing)
                {
                    playerList[0].hp -= playerList[1].attackDamage;
                    playerList[0].cards.Remove(upPlayer);
                    playerList[1].cards.Remove(downPlayer);    
                }
                else
                {
                    playerList[1].hp -= playerList[0].attackDamage;
                    playerList[0].cards.Remove(upPlayer);
                    playerList[1].cards.Remove(downPlayer);                    
                }

                return;
            }
            // 블러핑 <-> 방어
            else if((upPlayer.Card_Effect == Card.CardEffect.Defense && downPlayer.Card_Effect == Card.CardEffect.Bluffing)||
                    (upPlayer.Card_Effect == Card.CardEffect.Bluffing && downPlayer.Card_Effect == Card.CardEffect.Defense))
            {
                // Defense를 낸 플레이어가 카드를 잃음
                if(upPlayer.Card_Effect == Card.CardEffect.Defense)
                {
                    playerList[0].cards.Remove(upPlayer);
                }
                else
                {
                    playerList[1].cards.Remove(downPlayer);
                }
                return;
            }
        }
        // 한 플레이어만 카드를 선택했을때
        else
        {
            // 1번 플레이어가 카드를 선택했을때
            if(upPlayer == null && downPlayer != null)
            {
                if(upPlayer.Card_Effect == Card.CardEffect.Attack)
                {
                    playerList[1].hp -= playerList[0].attackDamage;
                    playerList[0].cards.Remove(upPlayer);
                }
                else if(upPlayer.Card_Effect == Card.CardEffect.Defense)
                {
                    playerList[0].cards.Remove(upPlayer);
                }
            }
            // 2번 플레이어가 카드를 선택했을때
            else if(upPlayer != null && downPlayer == null)
            {
                if(downPlayer.Card_Effect == Card.CardEffect.Attack)
                {
                    playerList[0].hp -= playerList[1].attackDamage;
                    playerList[1].cards.Remove(downPlayer);
                }
                else if(downPlayer.Card_Effect == Card.CardEffect.Defense)
                {
                    playerList[1].cards.Remove(downPlayer);
                }
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
            if(player.cards == null)
            {
                break; 
            }
            count += player.cards.Count;
        }
        
        // 남은 카드가 없다면 true 하나라도 있다면 false를 반환
        return (count == 0) ? true : false;
    }
    
    // 턴을 넘기는 턴체인지 메서드
    public void TurnChange(CardGamePlayer upPlyer, CardGamePlayer downPlayer)
    {
        Debug.Log("Turn Change");
        upPlyer.isPlayerturn = !upPlyer.isPlayerturn;
        downPlayer.isPlayerturn = !downPlayer.isPlayerturn;
    }

    // 플레이어가 카드를 선택하는 메서드
    private void PlayerCardSelect(CardGamePlayer upPlayer, CardGamePlayer downPlayer)
    {
        Debug.Log("Player Card Select");
        // 양 플레이어가 카드를 선택할 때까지 반복
        while(selectCount < 2)
        {
            CardSelect(upPlayer, downPlayer);
            selectCount++;
        }
        selectCount = 0;
    }

    // 카드 선택 메서드
    private void CardSelect(CardGamePlayer upPlayer, CardGamePlayer downPlayer)
    {
        Debug.Log("Card Select");
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
                // 턴 넘김
                TurnChange(upPlayer, downPlayer);
            }
        }
        else if(downPlayer.isPlayerturn == true)
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
                // 턴을 넘김
                TurnChange(upPlayer, downPlayer);
            }    
        }
    }

    // 선택된 카드를 반환하는 메서드
    private Card SelectedCard(List<Card> cards)
    {
        Debug.Log("Selected Card");
        int selectCardIndex = 0;


        // 미구현
        // UI를 씬창에서 먼저 구현해야함

        // 플레이어가 카드를 선택하는 UI를 보여줌

        // 플레이어가 카드를 선택했다면 턴을 넘김
        return cards[0];
    }

    // 카드 5장을 새로 나눠 가짐
    private void CardReset(CardGamePlayer upPlayer, CardGamePlayer downPlayer)
    {
        if(upPlayer.cards == null)
        {
            upPlayer.cards = new List<Card>();
        }
        if(downPlayer.cards == null)
        {
            downPlayer.cards = new List<Card>();
        }
        // upPlayer가 어드벤티지를 가지고 있다면 1장 추가
        if(Advantage(upPlayer, downPlayer))
        {
            upPlayer.cards.Add(deck[Random.Range(0, deck.Length)]);       
        }
        // downPlayer가 어드벤티지를 가지고 있다면 1장 추가
        else if(Advantage(downPlayer, upPlayer))
        {
            downPlayer.cards.Add(deck[Random.Range(0, deck.Length)]);    
        }
        // 두 플레이어가 카드를 5장씩 나눠가짐
        for(int i = 0; i < 5; i++)
        {
            upPlayer.cards.Add(deck[Random.Range(0, deck.Length)]);
            downPlayer.cards.Add(deck[Random.Range(0, deck.Length)]);
        }
        // 모든 카드가 분배된 이후 블러핑 카드 1장을 추가로 가짐짐
        Card bluf = Instantiate(cardPrefab.GetComponent<Card>(), cardSpawnPoint.position, Quaternion.identity);
        bluf.Card_Effect = Card.CardEffect.Bluffing;
        bluf.SetUp(true);
        downPlayer.cards.Add(bluf);
        bluf.transform.rotation = Quaternion.Euler(0, 0, 180);
        bluf.SetUp(false);
        upPlayer.cards.Add(bluf);


        for(int i = 0 ; i < upPlayer.cards.Count; i++)
        {
            upPlayer.cards[i].SetUp(false);
            Instantiate(upPlayer.cards[i], player1SpawnPoint.position, Quaternion.Euler(0, 0, 180), player1SpawnPoint);    
        }
        for(int i = 0 ; i < downPlayer.cards.Count; i++)
        {
            downPlayer.cards[i].SetUp(true);
            Instantiate(downPlayer.cards[i], player2SpawnPoint.position, Quaternion.identity, player2SpawnPoint);
        }
        
        Debug.Log("Card Reset"); 
    }

    // 어드벤티지 확인 메서드
    private bool Advantage(CardGamePlayer check, CardGamePlayer target)
    {
        // 적중수치와 회피수치를 비교했을때 1.5배 이상 높을경우
        if(check.status.hitRate >= target.status.evasion * 1.5f ||
           check.status.evasion >= target.status.hitRate * 1.5f)
        {
            return true;
        }
        return false;
    }
    
    // 게임이 종료 되었을때 플레이어 리스트를 초기화
    private void OnDestroy()
    {
        Debug.Log("Game Over");
        players.Clear();
        deck = null;
        playerList.Clear();    
    }
}
