using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillBase
{
    public string motionName;       // 스킬 사용은 모션을 동반함
    public float nextComboTime;     // 콤보를 이어가기 위한 시간
    public CrowdControlBase[] selfCCList;    // 이 스킬을 쓰면 내가 걸리게 될 CC 리스트
    public bool isLandAttack;       // 바닥에 있을때만 쓸수 있는 기술인지
    public GameObject skillPrefab;
}

public class CharacterBase : MonoBehaviour
{
    // CharacterBase 모두가 공유하는 static을 사용해서
    // 누가 플레이어 캐릭터인지 모두가 알 수 있게끔 만들어주기
    // 여러마리 일수도 있으니 리스트로
    public static List<CharacterBase> playerCharacter { get; private set; }

    [SerializeField]
    private Gauge health;
    
    [SerializeField]
    private InOutNode jumpCheckNode = null;

    [SerializeField]
    private InOutNode jumpNode;

    [SerializeField]
    private InOutNode moveNode;

    private float comboTime = 0.0f; // 다음 콤보를 이어갈 수 있는 시간
    private int comboIndex = -1;    // 현재 콤보 번호

    // 플레이어를 구분할 boolean 변수
    [SerializeField]
    private bool isPlayerCharacter = false;

    // 캐릭터가 공중에 떠있는지 체크
    public bool isFalling = false;

    [SerializeField]
    private bool jumpable = true;

    [SerializeField]
    private bool movable = true;

    [SerializeField]
    private bool attackable = true;

    [Tooltip("플레이어와 동맹상태인 캐릭터인지 확인합니다.")]
    public bool isAlly;


    // 캐릭터에 걸려있는 모든 CC기
    [SerializeField]
    private List<CrowdControlBase> CCList = new List<CrowdControlBase>();

    [SerializeField]
    private SkillBase[] basicAttack;    // 이 캐릭터가 사용할 수 있는 기본 공격 리스트

    private Animator anim;

    // 밖에서 콜라이더를 볼수만 있게끔
    public Collider col3D { get; private set; }
    public Collider2D  col2D { get; private set; }

    public Vector3 forward = Vector3.right;   // 캐릭터가 보고 있는 방향

    [SerializeField]
    [Tooltip("무적 지속 시간")]
    private float wantUntouchable;
    private float lastHitTime;

    private void Start()
    {
        // 만약 리스트를 아직 만들지 않은 상태였다면 리스트 만들어주기
        if (playerCharacter == null)
        {
            playerCharacter = new List<CharacterBase>();
        }

        // 내가 플레이어야! 라고 이야기해주는 
        // 플레이어 캐릭터 목록에다가 추가해주기
        if(isPlayerCharacter) playerCharacter.Add(this);

        // 점프체크노드한테 말해주기, "나한테 주면 돼"
        if (jumpCheckNode != null)
        {
            // this 대신 람다 함수 넣어도 됨
            // delegate void 델리게이트이름(인자)
            // 델리게이트이름 myFunction = 넣을 인자 => {내용};
            jumpCheckNode.In(this);
        }

        if(moveNode != null)
        {
            moveNode.In(this);
        }

        anim = GetComponent<Animator>();

        col3D = GetComponent<Collider>();
        col2D = GetComponent<Collider2D>();
    }

    public void OnDestroy()
    {
        // 이 캐릭터가 부서질때, 플레이어 리스트에서 this를 삭제 
        if (playerCharacter == null && isPlayerCharacter) playerCharacter.Remove(this);
    }

    public void SetDamage(CharacterBase from, int damage)
    {
        if(lastHitTime + wantUntouchable <= Time.realtimeSinceStartup)  //realtimesinceStartup : 현재 시간
        {
            if(anim != null) anim.SetTrigger("Hit");    // 모든 애니메이션한테 'Hit'라는 트리거를 만들것
            if(health != null) health.cur -= damage;

            SetCrowdControl(new CrowdControlBase(CCType.Root, 0.2f, false));    // 맞을 때 이동불가 잠깐 시켜주기

            // (목적지 - 출발지) = 해당 목적지로 가는 방향, normalized는 1로 맞춰주기, 원하는 만큼 튕겨나가기 위해

            // 대상과 나의 위치를 확인해서 오른쪽이면 1, 왼쪽이면 -1
            float popDirection = (transform.position.x - from.transform.position.x) > 0 ? 1 : -1;

            moveNode.In( new Vector2( popDirection * 20.0f, 100.0f ) );
            
            //().normalized * 20.0f);
        


            lastHitTime = Time.realtimeSinceStartup;    // 맞은 시간 저장
        }
    }

    public void SetFall(bool fall)
    {
        isFalling = fall;
        // 떨어지는 중이라고 전달
        anim.SetBool("IsFalling", fall);
    }

    // 군중제어 추가해주는 메서드
    public void SetCrowdControl(CrowdControlBase wantCrowdControl)
    {
        if(wantCrowdControl != null)
        {
            CCList.Add(wantCrowdControl);
        };
    }

    public void SetMove(Vector3 value)
    {
        //만약에 움직일 수 있는 노드가 붙어있지 않은 상태일때
        if (moveNode == null) return;

        // 움직일 수 있는 상황일 때에만 움직이라고 전달!
        if(movable)
        {
            moveNode.In(value);

            if(anim != null)
            {
                anim.SetBool("IsWalking", value.magnitude > 0.1f);
            }
            
        }

        // 움직일 수 없는 상황일 때에는 안움직이게 0값을 전달!
        else
        {
            if (anim != null)
            {
                moveNode.In(Vector3.zero);
                anim.SetBool("IsWalking", false);
            }
        }
    }

    public void SetJump() 
    {
        if(jumpNode == null) return;

        // 점프가 가능한 상태인지 확인
        if(jumpable)
        {
            // 바닥에 붙어있는지도 확인
            jumpNode.In(isFalling);
        }
    }

    public void SetAttack(Vector3 direction)
    {
        if(basicAttack.Length <= 0) return; // 공격이 없다면
        if(attackable)
        {
            if(direction == Vector3.zero)
            {
                direction = forward;   // 공격할 방향이 정해지지 않았으면, 보고있던 방향으로 attack
            }
            // 콤보 시간 안에 또 입력됬을 때
            if(comboTime > Time.realtimeSinceStartup)
            {
                comboIndex += 1;    // 콤보를 다음으로 미뤄주기
                comboIndex %= basicAttack.Length;   // basicAttack의 갯수맘큼 계속 돌게 하기위해
            }

            else{ comboIndex = 0; };     // 콤보 끊김

            SkillBase currentSkill = basicAttack[comboIndex];   // 현재 콤보에 맞는 공격

            // 스킬이 없거나 || 점프공격은 점프중에만(지상공격은 지상에서만)
            if(currentSkill == null || (currentSkill.isLandAttack && isFalling)) return;

            // 공격에 적혀있는 대로 다음 콤보 준비 시간을 만들어주기(지금 시간 + 유지 시간)
            comboTime = currentSkill.nextComboTime + Time.realtimeSinceStartup;

            anim.SetTrigger(currentSkill.motionName);   // 스킬의 모션을 실행

            if(currentSkill.skillPrefab != null)
            {
                // 오브젝트를 만들 때
                GameObject currentPrefab = Instantiate(currentSkill.skillPrefab);
                currentPrefab.transform.position = transform.position;
                
                SkillInfo info = currentPrefab.GetComponent<SkillInfo>();
                if(info != null)
                {
                    // 자기자신을 스킬 사용하는 주체로 입력
                    info.from = this;
                    // 바라보고 있는 방향으로
                    info.SetDirection(direction);
                    // 공격력, 동맹 확인
                    info.SetInfo(1.0f, isAlly);
                }
            }
            

            // 공격중일때 (공격 모션 중) 움직이지 못하게 셀프CC기 넣기
            CrowdControlBase cc;
            for(int i = 0;i < currentSkill.selfCCList.Length;i++)
            {
                cc = new CrowdControlBase(currentSkill.selfCCList[i]);
                CCList.Add(cc);
            }
        }
    }

    private void CheckCC()
    {
        // CC의 영향을 받을 수 있는 요소들을 미리 초기화
        movable = true;
        jumpable = true;
        attackable = true;

        if(CCList.Count <= 0) return;   // CCList가 비어있다면 반복문 돌릴 필요 없음

        // 캐릭터가 받고 있는 모든 CC기 처리해주기
        for(int i = 0; i < CCList.Count; i++)
        {
            CrowdControlBase current = CCList[i];
            // 무한유지기가 아닐 때 > 시간이 갈수록 CC시간 감소
            if(current.isInfinity == false)
            {
                current.leftTime -= Time.deltaTime;
                if(current.leftTime <= 0)   // 끝났으면 CC기 없애기
                {
                    CCList.RemoveAt(i);
                    --i;
                    continue;
                }
            }

            // 타입에 따라서 CC기의 기능 정해주기
            switch(current.type)
            {
                case CCType.Root:   // 이동불가(Root)
                    movable = false;
                    jumpable = false;
                    break;

                case CCType.Disarm: // 공격불가(Disarm)
                attackable = false;
                break;
            }
            
        }
    }

        private void Update()
    {
        CheckCC();
    }
}
