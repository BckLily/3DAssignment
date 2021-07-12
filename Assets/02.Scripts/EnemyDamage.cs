using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // ������ ���(������ ������) �����ϴ� ��.
    const string bulletTag = "BULLET";

    float hp = 100f; // ü��
    GameObject bloodEffect; // ���� ȿ��




    // Start is called before the first frame update
    void Start()
    {
        // Load �Լ��� ���� ������ Resources����
        // �����͸� �ҷ����� �Լ��̴�.
        // Load<������ ����>("������ ���");
        // �ֻ��� ��δ� Resources ����.
        // ���� Blood ������ Resources�� �ٷ� �ȿ� �ֱ� ������ Blood�� �����ָ� �ȴ�.
        // ���� ���� ������ �����ؼ� �� �����ȿ� ���ϴ� ������ ���� ���
        // "Prefabs/Blood"�� ���� ������� �����Ѵ�.
        // ������ ��δ� ���� ������ + ���ϸ���� ��Ȯ�ϰ� ��θ� ���.
        bloodEffect = Resources.Load<GameObject>("Blood");

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(bulletTag))
        {
            // ���� ȿ�� �Լ� ȣ��
            ShowBloodEffect(collision);
            // �Ѿ� ����
            Destroy(collision.gameObject);
            // �������� �ִ� ��ü�� �������� Ư�� ���� ���� �߰����ָ�
            // �������� �޴� ��ü���� � ��ü�� �����Ͽ����� Ȯ���ϱ⸸ �ϸ�
            // ��� �������� �޾Ҵ��� Ȯ���� �� �� �ִ�.
            hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;


            // �Ǽ� ���� ��� 1�� �ƴ� 0.999 ���� ǥ�õ� �� �ֱ� ������ 
            // ��Ȯ�� ���ƾ��ϴ� ==�� �ƴ� <=�� ����Ѵ�.
            // ü���� 0������ ��� ���� �׾��ٰ� �Ǵ�.
            if (hp <= 0)
            {
                GetComponent<EnemyAI>().state = EnemyAI.State.DIE;
            }
        }
    }

    void ShowBloodEffect(Collision coll)
    {
        // �浹 ��ġ �� ��������
        Vector3 pos = coll.contacts[0].point;
        // �浹 ��ġ�� ���� ����(�Ѿ��� ���ƿ� ����)
        Vector3 _normal = coll.contacts[0].normal;
        // �Ѿ��� ���ƿ� ���Ⱚ ���.
        Quaternion rot = Quaternion.FromToRotation(Vector3.back, _normal);
        // ���� ȿ�� ���� ����
        //GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot); ������ ����
        GameObject blood = Instantiate(bloodEffect, pos, rot);
        // 1���� ����
        Destroy(blood, 1f);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
