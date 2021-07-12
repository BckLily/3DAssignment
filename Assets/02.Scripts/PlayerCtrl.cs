using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// ���� Ŭ����
// Ŭ������ Insepctor View�� ǥ�õ����� �ʴ´�.
// ����ȭ: Inspector View�� ���̰� �ϱ� ���ؼ� �ٷ� �Ⱦƹ����°�(?)
// Ŭ������ ��� ������ �޸� ����ȭ ����� �ν����Ϳ� ǥ�õ�.
// using System;�� �߰��ϸ� [Serializable]�� ����� �� �ִ�.
[System.Serializable]
public class PlayerAnim
{
    // Player Animtion Clip�� ������ �ϱ� ���� Class.
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
}

public class PlayerCtrl : MonoBehaviour
{
    // File name �� Class name�� �����ؾ� �Ѵ�.
    // private: ���� ������ (���ٿ� ������ �δ� ��)
    // private�� �ۼ��� ��ũ��Ʈ ���ο����� ���ȴ�.
    // public�� ���� �ܺ� ��𿡼��� �� �� �ִ�.
    // �⺻ ���´� private�̴�.
    // �߰������� Protected��� ���� �����ڰ� �ִ�. Ŭ���� ����(����)�� ��ӵ� Ŭ���������� ����� �� �ִ�.
    // private >> ������ Ŭ����(PlayerCtrl)�� �ۿ��� �����ϴ� ���� �����ϴ� ������)
    // public  >> ������ Ŭ������ �ۿ��� �����ϴ� ���� ������ ������.
    // horizontal / vertical
    private float h = 0f; // float h = 0f;
    private float v = 0f; // float v = 0f;
    private float r = 0f;


    // Transform Component ���� ����
    Transform tr;
    // public���� ����� ������ Inspector View�� ����(ǥ��)�ȴ�.
    public float moveSpeed = 10f;
    public float rotSpeed = 300f;

    public PlayerAnim playerAnim;
    public Animation anim;


    // Start is called before the first frame update
    void Start()
    {
        // Transform Component�� tr ���� ����
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();

        anim.clip = playerAnim.idle;
        anim.Play();

        // �ν��Ͻ�ȭ �Ǿ���.
    }

    // Update is called once per frame
    void Update()
    {
        // GetAxis :: ������ �ִ� �̵�
        // GetAxisRaw :: ���������� �̵�
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");

        //print("H�� -" + h + " V�� -" + v); // log Check

        // Vector ���� ����ؼ� �����¿� �� �ƴ϶� �밢�� �̵��� Ŀ�� ����.
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        // ���� ����ȭ
        // �밢�� �̵��� �Ϲ����� �̵����� ũ�Ⱑ Ŀ�� ���� �����δ�.
        // �̸� �ذ��ϱ� ���ؼ� ������ ũ�⸦ �����ϰ� 1�� ����ȭ ��Ų��.
        // ���� ���͸� ����� ��.
        moveDir = moveDir.normalized;

        // Vector3.forward: ����
        // moveSpeed: �츮�� ������ �����̴� �ӵ�
        // v: Input.GetAxis�� ����ؼ� �Է¹��� -1 ~ 1������ ��.
        // Time.deltaTime: ������ ���������� ������ �� �ֵ��� �����ϴµ� ����ϴ� ��.
        // The interval in seconds from the last frame to the current one (Read Only).
        // Space.Self >> �ڱ� �ڽ�(Object)�� �������� (x, y, z) ���� ����. (Local ��ǥ)
        // Space.World>> Unity �� ���� �������� (x, y, z) ���� ����. (Global ��ǥ)
        tr.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);

        // ȸ���� ������ �Ǵ� ����(y��) Vector3.up
        tr.Rotate(Vector3.up * rotSpeed * Time.deltaTime * r);

        //// Vector3.Magnitude >> () ���� ���� ũ�⸸�� �������� ��.
        //print(Vector3.Magnitude(Vector3.forward + Vector3.right));
        //print(Vector3.Magnitude((Vector3.forward + Vector3.right).normalized));


        // Animation ���� ����
        if (v >= 0.1f) // ����(Front)
        {
            // CrossFade(AnimationClip.name, ��ȯ �ð�);
            anim.CrossFade(playerAnim.runF.name, 0.3f);
        }
        else if (v <= -0.1f) // ����(Back)
        {
            // CrossFade(AnimationClip.name, ��ȯ �ð�);
            anim.CrossFade(playerAnim.runB.name, 0.3f);
        }
        else if (h >= 0.1f) // ����(Right)
        {
            // CrossFade(AnimationClip.name, ��ȯ �ð�);
            anim.CrossFade(playerAnim.runR.name, 0.3f);
        }
        else if (h <= -0.1f) // ����(Left)
        {   // CrossFade(AnimationClip.name, ��ȯ �ð�);
            anim.CrossFade(playerAnim.runL.name, 0.3f);
        }
        else // �Է��� ������ idle �������� ����ȴ�.
        {
            anim.CrossFade(playerAnim.idle.name, 0.3f);
        }



    }
}
