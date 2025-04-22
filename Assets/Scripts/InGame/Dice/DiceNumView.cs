using TMPro;
using UnityEngine;

public class DiceNumView : MonoBehaviour
{
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
