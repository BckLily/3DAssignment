using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // 총알 공격력
    public float damage = 20f;
    // 총알 속도
    public float speed = 1000f;
    

    // Start is called before the first frame update
    void Start()
    {
        // this Object에 자동으로 적용되는 Component. AddForce는 힘을 가하는 방향.
        // RIgidbody를 rb로 명시하고 적용하는 것도 할 수 있음.
        // 다른 Class에서 호출하거나 지금 Class에서 호출하거나 하지 않기 때문에 저렇게 할 수 있다.
        // 추가적인 조작이 없을 경우 명시하지 않고 할 수 있다.
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
