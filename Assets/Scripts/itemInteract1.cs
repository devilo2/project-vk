using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemInteract1 : MonoBehaviour
{
    public GameObject interactUI;
    private bool inArea = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("!!!!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inArea)
        {
            Debug.Log(gameObject.name + " interacted!");

            StartCoroutine(Blink());

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactUI.SetActive(true);
        inArea = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
    IEnumerator Blink()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2.0f);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactUI.SetActive(false);
        inArea = false;
    }

}
