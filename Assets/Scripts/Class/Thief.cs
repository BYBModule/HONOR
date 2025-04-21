using UnityEngine;

public class Thief : IClass
{
    // 직업 초기 스테이터스
    public Status status;
    // 스테이터스 초기값을 생성하고 플레이어 스테이터스에 대입
    public Thief(Player player)
    {
        status.maxHp = 400;
        status.strength = 25;
        status.intelligence = 20;
        status.hitPercent = 25;
        status.evasion = 35;
        status.luck = 20;
        Set_Status(player, status);
        AttackDamage(player);
    }
    public void AttackDamage(Player player)
    {
        player.attackDamage = (int)((player.status.strength * 1.2 + player.status.evasion * 2)/2);
    }
    // 신속(이동거리 증가)
    public void ClassAbility()
    {
    }    
    public void Set_Status(Player player, Status status)
    {
        player.status = status;
    }
}
