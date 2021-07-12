using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// NavMeshAgnet가 없을 때 경고창을 띄워주는 것.
[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    // List도 Array(배열)과 유사. 동일하다.
    // 차이점 - 가변 길이로서 내용물에 따라 길이가 변한다.
    public List<Transform> wayPoints;
    public int nextIndex; // 다음 순찰 지점의 배열 인덱스


    NavMeshAgent agent;

    float damping = 1f; // 회전 속도 조절하는 계수
    Transform enemyTr;


    // 프로퍼티 작성
    // 프로퍼티란 함수인데 변수처럼 쓰이는 것.
    private readonly float patrolSpeed = 1.5f; // 읽기 전용 순찰 속도 변수
    private readonly float traceSpeed = 4f; // 읽기 전용 추적 속도 변수

    bool _patrolling; // 순찰 여부 판단 변수
    public bool patrolling
    {
        get
        {
            return _patrolling;
        }

        set
        {
            // set 동작시 전달받은 값은 value에 들어간다.
            // value에 있는 값을 _patrolling 변수에 전달해준다.
            // _patrolling = value * 3 + 2; 등과 같은 방식으로 저장 데이터와 보이는 데이터를 다르게 하는 식으로 은닉할 수 있다.
            _patrolling = value;
            if (_patrolling)
            {
                // 다른 동작이였다가 patrol로 변경시 이동 속도를 patrolSpee로 변경하고
                // MoveWayPoint를 설정하여 patrol하게 한다.
                damping = 1f;
                agent.speed = patrolSpeed;
                MoveWayPoint();
            }
        }
    }

    Vector3 _traceTarget; // 추적 대상 지정
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7f;
            // 추적 대상 지정 함수 호출.
            TraceTarget(_traceTarget);

        }
    }

    public float speed // agent의 이동속도를 가져오는 프로퍼티
    {
        // agent의 이동속도의 "크기" 값 가져옴.
        // get만 존재하므로 따로 설정을 하지 못하고 값만 가져올 수 있다.
        get { return agent.velocity.magnitude; }
    }



    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        // autoBraking은 목표 장소 근처에 오면 속도를 늦춰서 멈추는 것.
        // 브레이크를 꺼서 자동 감속하지 않도록 한다.
        // 목적지에 가까워질수록 속도를 줄이는 옵션
        agent.autoBraking = false;
        agent.speed = patrolSpeed;
        // 자동으로 회전하는 기능 비활성화
        // 직접 회전을 조절하겠다.
        agent.updateRotation = false;

        enemyTr = GetComponent<Transform>();

        // Hierarchy View에서 "오브젝트 이름"으로된 오브젝트를 검색.
        var group = GameObject.Find("WayPointGroup");
        // group이 null이 아닐 경우.
        // Hierarchy View에 WayPointGroup이 존재할 경우
        // if(group)
        if (group != null)
        {
            Debug.Log("group is not null");

            // WayPointGroup 하위에 있는 모든 Point의 Transform의 정보를
            // wayPoints라는 List변수에 대입한다.
            // WayPointGroup 하위에 있는 모든
            // Transform 컴포넌트를 가지고와서
            // wayPoints 변수에 넣어준다.
            // GetComponentsInChildren을 하면 자기 자신(WayPointGroup)도 같이 List에 추가된다.
            group.GetComponentsInChildren<Transform>(wayPoints);

            // 리스트에 들어가 있는 요소들 중에서
            // 지정된 인덱스의 오브젝트 삭제.
            wayPoints.RemoveAt(0);

            // Hierarchy에서 생성한 Point들의 개수들 중에서
            // 랜덤한 위치를 하나 뽑아 온다.
            nextIndex = Random.Range(0, wayPoints.Count);
        }
        else
        {
            Debug.LogError("Group is null");
        }

        // 웨이 포인트 변경하는 함수 호출
        MoveWayPoint();

    }

    void MoveWayPoint()
    {
        // isPathStale 경로 계산 중일 때는 true 끝나면 false 반환한다.
        // 거리 계산 중일 때는 순찰 경로 변경을 하지 않도록 하기 위해서이다.
        if (agent.isPathStale)
            return;

        // 만들어둔 Point들 중에서 한 곳으로 목적지를 설정.
        agent.destination = wayPoints[nextIndex].position;
        // 네비게이션 기능 활성화해서 이동 시작하도록 변경.
        agent.isStopped = false;
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale)
            return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop() // 외부에서 호출하여 사용하는 함수.
    {
        agent.isStopped = true;
        // 바로 정지하기 위하여 잔여 속도를 0으로 초기화.
        agent.velocity = Vector3.zero;

        _patrolling = false;

    }



    // Update is called once per frame
    void Update()
    {
        if (!agent.isStopped) // 적이 움직이는 중일 때
        {
            // 적이 진행해야될 방향 벡터를 통해서 회전 각도를 계산.
            // LookRotation >> 가야하는 방향 지정.
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            // 보간 함수를 사용해서 점진적으로 회전.(천천히 돌림 / 부드럽게)
            // Slerp(현재 방향, 향할 방향, 보간 값)
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }


        // 순찰 모드가 아닐 때 순찰에 관련된 동작을 할 필요가 없으므로
        // Update 함수를 빠르게 끝내버린다.
        if (!_patrolling) // if(!patrolling) // 프로퍼티를 사용.
            return;

        // 목적지에 도착했는지 판단하기 위한 조건.
        // 속도가 0.2보다 크고(sqr>> 제곱근) 남은 이동거리가 0.5이하인 경우
        // agent.velocity.Magnitude >= 0.2f
        // >> 목적지에 거의 도착했을 때
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.5f)
        {
            //// 순환 구조를 이루기 위해서 나머지 연산을 한다.
            //nextIndex++;
            //// 처음부터 마지막 순찰지를 돌고나면 다시 처음으로 돌아가도록 인덱스 변경한다.
            //nextIndex = nextIndex % wayPoints.Count;
            //// 인덱스 변경 후 이동 시작하기 위해 함수 호출.
            // 위 코드는 순찰 지점을 순차적으로 순환하도록 헀으므로 주석

            nextIndex = Random.Range(0, wayPoints.Count);
            

            MoveWayPoint();

        }
    }
}
