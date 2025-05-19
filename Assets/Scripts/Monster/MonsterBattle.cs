using System.Collections.Generic;
using UnityEngine;

public class MonsterBattle : MonoBehaviour
{
    public enum Type
    {
        Normal,
        Elite,
        Boss,
    }
    [SerializeField] private List<Monster> monsterList;
    [SerializeField] private List<Monster> aliveMonster;
    public Transform position;
    int monsterMaxCount = 4;
    float elapsedTime;
    // 전투 중 
    bool isBattle = true;
    bool battleTurn;
    void Awake()
    {

    }
    // 5턴이 지날때 마다 몬스터의 공격력과 체력을 증가시킴
    public void UpdateMonsterState(int turnCount)
    {
        if (turnCount % 5 == 0 && (int)turnCount / 5 != 0)
        {
            foreach (Monster monster in monsterList)
            {
                if (monster.monsterData.MonsterType == "Boss")
                {
                    return;
                }
                monster.attackDamage += 3 * turnCount;
                monster.maxHp += 10 * turnCount;
                monster.currentHp += 10 * turnCount;
            }
        }
    }

    // 몬스터 생성
    public void MonsterSpawnPoint(LayerMask input, Transform parent)
    {
        int randomEncounter = Random.Range(0, 100);
        // 플레이어가 밟은 발판의 레이어를 체크합니다.
        if (LayerMask.LayerToName(input) == "Floor")
        {
            if (aliveMonster.Count < monsterMaxCount)
            {
                // 플로어에서 몬스터를 생성
                if (randomEncounter > 70)
                {
                    aliveMonster.Add(Instantiate(monsterList[Random.Range(0, 2)], parent.position + new Vector3(0, 0.35f, 0), Quaternion.identity, parent));
                }
            }
        }
        else if (LayerMask.LayerToName(input) == ("Elite"))
        {
            // 엘리트 레이드로 진입하는 발판
            Instantiate(monsterList[2], parent.position + new Vector3(0, 0.3f, 0), Quaternion.identity, parent);
        }
        else if (LayerMask.LayerToName(input) == ("Boss"))
        {
            // 보스 몬스터로 진입하는 발판
        }
        else
        {
            // 플로어에 레이아웃이 없을 때
            Debug.LogError("FloorLayer Empty");
        }

    }
    // 몬스터를 스폰하는 메서드
    public void SpawnMonster(Dictionary<int, Transform> fields)
    {
        LayerMask input;
        foreach (Transform field in fields.Values)
        {
            input = field.gameObject.layer;
            // 몬스터 생성
            MonsterSpawnPoint(input, field);
        }
    }
    // 플레이어와 몬스터 턴 전환
    public void TurnToggle()
    {
        battleTurn = !battleTurn;
    }
    // 몬스터 전투
    public void Battle(Player player, Monster monster)
    {
        if (player.IsPlayerAlive() == true)
        {
            monster.UpdateMonsterHp(player.Attack());
        }
        else
        {
            player.PlayerDead();
            isBattle = false;
        }
        if (monster.IsMonsterAlive() == true)
        {
            player.UpdateHp(monster.attackDamage);
        }
        else
        {
            monster.MonsterDead();
            isBattle = false;
        }
        TurnToggle();

    }
}
