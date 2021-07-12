using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        // 순찰, 추적, 공격, 사망
        PATROL, TRACE, ATTACK, DIE,
    }

    public State state = State.PATROL; // 초기 상태 지정

    // 추적해야 하는 대상의 위치를 저장하는 변수
    Transform playerTr; // 플레이어 위치 저장 변수
    Transform enemyTr; // 적 캐릭터의 위치 저장 변수

    public float attackDist = 5f; // 공격 사거리
    public float traceDist = 10f; // 추적 사거리
    public bool isDie = false; // 사망 여부 판단 변수

    WaitForSeconds ws; // 시간 지연 변수

    MoveAgent moveAgent; // moveAgent 컴포넌트 제어 변수.
    EnemyFire enemyFire; // enemyFire 컴포넌트 제어 변수


    Animator animator;
    // 변수에 Animator의 Parameter를 미리 저장해두어서 사용.
    // Parameter의 대소문자를 구분하여야 한다.
    readonly int hashMove = Animator.StringToHash("IsMove");
    readonly int hashSpeed = Animator.StringToHash("Speed");
    readonly int hashDieIdx = Animator.StringToHash("DieIndex");
    readonly int hashDie = Animator.StringToHash("Die");
    readonly int hashOffset = Animator.StringToHash("Offset");
    readonly int hashWalkSpeed = Animator.StringToHash("WalkSpeed");
    readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");


    
    void Awake()
    {
        // Awake는 Script 첫 실행시에만 동작
        // Start는 Object 생성시에 동작
        // FindGameObjectWithTag를 통해서 Hierarchy View에 있는 PLAYER Tag를 가진 GameObject를 찾음.
        var player = GameObject.FindGameObjectWithTag("PLAYER");

        if (player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }

        enemyTr = GetComponent<Transform>();
        moveAgent = GetComponent<MoveAgent>();
        animator = GetComponent<Animator>();
        enemyFire = GetComponent<EnemyFire>();


        // 시간 지연 변수의 값을 0.3으로 설정.
        // 시간 지연 변수는 코루틴 함수에서 사용된다.
        ws = new WaitForSeconds(0.3f);


        // Offset과 WalkSpeed값을 이용해 애니메이션 동작을 다양하게 구성.
        // 속도는 조금씩 다르게 만들어준다.
        animator.SetFloat(hashOffset, Random.Range(0f, 1f));
        animator.SetFloat(hashWalkSpeed, Random.Range(1f, 1.2f));

    }

    private void OnEnable()
    {
        // OnEable은 해당 스크립트가 활성화 될 때마다 실행된다.
        // 상태 체크하는 코루틴 함수 호출.
        StartCoroutine(CheckState());
        // 상태 변화에 따라 행동을 지시하는 코루틴 함수 호출.
        StartCoroutine(Action());

        // Damage 스크립트의 OnPlayerDieEvent 이벤트에
        // EnemyAI 스크립트의 OnPlayerDie 함수를 연결시킨다.
        // Damage 스크립트의 OnPlayerDieEvent가 활성화되면
        // EnemyAI 스크립트의 OnPlayerDie 함수가 실행된다.
        Damage.OnPlayerDieEvent += this.OnPlayerDie;

    }


    private void OnDisable()
    {
        // 스크립트가 비활성화 될 때에는
        // 이벤트와 연결된 함수의 연결을 해제해주는 것을 원칙으로 한다.
        Damage.OnPlayerDieEvent -= this.OnPlayerDie;
    }


    IEnumerator CheckState() // 상태 체크 코루틴 함수
    {
        while (!isDie) // 적이 살아있는 동안 계속 실행되도록 while 사용.
        {
            if (state == State.DIE)
                yield break; // 코루틴 함수 정지시킨다. 안전장치

            // Distance(A 위치, B 위치) >> A와 B사이읭 거리를 계산해주는 함수.
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if (dist <= attackDist) // 공격 사거리 이내면 공격으로 변경
            {
                state = State.ATTACK;
            }
            else if (dist <= traceDist) // 추적 사거리 이내면 추적으로 변경
            {
                state = State.TRACE;
            }
            else // 공격도 추적도 아니면 순찰 상태로 변경.
            {
                state = State.PATROL;
            }

            yield return ws; // 위에서 설정한 지연시간 0.3초 대기.
        }
    }

    
    
    IEnumerator Action() // 상태에 따른 동작 설정
    {
        while (!isDie)
        {
            // 상태가 변경된 다음 동작에 들어갈 수 있게 약간의 대기시간을 둠.
            yield return ws;


            switch (state)
            {
                case State.PATROL:
                    enemyFire.isFire = false;
                    moveAgent.patrolling = true;
                    // animator에 설정된 Parameter의 Bool, Trigger 등등에 따라 맞춰서
                    animator.SetBool(hashMove, true);

                    break;
                case State.TRACE:
                    enemyFire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);

                    break;
                case State.ATTACK:
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    if(enemyFire.isFire==false)
                        enemyFire.isFire = true;

                    break;
                case State.DIE:
                    isDie = true;
                    enemyFire.isFire = false;
                    
                    moveAgent.Stop();

                    // 랜덤 값에 의해서 애니메이션 3개 중 하나를 랜덤하게 실행한다.
                    animator.SetInteger(hashDieIdx, Random.Range(0, 3));
                    animator.SetTrigger(hashDie);
                    // 사망 후 남아있는 콜라이더 비활성화해서 충돌이 발생하지 않도록 한다.
                    GetComponent<CapsuleCollider>().enabled = false;
                    OnDisable();

                    break;
                default:


                    break;
            }

        }

    }



    // Update is called once per frame
    void Update()
    {
        // Blend Tree의 경우 Float Parameter.
        // 애니메이터 변수의 Set 함수들의 종류는 여러가지 있다.
        // SetFloat 등 해당 함수는 (해쉬 값 / 파라미터 이름, 전달하고자 하는 값) 형태로 사용된다.
        animator.SetFloat(hashSpeed, moveAgent.speed);
    }

    public void OnPlayerDie()
    {
        moveAgent.Stop();
        enemyFire.isFire = false;
        StopAllCoroutines(); // 모든 코루틴 함수 종료.

        animator.SetTrigger(hashPlayerDie);
    }

}
