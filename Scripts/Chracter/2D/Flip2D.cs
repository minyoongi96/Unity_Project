using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 캐릭터의 이동방향에 따라 캐릭터 뒤집어주기
public class Flip2D : InOutNode        // 값을 받아야 하니 InOutNode
{
    // 일단 섣부른 flip을 시켜주진 않을 것.
    // 오른쪽을 보고 있었던 그림인지, 아닌지를 통해서 상황을 구분
    [SerializeField]
    private bool isRightSprite = false;

    // 그림을 뒤집어줄 SpriteRenderer
    private SpriteRenderer render;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();

    }

    public override void In<T>(T input)
    {
        if(typeof(T) == typeof(bool))
        {
            bool turn = (bool)(object)input;

            // 스프라이트 렌더러가 있는지 먼저 확
            if (render != null)
            {
                // XOR연산
                // turn          = true    true
                // isRightSprite = true    false
                //                ---------------
                //                 false   true
                render.flipX = turn ^ isRightSprite;
            }
        }
    }
}
