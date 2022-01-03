using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InOutNode : MonoBehaviour
{
    // 값을 만들게 되면 여기에 따라서 아웃리스트에 있는 요소들에게 전부 뿌려주기 위해
    [SerializeField]
    private List<InOutNode> outList;

    // 제네릭 메소드 : <안에> 자료형을 써주면 그 자료형에 맞게끔 함수를 변형시켜준다
    // 모든 자료형을 명시하지 않더라도 자동으로 자료형을 맞춰준다
    // ex) In(1.2f), In(1)
    virtual public void In<T>(T input)
    {
        
    }


    protected void Out<T>(T output)
    {
	// 모든 노드들을 돌면서 제 값을 하나하나 outList에 입력해주기
        foreach(InOutNode node in outList)
        {
            if(node != null)
            {
                node.In(output);
            }
        }
    }
}
