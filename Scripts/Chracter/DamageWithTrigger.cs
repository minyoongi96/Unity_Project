using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageWithTrigger : MonoBehaviour
{
    public CharacterBase from;
    public float damage;
    public bool isAlly;

    private void OnTriggerEnter2D(Collider2D collision) {
        
        CharacterBase character = collision.GetComponent<CharacterBase>();

        // 캐릭터인지 확인, 동맹인지 확인
        if(character != null && character.isAlly != isAlly)
        {
            character.SetDamage(from, (int)damage);
        }
    }
}
