using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dice : MonoBehaviour
{
    // 주사위의 값이 표기될 텍스트
    public TextMeshProUGUI upperSideText;
    // 방향 변수
    public Vector3Int directionValues;
    // 역방향 변수
    private Vector3Int opposingDirectionValues;
    List<string> FaceRepresent = new List<string>() {"", "1", "2", "3", "4", "5", "6"};

    void Start()
    {
        opposingDirectionValues = 7 * Vector3Int.one - directionValues;
    }
    void Update()
    {
        // 위치값의 변화가 있다면
        if(transform.hasChanged)
        {
            if((int) Vector3.Cross(Vector3.up, transform.right).magnitude == 0)
            {
                if(Vector3.Dot(Vector3.up, transform.right)> 0)
                {
                    upperSideText.text = FaceRepresent[directionValues.x];
                }
                else{
                    upperSideText.text = FaceRepresent[opposingDirectionValues.x];
                }
            }
            else if ((int) Vector3.Cross(Vector3.up, transform.up).magnitude == 0)
            {
                if(Vector3.Dot(lhs: Vector3.up, rhs: transform.up) > 0)
                {
                    upperSideText.text = FaceRepresent[directionValues.y];
                }
                else
                {
                    upperSideText.text = FaceRepresent[opposingDirectionValues.y];
                }
            }
        }
        transform.hasChanged = false;
    }
}
