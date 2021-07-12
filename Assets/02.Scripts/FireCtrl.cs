using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// struct: ����ü, Ŭ���� ��ȭ������ �����ϸ� ���ϴ�.
// Ŭ������ ����ü ���Ŀ� ������ ����
// ���ݿ� �ͼ��� �޸� ���� ������ ���̰� ���� ��
// ��ɻ��� ū ���̴� ����.
[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour
{
    // Inspector View���� ����ͼ� ����� ���� �ְ�
    // �ڵ�� ���� �����ؼ� ����� ���� �ִ�.
    public GameObject bullet; // �Ѿ� ������ ����ϱ� ���� ����
    public Transform firePos; // �Ѿ� �߻� ��ġ
    public ParticleSystem cartridge; // ź�� ������ ��� ����
    private ParticleSystem muzzleFlash; // �ѱ� ȭ�� ��ƼŬ



    public enum WeaponType
    {
        RIFLE = 0, SHOTGUN,
    }

    public WeaponType currentWeapon = WeaponType.RIFLE;

    AudioSource _audio;
    // ����� Ŭ�� ���� ����
    public PlayerSfx playerSfx;

    Shake shake;

    // Start is called before the first frame update
    void Start()
    {
        // firePos Object�Ʒ��� �ִ� Child �� ParticleSystem Component�� ���� Object �ϳ��� ������.
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
        _audio = GetComponent<AudioSource>();


        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
    }

    // Update is called once per frame
    void Update()
    {
        // GetMouseButton >> ��ư�� ������ ������ Update���� ��� ����
        // GetMouseButtonDown / Up >> ��ư�� ��� ������ �־ �� �� �� �� ����.
        // 0�̸� ��Ŭ��, 1�̸� ��Ŭ��.
        if (Input.GetMouseButtonDown(0))
        {
            // ���� �Լ� ȣ��
            Fire();
        }
    }

    void Fire()
    {
        // shake ��ũ��Ʈ ������ ShakeCamera �ڷ�ƾ �Լ� ȣ��
        // �Ű� ���� ���� ���������Ƿ� ShakeCamera �Լ��� ������
        // �⺻ ������ �����Ѵ�.
        StartCoroutine(shake.ShakeCamera());

        // �Ѿ��� ���� ����
        // Instantiate(���� �����Ϸ��� �ϴ� Object, Object�� �����Ǵ� ��ġ, Object�� ���ϴ� ����);
        // Instantiate(���� ������ ������Ʈ, ��ġ, ����);
        // ������ �ʴ� ��ü(Object)�� Ȱ��ȭ ���ִ� �Լ�.
        Instantiate(bullet, firePos.position, firePos.rotation);
        // Hierarchy�� �ö� �ִ� Object >> instance Object. Ȱ��ȭ ���� ��ü. �ν��Ͻ� ȭ.
        cartridge.Play(); // ź�� ��ƼŬ ���.
        muzzleFlash.Play(); // �ѱ� ȭ�� ��ƼŬ ���
        FireSfx(); // ���ݽ� ���� �߻�
    }

    void FireSfx()
    {
        // ���� ���õ� ������ �ѹ��� �´� ���带 �����ؼ� ������ �´�.
        var _sfx = playerSfx.fire[(int)currentWeapon];
        // PlayOneShot(��� ����, ���� ũ��)
        // ���� ũ��� 0 ~ 1 ������ ���� ������.
        // Play()�� ��� ������ �׻� �ִ� ũ���� �������� ����Ѵ�.
        _audio.PlayOneShot(_sfx, 1f);

        
    }


}
