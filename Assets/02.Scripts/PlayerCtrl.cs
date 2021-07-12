using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// 내부 클래스
// 클래스는 Insepctor View에 표시되지는 않는다.
// 직렬화: Inspector View에 보이게 하기 위해서 바로 꽂아버리는거(?)
// 클래스의 경우 변수와 달리 직렬화 해줘야 인스펙터에 표시됨.
// using System;을 추가하면 [Serializable]로 사용할 수 있다.
[System.Serializable]
public class PlayerAnim
{
    // Player Animtion Clip의 관리를 하기 위한 Class.
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
}

public class PlayerCtrl : MonoBehaviour
{
    // File name 과 Class name은 동일해야 한다.
    // private: 접근 지시자 (접근에 제한을 두는 것)
    // private은 작성된 스크립트 내부에서만 사용된다.
    // public은 내부 외부 어디에서나 쓸 수 있다.
    // 기본 형태는 private이다.
    // 추가적으로 Protected라는 접근 지시자가 있다. 클래스 본인(내부)와 상속된 클래스에서만 사용할 수 있다.
    // private >> 현재의 클래스(PlayerCtrl)의 밖에서 접근하는 것을 제한하는 지시자)
    // public  >> 현재의 클래스의 밖에서 접근하는 것이 가능한 지시자.
    // horizontal / vertical
    private float h = 0f; // float h = 0f;
    private float v = 0f; // float v = 0f;
    private float r = 0f;


    // Transform Component 접근 변수
    Transform tr;
    // public으로 선언된 변수는 Inspector View에 노출(표시)된다.
    public float moveSpeed = 10f;
    public float rotSpeed = 300f;

    public PlayerAnim playerAnim;
    public Animation anim;


    // Start is called before the first frame update
    void Start()
    {
        // Transform Component와 tr 변수 연결
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        anim.clip = playerAnim.idle;
        anim.Play();

        // 인스턴스화 되었다.
    }

    // Update is called once per frame
    void Update()
    {
        // GetAxis :: 가속이 있는 이동
        // GetAxisRaw :: 순간적으로 이동
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        //print("H값 -" + h + " V값 -" + v); // log Check

        // Vector 합을 사용해서 전후좌우 뿐 아니라 대각선 이동도 커버 가능.
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        // 벡터 정규화
        // 대각선 이동이 일반적인 이동보다 크기가 커서 빨리 움직인다.
        // 이를 해결하기 위해서 벡터의 크기를 일정하게 1로 정규화 시킨다.
        // 단위 벡터를 만드는 듯.
        moveDir = moveDir.normalized;

        // Vector3.forward: 정면
        // moveSpeed: 우리가 설정한 움직이는 속도
        // v: Input.GetAxis를 사용해서 입력받은 -1 ~ 1사이의 값.
        // Time.deltaTime: 동일한 프레임으로 동작할 수 있도록 설정하는데 사용하는 값.
        // The interval in seconds from the last frame to the current one (Read Only).
        // Space.Self >> 자기 자신(Object)를 기준으로 (x, y, z) 축을 설정. (Local 좌표)
        // Space.World>> Unity 맵 상을 기준으로 (x, y, z) 축을 설정. (Global 좌표)
        tr.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);

        // 회전의 기준이 되는 방향(y축) Vector3.up
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        //// Vector3.Magnitude >> () 안의 값의 크기만을 가져오는 것.
        //print(Vector3.Magnitude(Vector3.forward + Vector3.right));
        //print(Vector3.Magnitude((Vector3.forward + Vector3.right).normalized));


        // Animation 동작 구현
        if (v >= 0.1f) // 전진(Front)
        {
            // CrossFade(AnimationClip.name, 전환 시간);
            anim.CrossFade(playerAnim.runF.name, 0.3f);
        }
        else if (v <= -0.1f) // 후진(Back)
        {
            // CrossFade(AnimationClip.name, 전환 시간);
            anim.CrossFade(playerAnim.runB.name, 0.3f);
        }
        else if (h >= 0.1f) // 우측(Right)
        {
            // CrossFade(AnimationClip.name, 전환 시간);
            anim.CrossFade(playerAnim.runR.name, 0.3f);
        }
        else if (h <= -0.1f) // 좌측(Left)
        {   // CrossFade(AnimationClip.name, 전환 시간);
            anim.CrossFade(playerAnim.runL.name, 0.3f);
        }
        else // 입력이 없으면 idle 동작으로 변경된다.
        {
            anim.CrossFade(playerAnim.idle.name, 0.3f);
        }



    }
}
