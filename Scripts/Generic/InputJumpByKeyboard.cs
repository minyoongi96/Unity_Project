using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputJumpByKeyboard : MonoBehaviour
{
    // 키보드를 누르면 숫자가 나간다(Keycode)
    [SerializeField]
    private KeyCode key;

    void Update()
    {
        // 키보드가 눌릴 때에 점프
        // Input.GetKeyDown은 선택한 키가 눌리는 순간
        // Input.GetKey는 누르는 동안 계속
        // Input.GetKeyUp은 뗴는 순간
        if (CharacterBase.playerCharacter != null && Input.GetKeyDown(key))
        {
            foreach(CharacterBase currentCharacter in CharacterBase.playerCharacter)
            {
                currentCharacter.SetJump();
            }
            
        }
    }
}
