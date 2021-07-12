using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    const string bulletTag = "BULLET";
    float initHp = 100f; // 초기 체력
    public float currHP; // 현재 체력


    // 델리게이트 선언
    public delegate void PlayerDieHandler();
    // 델리게이트를 활용한 이벤트 선언
    // 프로젝트 내에서 어디서든 사용할 수 있게된다.(static)
    public static event PlayerDieHandler OnPlayerDieEvent;

    // Start is called before the first frame update
    void Start()
    {
        currHP = initHp;
    }

    // 충돌이 아닌 관통일 경우에 사용하는 함수
    // 충돌: OnCollisionEnter(Collsion)
    // 관통: OnTriggerEnter(Collider)
    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 물체의 태그 비교
        if (other.CompareTag(bulletTag))
        {
            Destroy(other.gameObject); // 충돌한 물체가 BULLET 이면 제거
            currHP -= 5f; // 체력 5감소
            Debug.Log("현재 체력 - " + currHP);

            if (currHP <= 0f)
            {
                // 플레이어 사망 함수 호출
                PlayerDie();
            }
        }
    }


    void PlayerDie()
    {
        //Debug.Log("플레이어 사망");

        //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");

        //for(int i = 0; i < enemies.Length; i++)
        //{
        //    // 함수 호출 방법
        //    // 직접 호출 하는 방법
        //    // enemies[i].GetComponent<EnemyAI>().OnPlayerDie();
        //    // SendMessage를 사용해서 호출하는 방법
        //    // DontRequireReceiver >> 호출에 대한 응답 X
        //    // RequireReceiver >> 호출에 대한 응답 O
        //    // 함수 이름이 같은 경우 먼저 만나는 함수를 호출하게 된다.
        //    enemies[i].SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //    // 델리게이트 호출 방법.
        //}

        // ======delegate====

        OnPlayerDieEvent();

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
