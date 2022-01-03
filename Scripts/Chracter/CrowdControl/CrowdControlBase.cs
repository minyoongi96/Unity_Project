using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CCType
{
    Root,   // 속박
    Disarm, // 무장해제
}

[System.Serializable]
public class CrowdControlBase
{
    public CCType type;     // CC기 종류
    public float leftTime;  // 남은 시간
    public bool isInfinity;   // 무한 지속 여부


    // from의 것을 그래로 복사해 가져오기
    public CrowdControlBase(CrowdControlBase from)
    {
        type = from.type;
        leftTime = from.leftTime;
        isInfinity = from.isInfinity;
    }

    public CrowdControlBase(CCType wantType, float wantTime, bool wantInfinity)
    {
        type = wantType;
        leftTime = wantTime;
        isInfinity = wantInfinity;
    }
    public CrowdControlBase()
    {

    }

}
