using UnityEngine;

public interface IClass
{
    // 공격 데미지
    public void AttackDamage(Player player);
    // 특수 능력
    public void ClassAbility();
    // 기본 스텟
    public void Set_Status(Player player, Status status);
}
