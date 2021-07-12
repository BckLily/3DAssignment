using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect; // ���� ����Ʈ ������ ����
    int hitCount = 0; // �Ѿ� ���� Ƚ��
    Rigidbody rb;

    public Mesh[] meshes; // ����� ����ϴ� �޽�
    MeshFilter meshFilter; // �޽��� ������ �� �޽� ����

    public Texture[] textures; // �����⸦ ����ϴ� �ؽ���
    MeshRenderer _renderer; // �ؽ�ó�� �������� �޽� ������


    public float expRadius = 10f; // ���� �ݰ�

    AudioSource _audio;
    public AudioClip expSfx;

    public Shake shake;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        meshFilter = GetComponent<MeshFilter>();
        _renderer = GetComponent<MeshRenderer>();
        // �������� ����, mainTexture�� �����ϴµ�, textures�� ����ִ� �� ��
        // Random���� Range 0~textures�� ���� ��ŭ �߿��� �����´�.
        _renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];

        _audio = GetComponent<AudioSource>();

        shake = GameObject.Find("CameraRig").GetComponent<Shake>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("BULLET"))
        {
            hitCount++; // �Ѿ˰� �浹�� �߻����� �� �浹 Ƚ�� ����
            if (hitCount == 3)
            {
                // ���� ȿ�� �Լ� ȣ��
                ExpBarrel();
            }
        }
    }


    void ExpBarrel()
    {
        // ���� ���� �� ��Ī�� ������ ��� ������ ���� ���� ��Ȳ.
        // GameObject effect�� ������ expEffect�� ����.
        // ���� ������ �Ǵ� ���� effect��� ��ü(����) �̸��� �ο�����.
        // ���� effect��� ��ü���� ���ؼ� ���� ����.
        GameObject effect = Instantiate(expEffect, transform.position, Quaternion.identity); // ���� Rotation ����.
        Destroy(effect, 2f); // ���� ���� �ð��� 2�� �ο�. ���� ����Ʈ�� ����� 2�� �Ŀ� ���� ����Ʈ ����.
        //rb.mass = 1f; // �Ͻ������� Barrel�� ����(mass)�� ����.
        //rb.AddForce(Vector3.up * 500f);

        IndirectDamage(transform.position);

        // ��ϵ� �޽� �߿��� �ϳ��� �����ϱ� ���ؼ� ���� ���ڸ� ����.
        // Random.Range(0, meshes.Length) >> Random�� ���� �������µ�
        // Range������ 0�������� meshes�迭�� Length �� ���̿��� ��������� ��.
        int idx = Random.Range(0, meshes.Length);
        // ���� �ε������� �ش��ϴ� �޽��� ������ �޽����Ϳ� ����
        meshFilter.sharedMesh = meshes[idx];

        _audio.PlayOneShot(expSfx, 1f);
        StartCoroutine(shake.ShakeCamera(0.1f, 0.2f, 0.5f));
    }

    void IndirectDamage(Vector3 pos)
    {
        // ������ �� Object�� ������ ������ ���̱� ������ Collider�� []�� �ٿ� �迭ó��.
        // Physics���� ������ �ִ� OverlapSphere �������� ���� ������ ����.
        // OverlapSphere Method�� ������ ���� ���ؼ� �����ȿ� �ִ� ��� ������Ʈ�� ��� �����ؼ� ������ �´�.
        // pos: ���� ����. expRadius: ���� �ݰ�. 1 << 8: ������ �� ���̾�(8�� ���̾�)
        Collider[] colls = Physics.OverlapSphere(pos, expRadius, 1 << 8);

        
        // ����� ������Ʈ�� ������� �ϳ��� �����ϵ��� �Ѵ�.
        // 1�� �����ϴ� for���� ������ ����.
        // var >> �޾ƿ��� �Ű������� Ÿ���� �𸣱� ������ ��� Ÿ���� ���� �� �ִ� var Ÿ���� ����.
        foreach(var coll in colls)
        {
            var _rb = coll.GetComponent<Rigidbody>();
            _rb.mass = 1f;
            /*
             * var _bc = coll.GetComponent<BarrelCtrl>();
             * _bc.hitCount+=2;
             */
            // �������� ���߷��� �ƴ϶�
            // ���� �Ʒ��� ���߷��� �ֱ� ���ؼ� �����.
            // AddExplostionForce(Ⱦ(����) ���߷�, ���� ����, ���� �ݰ�, ��(����));
            _rb.AddExplosionForce(600f, pos, expRadius, 500f);
            _rb.mass = 20f;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

}
