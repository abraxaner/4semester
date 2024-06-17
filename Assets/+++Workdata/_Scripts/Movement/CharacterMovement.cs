using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{

    int updateCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // updateCount++;
       // Debug.Log("Update:" + updateCount);
    }


    void OnJump()
    {
        Debug.Log("Jump!");
    }

    void OnSneak(InputValue inputValue)
    {
        Debug.Log("Sneak!" + inputValue.Get<float>());
    }

    void OnMove(InputValue inputValue)
    {
        Debug.Log("Move!" + inputValue.Get<Vector2>());
    }
}
