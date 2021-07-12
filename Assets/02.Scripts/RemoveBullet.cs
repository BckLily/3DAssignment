using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect; // 스파크 프리팹

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌체에 대한 정보가 담겨있는 곳: collision
        // 충돌이 발생한 것들 중에서 TAG가 BULLET인 것들만 검출
        if (collision.collider.tag == "BULLET")
        {
            // 스파크 이펙트 함수 호출.
            // 스파크를 발생시키기 위한 함수.
            ShowEffect(collision);

            // 충돌이 발생한 오브젝트 삭제
            Destroy(collision.gameObject);
            // 충돌이 발생한 후 원하는 시간이 지난 후 제거
            // Destroy(collision.gameObject, 5f);
            // 자료는 남아있지만 화면에서 보이지 않게 비활성화 하는 것.
            // collision.gameObject.SetActive(false);
        }

    }

    void ShowEffect(Collision coll)
    {
        // 충돌 지점의 정보를 가지고 온다.
        // 충돌 시 발생한 최초의 위치 정보.
        ContactPoint contact = coll.contacts[0];
        // FromToRotation >> 회전시키고자 하는 벡터, 타겟 벡터를 회전시킴.
        // contact.normal >> 법선 벡터(면에 수직인 벡터)
        // -Vector3.forward >> 충돌한 면의 반대쪽 방향으로 해줘야 발사한 당사자가 Effect를 볼 수 있으므로 회전시킨다.
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        // 총알이 발사된 위치로 이펙트를 조금 이동. (드럼통에서 조금 띄워서 총알 자국 생성)
        Vector3 point = contact.point + (-contact.normal * 0.05f);
        // 충돌이 난 후 이펙트의 효과 방향(Z)을 법선 벡터(총알이 날아온 방향 (-Z))으로 돌려서 생성.
        GameObject spark = Instantiate(sparkEffect, point, rot);

        // spark가 발생하고 난 후, Object의 Parent를 this Object로 설정하여,
        // this Object가 이동을 하게 될 경우 Object에 고정된 위치에서 같이 이동하게 만들었음.
        spark.transform.SetParent(this.transform);

        
    }

}
