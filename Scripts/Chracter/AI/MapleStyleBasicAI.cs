using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapleStyleBasicAI : MonoBehaviour
{
    [SerializeField]
    private CharacterBase target;

    [SerializeField]
    private Collider2D col;

    private Vector3 moveDirection = Vector3.left;  // 움직이는 방향(기본 방향은 왼쪽으로)

    [SerializeField]
    private int minDamage;  // 몬스터가 주는 최소 데미지

    [SerializeField]
    private int maxDamage;  // 몬스터가 주는 최대 데미지

    private float changeDirectionTime = 0;   // 이동 방향을 바꾼 시간

    private float restEndTime = 0;    // 쉬기가 끝난 시간

    private float moveTime = 1.0f;  // 움직이는 시간
    private bool resting = true;    // 쉬고 있는지

    private void Update()
    {
        if(target == null) return;  // 타겟이 없으면 return

        // 모든 플레이어 캐릭터를 확인
        foreach(CharacterBase current in CharacterBase.playerCharacter)
        {
            // 대상이 null이 아니고, 대상의 collider2D 확인
            if(current != null && current.col2D != null)
            {
                // 몬스터와 플레이어의 x, y 거리 차이
                float xDistance = Mathf.Abs(current.col2D.bounds.center.x - col.bounds.center.x);
                float yDistance = Mathf.Abs(current.col2D.bounds.center.y - col.bounds.center.y);

                // 둘의 반지름을 더하기
                float xTouch = current.col2D.bounds.extents.x + col.bounds.extents.x;
                float yTouch = current.col2D.bounds.extents.y + col.bounds.extents.y;

                // 둘의 거리가 둘의 반지름의 합보다 작으면 부딪힌 것
                if(xDistance <= xTouch && yDistance <= yTouch)
                {
                    current.SetDamage(target, Random.Range(minDamage, maxDamage)); // 랜덤하게 데미지 주기
                }
            };
        };

        if(!resting)
        {
            // 지형에서 떨어지기 전에(더이상 앞에 바닥이 없을 때)
            if(!target.isFalling)
            {
                // 콜라이더가 없었으면 가져오기
                if(col == null) col = target.GetComponent<Collider2D>();

                RaycastHit2D hit;
                // Raycast(시작 위치, 방향, 거리, 레이어 마스크) : 시작위치 -> 어떤 방향으로 선을 쏜다
                // 캐릭터 맨 아래 위치를 확인
                // 레이어 마스크에 8을 넣는 이유 : 땅의 Layer를 3번으로 설정했으므로 1에서 Ground 위치만큼 left shift
                hit = Physics2D.Raycast(target.transform.position, Vector2.down,
                                        col.bounds.extents.y + 0.5f, 1 << LayerMask.NameToLayer("Ground"));

                //Debug.DrawRay(target.transform.position, Vector2.down * (col.bounds.extents.y + 0.5f));
                
                // 바닥이 없는 곳에서 and 방향 바꾼 시간이 1초 지났을 때
                if(hit.transform == null && changeDirectionTime + 1.0f < Time.realtimeSinceStartup)
                {
                    moveDirection *= -1;    // 반대쪽으로 이동

                    transform.Translate(moveDirection * Time.deltaTime); // 그 방향대로 툭 쳐주기

                    changeDirectionTime = Time.realtimeSinceStartup;    // 방향을 바꾼 시간(현재 시간)을 저장
                };
            };

            target.SetMove(moveDirection);    // 앞으로 이동
        }
        else
        {
            target.SetMove(Vector3.zero);   // 멈추기
        };

        // 카운트 다운을 세면서 쉴지 말지를 결정
        moveTime -= Time.deltaTime;

        if(moveTime < 0)
        {
            resting = !resting; // 쉬고 있을 때에는 움직이고, 움직이고 있었을 때는 쉬는걸로
            moveTime = Random.Range(2.0f, 7.0f);
        };
    }
}
