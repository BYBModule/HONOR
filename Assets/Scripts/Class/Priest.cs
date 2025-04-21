using UnityEngine;

public class Priest : IClass
{
    // 직업 초기 스테이터스
    public Status status;
    // 스테이터스 초기값을 생성하고 플레이어 스테이터스에 대입
    public Priest(Player player)
    {
        status.maxHp = 400;
        status.strength = 20;
        status.intelligence = 45;
        status.hitPercent = 25;
        status.evasion = 25;
        status.luck = 20;
        Set_Status(player, status);
        AttackDamage(player);
    }
    public void AttackDamage(Player player)
    {
        player.attackDamage = (int)(player.status.intelligence * 1.5);
    }
    // 빛(아군 회복)
    public void ClassAbility()
    {
    }

    public void Set_Status(Player player, Status status)
    {
        player.status = status;
    }
}
