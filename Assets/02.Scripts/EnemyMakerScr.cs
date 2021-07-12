using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMakerScr : MonoBehaviour
{
    public GameObject enemy; // 적 게임 오브젝트 / 에디터를 통해서 가져올 것이다.
    int makeMax; // 최대 생성 Enemy. 10 명만 생성하고 생성하지 않는다. / 그러면 코루틴으로 돌리자.
    int makeCount; // 생성한 적 카운트
    float pointSize; // 맵을 여러 부분으로 나눴을 때 길이
    float makeDelay; // Enemy 생성 딜레이
    

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
            // 적 10명 생성하고 멈춘다.
            // 생성 딜레이 5초 마다 생성
            yield return new WaitForSeconds(makeDelay);
            // 생성 위치는 맵 전체 중에서 랜덤으로 결정된다.
            Vector3 pos = new Vector3(Random.Range(-2, 2) * pointSize + Random.Range(0f, 2.5f), 0f, Random.Range(-2, 2) * pointSize + Random.Range(0f, 2.5f));
            // 적을 생성한다.
            Instantiate(enemy, pos, Quaternion.identity);
            // 생성한 수를 늘려서 생성 횟수를 넘지 않도록 한다.
            makeCount += 1;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
