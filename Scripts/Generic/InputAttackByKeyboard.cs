using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAttackByKeyboard : MonoBehaviour
{
    [SerializeField]
    private KeyCode key;

    void Update()
    {
        if (CharacterBase.playerCharacter != null && Input.GetKeyDown(key))
        {
            foreach(CharacterBase currentCharacter in CharacterBase.playerCharacter)
            {
                currentCharacter.SetAttack(Vector3.zero);
            }
            
        }
    }
}
