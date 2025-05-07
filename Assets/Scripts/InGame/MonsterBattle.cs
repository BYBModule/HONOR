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
    [SerializeField] private List<MonsterData> monsters;

    // 몬스터를 스폰하는 메서드
    private void SpawnMonster(Type type, LayerMask input)
    {
        // 플레이어가 밟은 발판의 레이어를 체크합니다.
        if(input.Equals("Floor"))
        {
            // 플로어에서 일정확률로 일반 몬스터와 조우
        }
        else if (input.Equals("Elite"))
        {
            // 엘리트 레이드로 진입하는 발판
        }
        else if (input.Equals("Boss"))
        {
            // 보스 몬스터로 진입하는 발판
        }
        else
        {
            // 플로어에 레이아웃이 없을 때
            Debug.LogError("FloorLayer Empty");
        }
    }
}
