using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    // ����ũ ȿ���� �༭ ��鸱 ī�޶�
    public Transform shakeCamera;
    // ȸ���� ��ų�� ���� �Ǵ��ϴ� ����
    public bool shakeRotate = false;
    Vector3 originPos; // ���� ��ġ
    Quaternion originRot; // ���� ȸ����.


    // Start is called before the first frame update
    void Start()
    {
        originPos = shakeCamera.localPosition;
        originRot = shakeCamera.localRotation;

        
    }

    // �ܺ��� ���ο� ���ؼ� Ȱ��ȭ�� �� ���̱� ����.
    public IEnumerator ShakeCamera(float duration = 0.05f, float mPos = 0.03f,float mRot = 0.1f)
    {
        // ��� �ð� ����� ����
        float passTime = 0f;

        // ������ ���� �ð����ȸ� ��鸮���� ����
        while (passTime < duration)
        {
            // �������� 1�� ������ ��ü ��翡�� ��ǥ�� ����
            // (x, y, z)�� (-1, -1, -1) ~ (1, 1, 1) ������ ���� ����.
            // Object Transform Position�� �߽����� �������� 1
            // sqrx + sqry + sqrz = 1
            Vector3 shakePos = Random.insideUnitSphere;
            // ������ ������ ��ġ ���� ���� ī�޶��� ��ġ�� �������ش�.
            // �̵��ϴ� ���� 1�̳� �� ��� �ʹ� ũ�� ��鸱 �� �����Ƿ�
            // ���ϴ� �̵� ������ mPos�� ���ؼ� �� �����̰� �Ѵ�.
            shakeCamera.localPosition = shakePos * mPos;

            // ī�޶� ȸ���� ��Ű�� ���
            if (shakeRotate)
            {
                // ���� �յ��� ������ ����ϱ� ���ؼ� PerlinNoise�� ����Ѵ�.
                // �޸� ������� �ұ�Ģ���� ����� ������� �����Ͽ�
                // �ϰ��� �ִ� ������ ���¸� ���� ����� �߻�
                // ���� ��Ģ���� �ֵ��� ���δ�.
                // �����̳� �繰�� ��ġ�� �� ���� ���Ǹ� 
                // ���� ������ �ʵ忡 �ִ� ������ Ǯ ���� ���� �� ���� ����Ѵ�.
                float noise = Mathf.PerlinNoise(Time.time * mRot, 0f);
                Vector3 shakeRot = new Vector3(0, 0, noise);
                // ������ ���� ȸ������ ī�޶� ����
                shakeCamera.localRotation = Quaternion.Euler(shakeRot);
                
            }

            passTime += Time.deltaTime;
            yield return null;
        }
        // ���� �Ŀ� ī�޶��� ��ġ�� ȸ������ �ʱⰪ���� �ٽ� ����.
        shakeCamera.localPosition = originPos;
        shakeCamera.localRotation = originRot;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
