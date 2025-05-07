using TMPro;
using UnityEngine;

public class DiceNumView : MonoBehaviour
{
    // 주사위값을 가져올 DiceRoll변수
    DiceRoll dice;

    void Awake()
    {
        // DiceRoll이 포함된 오브젝트를 연결
        dice = FindAnyObjectByType<DiceRoll>();
    }
    private void Update()
    {
        // 주사위를 굴린 후 변화된 숫자를 텍스트로 출력
        if(dice != null)
        {
            if(dice.diceFaceNum != 0)
            {
                dice.scoreText.text = dice.diceFaceNum.ToString();
                dice.scoreText.gameObject.SetActive(true);
            }
            else
            {
                dice.scoreText.text = "0";
            }
        }
    }
}
