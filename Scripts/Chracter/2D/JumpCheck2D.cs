using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck2D : InOutNode
{
    private CharacterBase target;

    public override void In<T>(T input)
    {
        if(typeof(T) == typeof(CharacterBase))
        {
            target = (CharacterBase)(object)input;
        }
    }

    // 닿거나 떨어지거나 땅을 체크하는 메소드
    private bool CheckGround()
    {
        // 충돌체, 어떤거랑 충돌한지 알기위해
        // CapsuleCollider2D는 Collder2D의 자식 클래스라 Collider2D 선언
        Collider2D myCollider = GetComponent<Collider2D>();

        // 닿은 위치를 표시하고 있는 리스트
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        myCollider.GetContacts(contacts);   // 구체적으로 어떤 object와 충돌중인지

        // 바닥과 닿아있으면
        foreach(ContactPoint2D contact in contacts)
        {
            // contact.point는 닿은 위치를 표시하는데
            // 바닥 체크하는 것은 y만 확인하면 됨
            if(myCollider.bounds.center.y - myCollider.bounds.extents.y + myCollider.bounds.extents.x > contact.point.y)
            {
                return false;
            }
        }

        return true;
    }

    // 지금 오브젝트가 무언가에 닿은(충돌) 순간 불러와짐
    private void OnCollisionEnter2D(Collision2D collision)
    {

        target.SetFall(CheckGround());
    }
    // 오브젝트가 무언가에 떨어졌을 때 불러와ㅈ
    private void OnCollisionExit2D(Collision2D collision)
    {
        target.SetFall(CheckGround());
    }
}
