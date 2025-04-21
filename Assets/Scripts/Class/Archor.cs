using UnityEngine;

public class Archor : IClass
{
    // 직업 초기 스테이터스
    public Status status;
    // 스테이터스 초기값을 생성하고 플레이어 스테이터스에 대입
    public Archor(Player player)
    {
        status.maxHp = 420;
        status.strength = 35;
        status.intelligence = 10;
        status.hitPercent = 35;
        status.evasion = 30;
        status.luck = 20;
        Set_Status(player, status);
        AttackDamage(player);
    }
    public void AttackDamage(Player player)
    {
        player.attackDamage = (int)(player.status.strength * 1.5);
    }

    // 일제 사격(모든 적 타겟 공격격)
    public void ClassAbility()
    {
    }
    public void Set_Status(Player player, Status status)
    {
        player.status = status;
    }
}
