using System.Collections.Generic;
using UnityEngine;

public class BattlePhase : MonoBehaviour
{
    enum Card
    {
        // 공격
        Attack,
        // 수비
        Defense,
        // 블러핑
        Bluffing,
    }
    // 플레이어 리스트
    List<Player> players;
    // 플레이어의 데이터를 받을 리스트
    List<PlayerData> playerDatas;

    void Awake()
    {
        // players를 순회하며 player를 찾아낼 때
        foreach(Player player in players)
        {
            // 플레이어 데이터 리스트를 하나씩 추가함
            playerDatas.Add(player.Player_Data);
        }
    }

    // 카드 5장을 새로 나눠 가짐
    void CardReset(Player UpPlayer, Player DownPlayer)
    {
        
    }
}
