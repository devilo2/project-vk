using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blink : MonoBehaviour
{
    // Start is called before the first frame update
    public void Blink() {
        StartCoroutine(Blink_coroutine());
    }

    IEnumerator Blink_coroutine()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2.0f);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
