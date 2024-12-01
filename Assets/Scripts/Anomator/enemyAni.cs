using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAni : MonoBehaviour
{
    // Start is called before the first frame update
    Animator enemyAnimator;
    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            enemyAnimator.Play("Attack");
            enemyAnimator.Play("idle");
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            enemyAnimator.Play("death");
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            enemyAnimator.Play("idle");
        }
    }
}
