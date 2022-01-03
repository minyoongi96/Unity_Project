using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillInfo : MonoBehaviour
{
    [SerializeField]
    private List<DamageWithTrigger> damageList = new List<DamageWithTrigger>();

    public CharacterBase from;  // 스킬을 사용하는 캐릭터

    [SerializeField]
    private float leftTime;

    private float damageMultiplier; // 데미지 배율

    private bool isAlly;

    void Update()
    {
        leftTime -= Time.deltaTime;
        if(leftTime < 0) Destroy(gameObject);    
    }

    // 추상함수로 만들어서 자식들한테 만들라고 시키기
    public abstract void SetDirection(Vector3 wantDirection);

    // 동맹을 정하기(캐릭터 or 몬스터)
    public void SetInfo(float wantDamageMultiplier, bool wantAlly)
    {
        damageMultiplier = wantDamageMultiplier;
        isAlly = wantAlly;

        foreach(DamageWithTrigger target in damageList)
        {
            target.from = from;
            target.damage *= damageMultiplier;
            target.isAlly = wantAlly;
        }
    }
}
