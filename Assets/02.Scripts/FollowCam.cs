using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // 카메라가 추적할 대상
    public Transform target;
    // 추적 대상이 이동하였을 때 카메라가 부드럽게 따라가기 위해서 사용하는 변수들.
    // 이동 속도 계수
    public float moveDamping = 15f;
    // 회전 속도 계수
    public float rotateDamping = 10f;
    // 추적 대상과의 거리
    public float distance = 5f;
    // 추적 대상과의 높이
    public float height = 4f;
    // Player 모델의 높이가 2정도 될 것이라 오브젝트 기준이 되는 바닥이 아닌 머리쪽을 볼 수 있게 차이를 줌.
    // 추적 좌표의 오프셋
    public float targetOffset = 2f;

    private Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // 콜백함수 - 호출을 따로 하지 않아도 알아서 작동하는 함수
    // 이벤트 트리거 등 여러가지 사용법이 있다.
    // Update is called once per frame
    void LateUpdate()
    {
        var camPos = target.position - (target.forward * distance) + (target.up * height);

        // 지금의 위치와 움직여야할 위치를 구형 보간(완만하게 처리)한 후 카메라의 이동을 부드럽게 한다.
        // Slerp: Spherically interpolates between two vectors.
        tr.position = Vector3.Slerp(tr.position, camPos, Time.deltaTime * moveDamping);
        // Quaternion >> 쿼터니언 각. 일반적으로 사용하는 x, y ,z 에 w라는 새로운 값을 사용해서
        // 오일러 각에서 벡터간의 충돌 시 발생할 수 있는 오류를 없애서 사용하는 각도.
        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotateDamping);
        // 기존의 발바닥을 쳐다보는 카메라를 오프셋만큼 위쪽을 보도록 조정.
        tr.LookAt(target.position + (target.up * targetOffset));
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        // DrawWireSphere(위치, 지름)
        // 선으로 이루어진 구형의 모양을 그림(씬뷰에만 표시된다. 디버그 용도)
        // 타겟 지점(Player의 머리 부분)에 생성.
        Gizmos.DrawWireSphere(target.position + target.up * targetOffset, 0.1f);

        // 메인 카메라와 추적 지점 사이에 선을 그린다.
        // DrawLine(출발 지점, 도착 지점)
        // 출발과 도착 지점 사이에 선을 그린다.
        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);
    }
}
