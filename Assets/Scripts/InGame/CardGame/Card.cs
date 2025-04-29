using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Card : MonoBehaviour
{
    public TextMeshPro cardEffectText;
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
    [HideInInspector] public GameObject card;
    // 카드의 앞면
    public GameObject cardFront;
    // 카드의 뒷면
    public GameObject cardBack;
    public CardEffect Card_Effect;
    
    private bool isFront = true;

    public void SetUp(bool isFront)
    {
        this.isFront = isFront;
        cardEffectText.text = Card_Effect.ToString();
        cardFront.SetActive(isFront);
        cardBack.SetActive(!isFront);
        cardEffectText.gameObject.SetActive(isFront);

    }
}