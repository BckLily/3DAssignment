using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // 변수를 상수(고정된 값으로) 저장하는 것.
    const string bulletTag = "BULLET";

    float hp = 100f; // 체력
    GameObject bloodEffect; // 혈흔 효과




    // Start is called before the first frame update
    void Start()
    {
        // Load 함수는 예약 폴더인 Resources에서
        // 데이터를 불러오는 함수이다.
        // Load<데이터 유형>("파일의 경로");
        // 최상위 경로는 Resources 폴더.
        // 현재 Blood 파일은 Resources의 바로 안에 있기 때문에 Blood만 적어주면 된다.
        // 만약 하위 폴더가 존재해서 그 폴더안에 원하는 파일이 있을 경우
        // "Prefabs/Blood"와 같은 방식으로 수정한다.
        // 파일의 경로는 하위 폴더명 + 파일명까지 정확하게 경로를 명시.
        bloodEffect = Resources.Load<GameObject>("Blood");

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(bulletTag))
        {
            // 혈흔 효과 함수 호출
            ShowBloodEffect(collision);
            // 총알 삭제
            Destroy(collision.gameObject);
            // 데미지를 주는 물체에 데미지나 특성 같은 것을 추가해주면
            // 데미지를 받는 물체에서 어떤 물체가 접촉하였는지 확인하기만 하면
            // 어떠한 데미지를 받았는지 확인을 할 수 있다.
            hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;


            // 실수 값의 경우 1이 아닌 0.999 같이 표시될 수 있기 때문에 
            // 정확히 같아야하는 ==이 아닌 <=을 사용한다.
            // 체력이 0이하일 경우 적이 죽었다고 판단.
            if (hp <= 0)
            {
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
            }
        }
    }

    void ShowBloodEffect(Collision coll)
    {
        // 충돌 위치 값 가져오기
        Vector3 pos = coll.contacts[0].point;
        // 충돌 위치의 법선 벡터(총알이 날아온 방향)
        Vector3 _normal = coll.contacts[0].normal;
        // 총알이 날아온 방향값 계산.
        Quaternion rot = Quaternion.FromToRotation(Vector3.back, _normal);
        // 혈흔 효과 동적 생성
        //GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot); 동일한 내용
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        // 1초후 삭제
        Destroy(blood, 1f);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
