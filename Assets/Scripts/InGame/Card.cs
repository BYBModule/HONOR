using UnityEditor.Rendering;
using UnityEngine;

public class Card
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
    public CardEffect Card_Effect;
}
