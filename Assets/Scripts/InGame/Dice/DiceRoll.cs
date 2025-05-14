using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class DiceRoll : MonoBehaviour
{
    // 리지드 바디
    Rigidbody rigidbody;
    
    // 주사위의 숫자를 나타내기위한 텍스트
    public TextMeshProUGUI scoreText;
    // 주사위 회전 시 XYZ축으로 랜덤하게 힘을 가하기 위한 변수
    [SerializeField] private float maxRandomForceValue;
    
    // 주사위를 굴릴 때 위로 가해지는 힘힘
    [SerializeField] private float startRollingForce;

    // 각 축으로 가해지는 힘
    private float forceX;
    private float forceY;
    private float forceZ;

    // 주사위의 값
    public int diceFaceNum = 0;
    
    void Awake()
    { 
        // 초기화
        Initialize();
    }
    public void ChangeRigidBody()
    {
        transform.localPosition = Vector3.up; 
        rigidbody.useGravity = !rigidbody.useGravity;
        rigidbody.isKinematic = !rigidbody.isKinematic;
    }
    // 주사위를 굴렸을 때 실행되는 메서드
    public void RollDice()
    {
        diceFaceNum = 0;
        ChangeRigidBody();
        // 각 축에 랜덤한 수 를 할당
        forceX = UnityEngine.Random.Range(0, maxRandomForceValue);
        forceY = UnityEngine.Random.Range(0, maxRandomForceValue);
        forceZ = UnityEngine.Random.Range(0, maxRandomForceValue);

        // 위쪽방향으로 힘을 가함
        rigidbody.AddForce(Vector3.up * startRollingForce);
        // xyz값으로 회전시키는 힘을 가함
        rigidbody.AddTorque(forceX, forceY, forceZ);

    }

    // 초기화
    private void Initialize()
    {
        rigidbody = GetComponent<Rigidbody>();
        transform.rotation = new Quaternion(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), 0);
        // 주사위가 중력작용을 받아 땅과 충돌하게 설정
        rigidbody.useGravity = false;
        // 주사위에 가해지는 힘을 받기위해 Kinematic값을 False로 전환
        rigidbody.isKinematic = true;
    }
}
