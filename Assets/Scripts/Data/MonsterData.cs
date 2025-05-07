using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Scriptable Objects/MonsterData")]
public class MonsterData : ScriptableObject
{
    private string monsterType;
    public string MonsterType;
    private int maxHp;
    public int MaxHp;
    private int currentHp;
    public int CurrentHp;
    private int attackDamage;
    public int AttackDamage;
}
