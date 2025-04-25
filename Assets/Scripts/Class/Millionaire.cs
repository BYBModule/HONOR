using UnityEngine;

public class Millionaire : IClass
{   
    // 직업 초기 스테이터스
    public Status status;
    // 스테이터스 초기값을 생성하고 플레이어 스테이터스에 대입
    public Millionaire(Player player)
    {
        status = new Status();
        status.maxHp = 500;
        status.strength = 20;
        status.intelligence = 20;
        status.hitRate = 20;
        status.evasion = 25;
        status.luck = 40;
        Set_Status(player, status);
        AttackDamage(player);
    }
    public void AttackDamage(Player player)
    {
        player.Player_Data.attackDamage = (int)(player.status.strength * 1.5);
    }

    // 용병 고용(특수 승리)
    public void ClassAbility()
    {
    }
    public void Set_Status(Player player, Status status)
    {
        player.status = status;
    }
    public void Update_Status(Status status)
    {
        this.status = status;
    }
}
