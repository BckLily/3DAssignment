using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    const string bulletTag = "BULLET";
    float initHp = 100f; // �ʱ� ü��
    public float currHP; // ���� ü��


    // ��������Ʈ ����
    public delegate void PlayerDieHandler();
    // ��������Ʈ�� Ȱ���� �̺�Ʈ ����
    // ������Ʈ ������ ��𼭵� ����� �� �ְԵȴ�.(static)
    public static event PlayerDieHandler OnPlayerDieEvent;

    // Start is called before the first frame update
    void Start()
    {
        currHP = initHp;
    }

    // �浹�� �ƴ� ������ ��쿡 ����ϴ� �Լ�
    // �浹: OnCollisionEnter(Collsion)
    // ����: OnTriggerEnter(Collider)
    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� �±� ��
        if (other.CompareTag(bulletTag))
        {
            Destroy(other.gameObject); // �浹�� ��ü�� BULLET �̸� ����
            currHP -= 5f; // ü�� 5����
            Debug.Log("���� ü�� - " + currHP);

            if (currHP <= 0f)
            {
                // �÷��̾� ��� �Լ� ȣ��
                PlayerDie();
            }
        }
    }


    void PlayerDie()
    {
        //Debug.Log("�÷��̾� ���");

        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");

        //for(int i = 0; i < enemies.Length; i++)
        //{
        //    // �Լ� ȣ�� ���
        //    // ���� ȣ�� �ϴ� ���
        //    // enemies[i].GetComponent<EnemyAI>().OnPlayerDie();
        //    // SendMessage�� ����ؼ� ȣ���ϴ� ���
        //    // DontRequireReceiver >> ȣ�⿡ ���� ���� X
        //    // RequireReceiver >> ȣ�⿡ ���� ���� O
        //    // �Լ� �̸��� ���� ��� ���� ������ �Լ��� ȣ���ϰ� �ȴ�.
        //    enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //    // ��������Ʈ ȣ�� ���.
        //}

        // ======delegate====

        OnPlayerDieEvent();

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
