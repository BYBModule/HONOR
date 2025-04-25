using System.Collections.Generic;
using UnityEngine;

    // 카드 게임 플레이어
    public class CardGamePlayer
    {
        // 플레이어 스탯
        public Status status;

        public string className;
        
        public int playerId;
        // 체력
        public int hp;
        
        // 공격력
        public int attackDamage;
        
        // 현재 가지고 있는 카드
        public List<Card> cards;

        // 현재 플레이어 턴
        public bool isPlayerturn = false;
        
        // CardGamePlayer 생성자
        public CardGamePlayer(int hp, int attackDamage, string className, Status status)
        {
            
            this.status = status;
            this.className = className;
            this.hp = hp;
            this.attackDamage = attackDamage;
            isPlayerturn = !isPlayerturn;
        }
    }
