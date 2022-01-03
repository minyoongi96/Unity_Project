using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMoveByKeyboard : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CharacterBase.playerCharacter != null)
        {
            foreach(CharacterBase currentCharacter in CharacterBase.playerCharacter)
            {
                currentCharacter.SetMove
                (
                new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0)
                );
            }

            
        }
    }
}
