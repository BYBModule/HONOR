using TMPro;
using UnityEngine;

public class CheckDiceNum : MonoBehaviour
{
    // 회전하는 주사위
    DiceRoll dice;
    void Awake()
    {
        // DiceRoll이 포함된 오브젝트를 연결
        dice = FindAnyObjectByType<DiceRoll>();
    }
    // 주사위의 한 면이 땅에 닿았을 때
    private void OnTriggerStay(Collider other)
    {
        if(dice != null)
        {
            // 주사위가 완전히 멈췄을 때때
            if(dice.GetComponent<Rigidbody>().linearVelocity == Vector3.zero)
            {
                // 닿은 면의 반대에 있는 숫자를 dicFaceNum에 저장
                dice.diceFaceNum = int.Parse(other.name);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        dice.scoreText.gameObject.SetActive(false);
    }
}
