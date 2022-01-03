using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump2D : InOutNode
{
    private Rigidbody2D rigid;
    public float jumpForce;

    public void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public override void In<T>(T input)
    {
        if(typeof(T) == typeof(bool))
        {
            // true를 받았을 때 점프하기
            if((bool)(object)input == false)    // CharacterBase의 SetJump에서 isFalling이 false일때
            {
                if(rigid != null)
                {
                    // Force : 힘
                    rigid.AddForce(Vector2.up * jumpForce);
                }
            }
        }
        
    }
}
