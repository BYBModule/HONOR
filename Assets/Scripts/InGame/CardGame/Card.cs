using Unity.Mathematics;
using UnityEngine;

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
    // 카드 이미지를 가지고 있는 스프라이트 랜더러
    [HideInInspector] public GameObject card;
    // 카드의 앞면
    public GameObject cardFront;
    // 카드의 뒷면
    public GameObject cardBack;
    public CardEffect Card_Effect;
    
    private bool isFront = false;

    public void SetUp(bool isFront)
    {
        this.isFront = isFront;
        if(isFront)
        {
            card = cardFront;        
        }
        else
        {
            card = cardBack;
        }
    }
}