using UnityEngine;

public class PlayerData
{
    // 플레이어 체력
    public int playerHp;
    // 플레이어 이름
    public string playerName;
    // 플레이어 공격력
    public int attackDamage;
    // 소지 골드
    public int playerGold;

    // 플레이어 데이터 생성자(모든 변수 초기화)
    public PlayerData()
    {
        playerHp = 0;
        playerName = "";
        attackDamage = 0;
        playerGold = 0;
    }
    
}
