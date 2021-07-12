using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect; // ����ũ ������

    private void OnCollisionEnter(Collision collision)
    {
        // �浹ü�� ���� ������ ����ִ� ��: collision
        // �浹�� �߻��� �͵� �߿��� TAG�� BULLET�� �͵鸸 ����
        if (collision.collider.tag == "BULLET")
        {
            // ����ũ ����Ʈ �Լ� ȣ��.
            // ����ũ�� �߻���Ű�� ���� �Լ�.
            ShowEffect(collision);

            // �浹�� �߻��� ������Ʈ ����
            Destroy(collision.gameObject);
            // �浹�� �߻��� �� ���ϴ� �ð��� ���� �� ����
            // Destroy(collision.gameObject, 5f);
            // �ڷ�� ���������� ȭ�鿡�� ������ �ʰ� ��Ȱ��ȭ �ϴ� ��.
            // collision.gameObject.SetActive(false);
        }

    }

    void ShowEffect(Collision coll)
    {
        // �浹 ������ ������ ������ �´�.
        // �浹 �� �߻��� ������ ��ġ ����.
        ContactPoint contact = coll.contacts[0];
        // FromToRotation >> ȸ����Ű���� �ϴ� ����, Ÿ�� ���͸� ȸ����Ŵ.
        // contact.normal >> ���� ����(�鿡 ������ ����)
        // -Vector3.forward >> �浹�� ���� �ݴ��� �������� ����� �߻��� ����ڰ� Effect�� �� �� �����Ƿ� ȸ����Ų��.
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);

        // �Ѿ��� �߻�� ��ġ�� ����Ʈ�� ���� �̵�. (�巳�뿡�� ���� ����� �Ѿ� �ڱ� ����)
        Vector3 point = contact.point + (-contact.normal * 0.05f);
        // �浹�� �� �� ����Ʈ�� ȿ�� ����(Z)�� ���� ����(�Ѿ��� ���ƿ� ���� (-Z))���� ������ ����.
        GameObject spark = Instantiate(sparkEffect, point, rot);

        // spark�� �߻��ϰ� �� ��, Object�� Parent�� this Object�� �����Ͽ�,
        // this Object�� �̵��� �ϰ� �� ��� Object�� ������ ��ġ���� ���� �̵��ϰ� �������.
        spark.transform.SetParent(this.transform);

        
    }

}
