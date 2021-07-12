using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// NavMeshAgnet�� ���� �� ���â�� ����ִ� ��.
[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    // List�� Array(�迭)�� ����. �����ϴ�.
    // ������ - ���� ���̷μ� ���빰�� ���� ���̰� ���Ѵ�.
    public List<Transform> wayPoints;
    public int nextIndex; // ���� ���� ������ �迭 �ε���


    NavMeshAgent agent;

    float damping = 1f; // ȸ�� �ӵ� �����ϴ� ���
    Transform enemyTr;


    // ������Ƽ �ۼ�
    // ������Ƽ�� �Լ��ε� ����ó�� ���̴� ��.
    private readonly float patrolSpeed = 1.5f; // �б� ���� ���� �ӵ� ����
    private readonly float traceSpeed = 4f; // �б� ���� ���� �ӵ� ����

    bool _patrolling; // ���� ���� �Ǵ� ����
    public bool patrolling
    {
        get
        {
            return _patrolling;
        }

        set
        {
            // set ���۽� ���޹��� ���� value�� ����.
            // value�� �ִ� ���� _patrolling ������ �������ش�.
            // _patrolling = value * 3 + 2; ��� ���� ������� ���� �����Ϳ� ���̴� �����͸� �ٸ��� �ϴ� ������ ������ �� �ִ�.
            _patrolling = value;
            if (_patrolling)
            {
                // �ٸ� �����̿��ٰ� patrol�� ����� �̵� �ӵ��� patrolSpee�� �����ϰ�
                // MoveWayPoint�� �����Ͽ� patrol�ϰ� �Ѵ�.
                damping = 1f;
                agent.speed = patrolSpeed;
                MoveWayPoint();
            }
        }
    }

    Vector3 _traceTarget; // ���� ��� ����
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7f;
            // ���� ��� ���� �Լ� ȣ��.
            TraceTarget(_traceTarget);

        }
    }

    public float speed // agent�� �̵��ӵ��� �������� ������Ƽ
    {
        // agent�� �̵��ӵ��� "ũ��" �� ������.
        // get�� �����ϹǷ� ���� ������ ���� ���ϰ� ���� ������ �� �ִ�.
        get { return agent.velocity.magnitude; }
    }



    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        // autoBraking�� ��ǥ ��� ��ó�� ���� �ӵ��� ���缭 ���ߴ� ��.
        // �극��ũ�� ���� �ڵ� �������� �ʵ��� �Ѵ�.
        // �������� ����������� �ӵ��� ���̴� �ɼ�
        agent.autoBraking = false;
        agent.speed = patrolSpeed;
        // �ڵ����� ȸ���ϴ� ��� ��Ȱ��ȭ
        // ���� ȸ���� �����ϰڴ�.
        agent.updateRotation = false;

        enemyTr = GetComponent<Transform>();

        // Hierarchy View���� "������Ʈ �̸�"���ε� ������Ʈ�� �˻�.
        var group = GameObject.Find("WayPointGroup");
        // group�� null�� �ƴ� ���.
        // Hierarchy View�� WayPointGroup�� ������ ���
        // if(group)
        if (group != null)
        {
            Debug.Log("group is not null");

            // WayPointGroup ������ �ִ� ��� Point�� Transform�� ������
            // wayPoints��� List������ �����Ѵ�.
            // WayPointGroup ������ �ִ� ���
            // Transform ������Ʈ�� ������ͼ�
            // wayPoints ������ �־��ش�.
            // GetComponentsInChildren�� �ϸ� �ڱ� �ڽ�(WayPointGroup)�� ���� List�� �߰��ȴ�.
            group.GetComponentsInChildren<Transform>(wayPoints);

            // ����Ʈ�� �� �ִ� ��ҵ� �߿���
            // ������ �ε����� ������Ʈ ����.
            wayPoints.RemoveAt(0);

            // Hierarchy���� ������ Point���� ������ �߿���
            // ������ ��ġ�� �ϳ� �̾� �´�.
            nextIndex = Random.Range(0, wayPoints.Count);
        }
        else
        {
            Debug.LogError("Group is null");
        }

        // ���� ����Ʈ �����ϴ� �Լ� ȣ��
        MoveWayPoint();

    }

    void MoveWayPoint()
    {
        // isPathStale ��� ��� ���� ���� true ������ false ��ȯ�Ѵ�.
        // �Ÿ� ��� ���� ���� ���� ��� ������ ���� �ʵ��� �ϱ� ���ؼ��̴�.
        if (agent.isPathStale)
            return;

        // ������ Point�� �߿��� �� ������ �������� ����.
        agent.destination = wayPoints[nextIndex].position;
        // �׺���̼� ��� Ȱ��ȭ�ؼ� �̵� �����ϵ��� ����.
        agent.isStopped = false;
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale)
            return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop() // �ܺο��� ȣ���Ͽ� ����ϴ� �Լ�.
    {
        agent.isStopped = true;
        // �ٷ� �����ϱ� ���Ͽ� �ܿ� �ӵ��� 0���� �ʱ�ȭ.
        agent.velocity = Vector3.zero;

        _patrolling = false;

    }



    // Update is called once per frame
    void Update()
    {
        if (!agent.isStopped) // ���� �����̴� ���� ��
        {
            // ���� �����ؾߵ� ���� ���͸� ���ؼ� ȸ�� ������ ���.
            // LookRotation >> �����ϴ� ���� ����.
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            // ���� �Լ��� ����ؼ� ���������� ȸ��.(õõ�� ���� / �ε巴��)
            // Slerp(���� ����, ���� ����, ���� ��)
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }


        // ���� ��尡 �ƴ� �� ������ ���õ� ������ �� �ʿ䰡 �����Ƿ�
        // Update �Լ��� ������ ����������.
        if (!_patrolling) // if(!patrolling) // ������Ƽ�� ���.
            return;

        // �������� �����ߴ��� �Ǵ��ϱ� ���� ����.
        // �ӵ��� 0.2���� ũ��(sqr>> ������) ���� �̵��Ÿ��� 0.5������ ���
        // agent.velocity.Magnitude >= 0.2f
        // >> �������� ���� �������� ��
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f && agent.remainingDistance <= 0.5f)
        {
            //// ��ȯ ������ �̷�� ���ؼ� ������ ������ �Ѵ�.
            //nextIndex++;
            //// ó������ ������ �������� ������ �ٽ� ó������ ���ư����� �ε��� �����Ѵ�.
            //nextIndex = nextIndex % wayPoints.Count;
            //// �ε��� ���� �� �̵� �����ϱ� ���� �Լ� ȣ��.
            // �� �ڵ�� ���� ������ ���������� ��ȯ�ϵ��� �����Ƿ� �ּ�

            nextIndex = Random.Range(0, wayPoints.Count);
            

            MoveWayPoint();

        }
    }
}
