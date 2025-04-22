using UnityEngine;

public interface IClass
{

    // 특수 능력
    public void ClassAbility();
    // 공격 데미지
    public void AttackDamage(Player player);
    // 스텟 세팅
    public void Set_Status(Player player, Status status);
    // 스탯증감을 업데이트하기 위한 메서드
    public void Update_Status(Status status);
}
