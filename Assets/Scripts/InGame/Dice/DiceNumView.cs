using TMPro;
using UnityEngine;

public class DiceNumView : MonoBehaviour
{
    // 주사위값을 가져올 DiceRoll변수
    DiceRoll dice;
    // 주사위의 숫자를 나타내기위한 텍스트
    [SerializeField] TextMeshProUGUI scoreText;
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
                scoreText.text = dice.diceFaceNum.ToString();
            }
            else
            {
                scoreText.text = "0";
            }
        }
    }
}
