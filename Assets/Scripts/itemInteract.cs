using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class itemInteract : MonoBehaviour
{
    protected GameObject interactUI;
    protected bool inArea = false;

    abstract public void Action();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GameObject UI = GameObject.Find("UI");
        interactUI = UI.transform.Find("interact_key").gameObject;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && inArea)
        {
            Action();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        interactUI.SetActive(true);
        inArea = true;
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        interactUI.SetActive(false);
        inArea = false;
    }

}
