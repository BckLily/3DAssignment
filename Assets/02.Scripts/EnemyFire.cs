using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    AudioSource _audio;
    Animator animator;
    Transform playerTr;
    Transform enemyTr;

    readonly int hashFire = Animator.StringToHash("Fire");
    readonly int hashReload = Animator.StringToHash("Reload");

    // ���� ���� ����
    float nextFire = 0f;
    readonly float fireRate = 0.1f; // (�ּ�)�߻� ����
    readonly float damping = 10f; // ȸ�� �ӵ� ���
    
    
    public bool isFire = false; // �Ѿ� �߻� ���� �Ǵ�.
    public AudioClip fireSfx; // �Ѿ� �߻� ����


    // ������ ����
    readonly float reloadTime = 2f; // ������ �ð�
    readonly int maxBullet = 10; // źâ �ִ� �Ѿ� ��
    int currBullet = 10; // ���� �Ѿ� ��
    bool isReload; // ������ ����
    WaitForSeconds wsReload; // ������ ���� �ð� ����.
    public AudioClip reloadSfx;

    public GameObject Bullet;
    public Transform firePos;

    public MeshRenderer muzzleFlash;


    // Start is called before the first frame update
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<Transform>();
        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();

        wsReload = new WaitForSeconds(reloadTime);

        // ���� ������ �� ���� �÷��� ��Ȱ��ȭ
        // ���߿� �Ѿ� �߻� �ÿ��� Ȱ��ȭ�ؼ� ���.
        muzzleFlash.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ��ȣ�� ������ ����
        // �߰����� ���� �ο�
        // ������ ���� �ƴϸ鼭 ���� ��ȣ�� ������ ����.
        if (isFire && !isReload)
        {
            // Time.time�� ���� �÷��� ���� ����� �ð�
            // nextFire�� �Ѿ��� �߻�� �ð��� ������.
            if (Time.time >= nextFire)
            {
                // ���� �Լ� ȣ��
                Fire();

                // �ұ�Ģ���� �Ѿ� �߻縦 ���ؼ� Random.Range�� �����.
                nextFire = Time.time + fireRate + Random.Range(0f, 0.3f);
            }

            // enemy�� player�� position(Tr)�� ���ؼ� ������ ȸ��
            // (x, y, z) - (a, b, c)
            // �÷��̾ �ִ� ��ġ�� ȸ�� ���� ���
            // A ���� - B ���� = B���� A������ ����� �Ÿ�
            // B ���� - A ���� = A���� B������ ����� �Ÿ�
            Quaternion rot = Quaternion.LookRotation(playerTr.position - enemyTr.position);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);

        }
    }

    void Fire()
    {
        animator.SetTrigger(hashFire);
        _audio.PlayOneShot(fireSfx, 1f);

        StartCoroutine(ShowMuzzleFlash());

        GameObject _bullet = Instantiate(Bullet, firePos.position, firePos.rotation);
        Destroy(_bullet, 3f);

        currBullet--; // �Ѿ� 1�� ����
        isReload = (currBullet % maxBullet == 0);
        /*
         * if(currBullet == 0)
         *    isReload = true;
         * else
         *    isReload = false;
         */

        if (isReload)
        {
            // ������ �ڷ�ƾ �Լ� ȣ��
            StartCoroutine(Reloading());
        }

    }

    IEnumerator Reloading()
    {
        muzzleFlash.enabled = false;

        animator.SetTrigger(hashReload);
        _audio.PlayOneShot(reloadSfx, 1f);
        yield return wsReload;

        currBullet = maxBullet;
        isReload = false;
    }

    IEnumerator ShowMuzzleFlash()
    {
        // ��Ȱ��ȭ �ߴ� ���� �÷��� Ȱ��ȭ.
        muzzleFlash.enabled = true;

        // ���� �÷��� ������Ʈ�� 0~360���� ȸ���ϱ� ���Ͽ� ���
        Quaternion rot = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
        muzzleFlash.transform.localRotation = rot;
        // Vector3 >> 1, 1 ,1 
        // ���� ���� (1,1,1) �� ���Ϳ� �������� ���ؼ� ũ�⸦ �����ϴ� ��.
        // Vector3 (1, 1, 1) * 2 = (2 ,2 ,2) �� �ȴ�.(�ִ�)
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1f, 2f);
        Vector2 offset = new Vector2(Random.Range(0, 2), Random.Range(0, 2)) * 0.5f;

        // ���̴� ������ ���Ǵ� offset ���� ������ ���� offset���� �����ϱ� ���ؼ�.
        // _MainTex ��� ��Ī�� ���̴� ��ü���� ������� ������
        // ����ڰ� ���� �Ұ�.
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);
        
        // ���� �÷��� �����Ǵ� �ð��� 0.05�ʿ��� 0.2�� �����ϰ� ����
        yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        muzzleFlash.enabled = false;
    }


}
