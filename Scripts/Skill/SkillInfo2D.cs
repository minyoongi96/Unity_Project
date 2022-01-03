using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo2D : SkillInfo
{
    public override void SetDirection(Vector3 wantDirection)
    {
        transform.localScale = new Vector3(-wantDirection.x, 1, 1);
    }
}
