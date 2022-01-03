using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 클래스 이름은 항상 파일 이름이랑 똑같이 맞춰주기!
// MonoBehaviour는 컴포넌트로 넣기 위한 거니까 항상 신경써주기

// 이걸 돌리려면 Rigidbody2D를 넣어줘야함
[RequireComponent(typeof(Rigidbody2D))]

public class Move2D : InOutNode
{
	// 움직이려면 이렇게 확인(변수로 Rigidbody2D를 선언)
	private Rigidbody2D rigid;

	private CharacterBase character;	// 캐릭터 받아주기

	// 이동 속도
	[SerializeField]
	private float moveSpeed;

	// 강제로 이동되고 있는 속도
	private float pushSpeed;

	// 현재 움직이는 방향
	private float moveDirection = 0.0f;

    public override void In<T>(T input)
    {
		if(typeof(T) == typeof(Vector2))	// 캐릭터가 밀려나가는 양을 설정해주기
		{
			Vector2 value = (Vector2)(object)input;

			pushSpeed = value.x / 2;	// 공격에 의헤 밀렸을 때

			// 속도 반영해주기
			rigid.AddForce(value);
		}

		// 받은 내용을 체크
		if (typeof(T) == typeof(Vector3))
		{
			Vector3 direction = ((Vector3)(object)input);
			// Vector3로 읽어서 x축만 받아오기
			moveDirection = direction.x;

			if(direction.magnitude > 0.1f)
			{
				// 이동하려는 방향이 오른쪽
				Out(direction.x > 0.0f);
				if(character != null)
				{
					if(direction.x > 0.0f)
					{
						character.forward = Vector3.right;
					}
					else
					{
						character.forward = Vector3.left;
					}
				}
			}

        }
		else if(typeof(T) == typeof(CharacterBase))
		{
			character = (CharacterBase)(object)input;
		}
    }

    // 컴포넌트가 만들어질 때(시작할 때) 처음 한 번 실행되는 곳
    void Start()
    {
        // 동일한 성격의 컴포넌트를 불러오기
	rigid = GetComponent<Rigidbody2D>();
    }

    // 매 프레임마다 실행되는 곳
    void Update()
    {	

	float calculatedVelocityX = moveDirection * moveSpeed;
	// 유니티의 "입력"에 해당하는 부분
	// 수평 축을 확인하기
	//Debug.Log(Input.GetAxis("Horizontal"));

	// x축(좌우)로는 입력한 내용(Horizontal)으로 이동하고
	// y축(상하)로는 그냥 그대로 입력
	// x축에 밀려나는 속도도 같이 확인
	rigid.velocity = new Vector2(calculatedVelocityX + pushSpeed, rigid.velocity.y);

	pushSpeed *= 0.3f;	// 무한정 날아가지 않게 줄여주기
    }
}
