using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBlink : itemInteract
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Action()
    {
        Debug.Log(gameObject.name + " interacted!");
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2.0f);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

}
