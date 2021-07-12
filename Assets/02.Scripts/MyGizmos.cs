using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color _color = Color.yellow;
    public float _radius = 0.1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        // 해당 위치에 _radius의 크기만큼 Gizmos를 그린다.
        // DrawShpere로 구형의 Gizmos를 생성.
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
