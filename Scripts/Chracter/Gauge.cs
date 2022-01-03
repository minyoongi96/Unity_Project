using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauge : MonoBehaviour
{
    public float max;
    public float cur;

    void Start()
    {
        cur = max;
    }

    void Update()
    {
        // 하나의 값을 최소 ~ 최대 사이에 두게 하기
        cur = Mathf.Clamp(cur, 0, max);
    }
}
