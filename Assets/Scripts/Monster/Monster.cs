using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterData monsterData;
    public int attackDamage;
    public int maxHp;
    public int currentHp;
    public int dropGold;
    void Awake()
    {
        this.attackDamage = monsterData.AttackDamage;
        this.maxHp = monsterData.MaxHp;
        this.currentHp = monsterData.MaxHp;
        this.dropGold = monsterData.DropGold;
    }
    // 몬스터의 생존여부를 확인하기 위한 메서드
    public bool IsMonsterAlive()
    {
        return currentHp > 0 ? true : false;
    }
    public int Attack()
    {
        return -attackDamage;
    }
    // 몬스터 증가/감소
    public void UpdateMonsterHp(int interaction)
    {
        currentHp += interaction;
    }
    public void MonsterDead()
    {
        Destroy(this.gameObject);
    }
}
