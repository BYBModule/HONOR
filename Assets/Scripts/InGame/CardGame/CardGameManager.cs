using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardGameManager : MonoBehaviour
{
    // 카드게임 플레이어
    public List<CardGamePlayer> players;
    public Button button;
    public TextMeshPro turnCountText;
    public Card[] deck;
    public List<Player> playerList;
    public List<CardGameUI> cardGameUI;
    public GameObject cardPrefab;
    public Transform cardSpawnPoint;
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;
    public BattlePhase battlePhase;

    bool isPlayerturn = true;

    void Awake()
    {
        // 카드게임 플레이어 리스트 생성
        players = new List<CardGamePlayer>(2);
        // 카드 덱 리스트 생성
        deck = new Card[20];  
    }
    void Start()
    {
        // 카드게임 매니저 초기화
        Initialize();
        battlePhase.gameObject.SetActive(true);
    }
    private void Initialize()
    {
        // 플레이어 할당
        foreach(var player in playerList)
        {
            players.Add(new CardGamePlayer(player.Player_Data.playerHp, player.Player_Data.attackDamage, isPlayerturn, player.player_Class.ToString(), player.status));
            isPlayerturn = !isPlayerturn;
        }
        // 게임 UI 생성
        SettingUI(players);
        // 카드 덱 생성
        for(int i = 0; i < deck.Length; i++)
        {
            // 오브젝트 생성
            deck[i] = Instantiate(cardPrefab.GetComponent<Card>(), cardSpawnPoint.position, Quaternion.identity);
            deck[i].transform.parent = cardSpawnPoint;
            // 카드 세팅
            deck[i].SetUp(true);
            // 공격, 수비 효과 랜덤 부여
            deck[i].Card_Effect = (Card.CardEffect)Random.Range(0, 2);
        }
    }
    private void SettingUI(List<CardGamePlayer> players)
    {
        for(int i = 0; i < players.Count; i++)
        {
            // 플레이어 데이터를 UI에 세팅
            cardGameUI[i].TextSetting(players[i]);
        }
    }
    // 카드게임 시작
    public void StartCardGame()
    {
        CardSetting();
    }
    private void CardSetting()
    {
        if(CardCheck())
        {
            ResetCard(players[0], players[1]);    
        }
    }
    private bool CardCheck()
    {
        int count = 0;
        foreach(var player in players)
        {
            if(player.cards == null)
            {
                break;
            }
            if(player.cards.Count <= 0)
            {
                break;
            }
            else
            {
                if(player.cards[0].Card_Effect == Card.CardEffect.Bluffing)
                {
                    break;
                }
                else
                {
                    count++;
                }
            }
        }
        return (count == 0) ? true : false;
    }
    private void ResetCard(CardGamePlayer upPlayer, CardGamePlayer downPlayer)
    {
        if(upPlayer.cards == null)
        {
            upPlayer.cards = new List<Card>();
        }
        if(downPlayer.cards == null)
        {
            downPlayer.cards = new List<Card>();
        }
        CardGamePlayer advantagePlayer = Advantage(upPlayer, downPlayer);
        // upPlayer가 어드벤티지를 가지고 있다면 1장 추가
        if(advantagePlayer != null)
        {
            advantagePlayer.cards.Add(deck[Random.Range(0, deck.Length)]);
        }
        // 두 플레이어가 카드를 5장씩 나눠가짐
        for(int i = 0; i < 5; i++)
        {
            upPlayer.cards.Add(deck[Random.Range(0, deck.Length)]);
            downPlayer.cards.Add(deck[Random.Range(0, deck.Length)]);
        }
        // 모든 카드가 분배된 이후 블러핑 카드 1장을 추가로 가짐
        Card bluf = Instantiate(cardPrefab.GetComponent<Card>(), cardSpawnPoint.position, Quaternion.identity);
        bluf.Card_Effect = Card.CardEffect.Bluffing;
        bluf.SetUp(true);
        downPlayer.cards.Add(bluf);
        bluf.transform.rotation = Quaternion.Euler(0, 0, 180);
        bluf.SetUp(false);
        upPlayer.cards.Add(bluf);
        Destroy(bluf.gameObject);

        for(int i = 0 ; i < upPlayer.cards.Count; i++)
        {
            upPlayer.cards[i].SetUp(false);
            upPlayer.cards[i] = Instantiate(upPlayer.cards[i], player1SpawnPoint.position, Quaternion.Euler(0, 0, 180), player1SpawnPoint);    
        }
        for(int i = 0 ; i < downPlayer.cards.Count; i++)
        {
            downPlayer.cards[i].SetUp(true);
            downPlayer.cards[i] = Instantiate(downPlayer.cards[i], player2SpawnPoint.position, Quaternion.identity, player2SpawnPoint);
        }
    }
    // 어드벤티지 체크
    private CardGamePlayer Advantage(CardGamePlayer st, CardGamePlayer nd)
    {
        // 첫번째 플레이어의 적중수치와 두번째 플레이어의 회피수치를 비교했을때 1.5배 이상 높을경우
        if(st.status.hitRate >= nd.status.evasion * 1.5f ||
           st.status.evasion >= nd.status.hitRate * 1.5f)
        {
            return st;
        }
        // 두번째 플레이어의 적중수치와 첫번째 플레이어의 회피수치를 비교했을때 1.5배 이상 높을경우
        else if(nd.status.hitRate >= st.status.evasion * 1.5f ||
           nd.status.evasion >= st.status.hitRate * 1.5f)
        {
            return nd;
        }
        return null;
    }
    // 카드 게임 로직
    public void CardGameLogic(Card upPlayer, Card downPlayer)
    {
        if(upPlayer != null && downPlayer !=null)
        {
            if(upPlayer.Card_Effect == Card.CardEffect.Attack && downPlayer.Card_Effect == Card.CardEffect.Attack)
            {
                // 주사위 던지기
                players[0].cards.Remove(upPlayer);
                players[1].cards.Remove(downPlayer);
                Destroy(upPlayer.gameObject);
                Destroy(downPlayer.gameObject);
            }
            // 공격 <-> 방어, 방어 <-> 방어
            else if((upPlayer.Card_Effect == Card.CardEffect.Attack && downPlayer.Card_Effect == Card.CardEffect.Defense)||
                    (upPlayer.Card_Effect == Card.CardEffect.Defense && downPlayer.Card_Effect == Card.CardEffect.Attack)||
                    (upPlayer.Card_Effect == Card.CardEffect.Defense && downPlayer.Card_Effect == Card.CardEffect.Defense))
            {
                // 두 플레이어 전부 카드를 잃음
                players[0].cards.Remove(upPlayer);
                players[1].cards.Remove(downPlayer);
                Destroy(upPlayer.gameObject);
                Destroy(downPlayer.gameObject);
                return;
            }
            // 블러핑 <-> 공격
            else if((upPlayer.Card_Effect == Card.CardEffect.Attack && downPlayer.Card_Effect == Card.CardEffect.Bluffing)||
                    (upPlayer.Card_Effect == Card.CardEffect.Bluffing && downPlayer.Card_Effect == Card.CardEffect.Attack))
            {
                // Bluffing을 낸 플레이어가 공격당함
                if(upPlayer.Card_Effect == Card.CardEffect.Bluffing)
                {
                    players[0].hp -= players[1].attackDamage;
                    players[0].cards.Remove(upPlayer);
                    players[1].cards.Remove(downPlayer);
                    Destroy(upPlayer.gameObject);
                    Destroy(downPlayer.gameObject);
                    return;   
                }
                else
                {
                    players[1].hp -= players[0].attackDamage;
                    players[0].cards.Remove(upPlayer);
                    players[1].cards.Remove(downPlayer);
                    Destroy(upPlayer.gameObject);
                    Destroy(downPlayer.gameObject);
                    return;                  
                }
            }
            // 블러핑 <-> 방어
            else if((upPlayer.Card_Effect == Card.CardEffect.Defense && downPlayer.Card_Effect == Card.CardEffect.Bluffing)||
                    (upPlayer.Card_Effect == Card.CardEffect.Bluffing && downPlayer.Card_Effect == Card.CardEffect.Defense))
            {
                // Defense를 낸 플레이어가 카드를 잃음
                if(upPlayer.Card_Effect == Card.CardEffect.Defense)
                {
                    players[0].cards.Remove(upPlayer);
                    Destroy(upPlayer.gameObject);
                    return;
                }
                else
                {
                    players[1].cards.Remove(downPlayer);
                    Destroy(downPlayer.gameObject);
                    return;
                }
            }
        }
        else
        {
            // 1번 플레이어가 카드를 선택했을때
            if(upPlayer == null && downPlayer != null)
            {
                if(downPlayer.Card_Effect == Card.CardEffect.Attack)
                {
                    players[0].hp -= players[1].attackDamage;
                    players[1].cards.Remove(downPlayer);
                    Destroy(downPlayer);
                }
                else if(downPlayer.Card_Effect == Card.CardEffect.Defense)
                {
                    players[1].cards.Remove(downPlayer);
                    Destroy(downPlayer);
                }
            }
            // 2번 플레이어가 카드를 선택했을때
            else if(upPlayer != null && downPlayer == null)
            {
                if(upPlayer.Card_Effect == Card.CardEffect.Attack)
                {
                    players[1].hp -= players[0].attackDamage;
                    players[0].cards.Remove(upPlayer);
                    Destroy(upPlayer);
                }
                else if(upPlayer.Card_Effect == Card.CardEffect.Defense)
                {
                    players[0].cards.Remove(upPlayer);
                    Destroy(upPlayer);
                }
            }
        }

                
    }
    // 게임이 종료 되었을때 플레이어 리스트를 초기화
    private void OnDestroy()
    {
        Debug.Log("Game End");
        players.Clear();
        deck = null;
        playerList.Clear();    
    }
}
