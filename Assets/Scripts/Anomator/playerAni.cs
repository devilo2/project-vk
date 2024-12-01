using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAni : MonoBehaviour
{
    Animator playerAnimator;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            playerAnimator.Play("Attack");
            playerAnimator.Play("idle");
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            playerAnimator.Play("hit");
            playerAnimator.Play("idle");
        }
        if(Input.GetKeyUp(KeyCode.R))
        {
            playerAnimator.Play("Death");
        }
        if( Input.GetKeyUp(KeyCode.Z))
        {
            playerAnimator.Play("idle");
        }
    }
}
