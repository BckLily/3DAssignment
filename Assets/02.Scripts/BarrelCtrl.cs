using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect; // 폭발 이펙트 프리팹 변수
    int hitCount = 0; // 총알 맞은 횟수
    Rigidbody rb;

    public Mesh[] meshes; // 모양을 담당하는 메쉬
    MeshFilter meshFilter; // 메쉬를 적용해 줄 메쉬 필터

    public Texture[] textures; // 껍데기를 담당하는 텍스쳐
    MeshRenderer _renderer; // 텍스처를 적용해줄 메쉬 렌더러


    public float expRadius = 10f; // 폭발 반경

    AudioSource _audio;
    public AudioClip expSfx;

    public Shake shake;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshFilter = GetComponent<MeshFilter>();
        _renderer = GetComponent<MeshRenderer>();
        // 렌더러의 재질, mainTexture를 변경하는데, textures에 들어있는 것 중
        // Random으로 Range 0~textures의 길이 만큼 중에서 가져온다.
        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];

        _audio = GetComponent<AudioSource>();

        shake = GameObject.Find("CameraRig").GetComponent<Shake>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            hitCount++; // 총알과 충돌이 발생했을 때 충돌 횟수 증가
            if (hitCount == 3)
            {
                // 폭발 효과 함수 호출
                ExpBarrel();
            }
        }
    }


    void ExpBarrel()
    {
        // 폭발 생성 후 지칭할 변수가 없어서 제어할 수가 없는 상황.
        // GameObject effect에 생성된 expEffect를 넣음.
        // 동적 생성이 되는 순간 effect라는 객체(변수) 이름을 부여해줌.
        // 이후 effect라는 객체명을 통해서 제어 가능.
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity); // 고유 Rotation 유지.
        Destroy(effect, 2f); // 삭제 지연 시간을 2초 부여. 폭발 이펙트가 생기고 2초 후에 폭발 이펙트 삭제.
        //rb.mass = 1f; // 일시적으로 Barrel의 무게(mass)를 줄임.
        //rb.AddForce(Vector3.up * 500f);

        IndirectDamage(transform.position);

        // 등록된 메쉬 중에서 하나를 선택하기 위해서 랜덤 숫자를 뽑음.
        // Random.Range(0, meshes.Length) >> Random한 값을 가져오는데
        // Range설정을 0에서부터 meshes배열의 Length 의 사이에서 가져오라는 뜻.
        int idx = Random.Range(0, meshes.Length);
        // 뽑은 인덱스에서 해당하는 메쉬를 선택해 메쉬필터에 적용
        meshFilter.sharedMesh = meshes[idx];

        _audio.PlayOneShot(expSfx, 1f);
        StartCoroutine(shake.ShakeCamera(0.1f, 0.2f, 0.5f));
    }

    void IndirectDamage(Vector3 pos)
    {
        // 영향을 줄 Object를 여러개 가져올 것이기 때문에 Collider에 []를 붙여 배열처리.
        // Physics물리 영향을 주는 OverlapSphere 겹쳐지는 구형 범위를 측정.
        // OverlapSphere Method는 지정된 값에 의해서 범위안에 있는 대상 오브젝트를 모두 검출해서 가지고 온다.
        // pos: 폭발 원점. expRadius: 폭발 반경. 1 << 8: 영향을 줄 레이어(8번 레이어)
        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << 8);

        
        // 검출된 오브젝트를 순서대로 하나씩 선택하도록 한다.
        // 1씩 증가하는 for문과 동일한 역할.
        // var >> 받아오는 매개변수의 타입을 모르기 때문에 모든 타입을 받을 수 있는 var 타입을 받음.
        foreach(var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1f;
            /*
             * var _bc = coll.GetComponent<BarrelCtrl>();
             * _bc.hitCount+=2;
             */
            // 직선적인 폭발력이 아니라
            // 위로 아래로 폭발력을 주기 위해서 사용함.
            // AddExplostionForce(횡(가로) 폭발력, 폭발 원점, 폭발 반경, 종(세로));
            _rb.AddExplosionForce(600f, pos, expRadius, 500f);
            _rb.mass = 20f;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

}
