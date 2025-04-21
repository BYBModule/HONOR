using UnityEngine;

public class Paladin : IClass
{
    // 직업 초기 스테이터스
    public Status status;
    // 스테이터스 초기값을 생성하고 플레이어 스테이터스에 대입
    public Paladin(Player player)
    {
        status.maxHp = 570;
        status.strength = 30;
        status.intelligence = 10;
        status.hitPercent = 20;
        status.evasion = 20;
        status.luck = 20;
        Set_Status(player, status);
        AttackDamage(player);
    }
    public void AttackDamage(Player player)
    {
        player.attackDamage = (int)(player.status.strength* 1.5);
    }

    // 기도(지정 보호막)
    public void ClassAbility()
    {
    }    
    public void Set_Status(Player player, Status status)
    {
        player.status = status;
    }
}
