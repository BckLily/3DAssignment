using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// struct: 구조체, 클래스 열화판으로 생각하면 편리하다.
// 클래스는 구조체 이후에 등장한 개념
// 지금에 와서는 메모리 적재 형태의 차이가 있을 뿐
// 기능상의 큰 차이는 없다.
[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour
{
    // Inspector View에서 끌어와서 사용할 수도 있고
    // 코드로 직접 대입해서 사용할 수도 있다.
    public GameObject bullet; // 총알 프리팹 사용하기 위한 변수
    public Transform firePos; // 총알 발사 위치
    public ParticleSystem cartridge; // 탄피 프리팹 사용 변수
    private ParticleSystem muzzleFlash; // 총구 화염 파티클



    public enum WeaponType
    {
        RIFLE = 0, SHOTGUN,
    }

    public WeaponType currentWeapon = WeaponType.RIFLE;

    AudioSource _audio;
    // 오디오 클립 저장 변수
    public PlayerSfx playerSfx;

    Shake shake;

    // Start is called before the first frame update
    void Start()
    {
        // firePos Object아래에 있는 Child 중 ParticleSystem Component를 가진 Object 하나를 가져옴.
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
        _audio = GetComponent<AudioSource>();


        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
    }

    // Update is called once per frame
    void Update()
    {
        // GetMouseButton >> 버튼을 누르고 있으면 Update마다 계속 동작
        // GetMouseButtonDown / Up >> 버튼을 계속 누르고 있어도 딱 한 번 만 동작.
        // 0이면 좌클릭, 1이면 우클릭.
        if (Input.GetMouseButtonDown(0))
        {
            // 공격 함수 호출
            Fire();
        }
    }

    void Fire()
    {
        // shake 스크립트 내부의 ShakeCamera 코루틴 함수 호출
        // 매개 변수 값을 생략했으므로 ShakeCamera 함수에 설정된
        // 기본 값으로 동작한다.
        StartCoroutine(shake.ShakeCamera());

        // 총알을 동적 생성
        // Instantiate(내가 생성하려고 하는 Object, Object가 생성되는 위치, Object가 향하는 방향);
        // Instantiate(동적 생성할 오브젝트, 위치, 방향);
        // 사용되지 않는 객체(Object)를 활성화 해주는 함수.
        Instantiate(bullet, firePos.position, firePos.rotation);
        // Hierarchy에 올라가 있는 Object >> instance Object. 활성화 중인 객체. 인스턴스 화.
        cartridge.Play(); // 탄피 파티클 재생.
        muzzleFlash.Play(); // 총구 화염 파티클 재생
        FireSfx(); // 공격시 사운드 발생
    }

    void FireSfx()
    {
        // 현재 선택된 무기의 넘버에 맞는 사운드를 선택해서 가지고 온다.
        var _sfx = playerSfx.fire[(int)currentWeapon];
        // PlayOneShot(재생 음원, 볼륨 크기)
        // 볼륨 크기는 0 ~ 1 사이의 값을 가진다.
        // Play()는 재생 음원을 항상 최대 크기의 볼륨으로 재생한다.
        _audio.PlayOneShot(_sfx, 1f);

        
    }


}
