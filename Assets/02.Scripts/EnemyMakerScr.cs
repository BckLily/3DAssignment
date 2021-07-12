using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMakerScr : MonoBehaviour
{
    public GameObject enemy; // �� ���� ������Ʈ / �����͸� ���ؼ� ������ ���̴�.
    int makeMax; // �ִ� ���� Enemy. 10 �� �����ϰ� �������� �ʴ´�. / �׷��� �ڷ�ƾ���� ������.
    int makeCount; // ������ �� ī��Ʈ
    float pointSize; // ���� ���� �κ����� ������ �� ����
    float makeDelay; // Enemy ���� ������
    

    // Start is called before the first frame update
    void Start()
    {
        makeMax = 10;
        pointSize = 50 / 4f;
        makeDelay = 5f;
        makeCount = 0;

        StartCoroutine(MakeEnemy());
    }

    IEnumerator MakeEnemy()
    {
        while (makeCount < makeMax)
        {
            // �� 10�� �����ϰ� �����.
            // ���� ������ 5�� ���� ����
            yield return new WaitForSeconds(makeDelay);
            // ���� ��ġ�� �� ��ü �߿��� �������� �����ȴ�.
            Vector3 pos = new Vector3(Random.Range(-2, 2) * pointSize + Random.Range(0f, 2.5f), 0f, Random.Range(-2, 2) * pointSize + Random.Range(0f, 2.5f));
            // ���� �����Ѵ�.
            Instantiate(enemy, pos, Quaternion.identity);
            // ������ ���� �÷��� ���� Ƚ���� ���� �ʵ��� �Ѵ�.
            makeCount += 1;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
