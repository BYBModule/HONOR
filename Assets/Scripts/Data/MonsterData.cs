using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    public string MonsterType;
    public int MaxHp;
    public int CurrentHp;
    public int AttackDamage;
    public int DropGold;
}
