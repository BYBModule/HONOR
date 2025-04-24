using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    // 카드
    public enum CardEffect
    {
        // 공격
        Attack = 0,
        // 수비
        Defense = 1,
        // 블러핑
        Bluffing = 2,
    }
    // 카드 원본
    public GameObject cardPrefab;
    public SpriteRenderer card;
    public Sprite cardFront;
    public Sprite cardBack;
    
    [HideInInspector]
    public CardEffect Card_Effect;
    
    [HideInInspector]
    public bool isFront;

    void Awake()
    {
        Initialize();    
    }

    public void SetUp(bool isFront)
    {
        this.isFront = isFront;
        if(isFront)
        {
            card.sprite = cardFront;        
        }
        else
        {
            card.sprite = cardBack;
        }
    }
    public void Initialize()
    {
        SetUp(false);
        Instantiate(cardPrefab, transform.position, Quaternion.identity);    
    }
}
