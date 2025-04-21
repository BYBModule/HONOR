using System;
using UnityEngine;

public class DiceRotate : MonoBehaviour
{
    // 메인 카메라 변수
    private Camera mainCamera;
    // 카메라의 z값을 저장
    private float cameraZDistance;
    // 리지드바디
    private Rigidbody rigidbody;
    // 랜덤으로 z회전 속도를 저장하기 위한 변수수
    private float randomZRotationVelocity;
    // 회전 속도
    public float rotationSpeed = 150;
    void Start()
    {
        // 오브젝트의 RigidBody를 가져옵니다
        rigidbody = GetComponent<Rigidbody>();
        // 메인 카메라 오브젝트를 연결
        mainCamera = Camera.main;
        // 스크린 뷰 상의 z값을 변수에 저장장
        cameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z;
    }

    private void OnMouseDown()
    {
        // 마우스 클릭 시 무작위 회전값 발생
        randomZRotationVelocity = UnityEngine.Random.Range(-1.0f, 1.0f);
    }

    private void OnMouseDrag()
    {
        // 마우스의 좌표를 저장하는 변수
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
        // 카메라의 월드 포지션 좌표를 사용하여 newWorldPosition변수를 초기화
        Vector3 newWorldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        // 오브젝트의 위치값을 새로운 월드포지션으로 초기화
        transform.position = newWorldPosition;
        // 다이스 회전
        rigidbody.angularVelocity += new Vector3(x:Input.GetAxis("Mouse Y"), y:Input.GetAxis("Mouse X"), randomZRotationVelocity) * rotationSpeed;
    }

}
