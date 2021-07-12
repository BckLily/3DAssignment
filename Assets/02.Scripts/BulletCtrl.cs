using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // �Ѿ� ���ݷ�
    public float damage = 20f;
    // �Ѿ� �ӵ�
    public float speed = 1000f;
    

    // Start is called before the first frame update
    void Start()
    {
        // this Object�� �ڵ����� ����Ǵ� Component. AddForce�� ���� ���ϴ� ����.
        // RIgidbody�� rb�� ����ϰ� �����ϴ� �͵� �� �� ����.
        // �ٸ� Class���� ȣ���ϰų� ���� Class���� ȣ���ϰų� ���� �ʱ� ������ ������ �� �� �ִ�.
        // �߰����� ������ ���� ��� ������� �ʰ� �� �� �ִ�.
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
