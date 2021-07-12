using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // ī�޶� ������ ���
    public Transform target;
    // ���� ����� �̵��Ͽ��� �� ī�޶� �ε巴�� ���󰡱� ���ؼ� ����ϴ� ������.
    // �̵� �ӵ� ���
    public float moveDamping = 15f;
    // ȸ�� �ӵ� ���
    public float rotateDamping = 10f;
    // ���� ������ �Ÿ�
    public float distance = 5f;
    // ���� ������ ����
    public float height = 4f;
    // Player ���� ���̰� 2���� �� ���̶� ������Ʈ ������ �Ǵ� �ٴ��� �ƴ� �Ӹ����� �� �� �ְ� ���̸� ��.
    // ���� ��ǥ�� ������
    public float targetOffset = 2f;

    private Transform tr;

    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // �ݹ��Լ� - ȣ���� ���� ���� �ʾƵ� �˾Ƽ� �۵��ϴ� �Լ�
    // �̺�Ʈ Ʈ���� �� �������� ������ �ִ�.
    // Update is called once per frame
    void LateUpdate()
    {
        var camPos = target.position - (target.forward * distance) + (target.up * height);

        // ������ ��ġ�� ���������� ��ġ�� ���� ����(�ϸ��ϰ� ó��)�� �� ī�޶��� �̵��� �ε巴�� �Ѵ�.
        // Slerp: Spherically interpolates between two vectors.
        tr.position = Vector3.Slerp(tr.position, camPos, Time.deltaTime * moveDamping);
        // Quaternion >> ���ʹϾ� ��. �Ϲ������� ����ϴ� x, y ,z �� w��� ���ο� ���� ����ؼ�
        // ���Ϸ� ������ ���Ͱ��� �浹 �� �߻��� �� �ִ� ������ ���ּ� ����ϴ� ����.
        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, Time.deltaTime * rotateDamping);
        // ������ �߹ٴ��� �Ĵٺ��� ī�޶� �����¸�ŭ ������ ������ ����.
        tr.LookAt(target.position + (target.up * targetOffset));
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        // DrawWireSphere(��ġ, ����)
        // ������ �̷���� ������ ����� �׸�(���信�� ǥ�õȴ�. ����� �뵵)
        // Ÿ�� ����(Player�� �Ӹ� �κ�)�� ����.
        Gizmos.DrawWireSphere(target.position + target.up * targetOffset, 0.1f);

        // ���� ī�޶�� ���� ���� ���̿� ���� �׸���.
        // DrawLine(��� ����, ���� ����)
        // ��߰� ���� ���� ���̿� ���� �׸���.
        Gizmos.DrawLine(target.position + (target.up * targetOffset), transform.position);
    }
}
