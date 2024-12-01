using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class itemInteract1 : MonoBehaviour
{
    public GameObject interactUI;
    private bool inArea = false;
    [SerializeField] private UnityEvent onInteract;
    public int num = Random.Range(1, 4);

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
            if (num == 1)
            {
                PlayerData.EnableEnergy += 1;
                
            }
            if (num == 2)
            {
                PlayerData.ReturningGear += 1;
            }
            if (num == 3)
            {
                PlayerData.SelfRecoveryPowerCapsule += 1;
            }

            // StartCoroutine(Blink());
            onInteract.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(interactUI != null) {
            interactUI.SetActive(true);
        }
        inArea = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(interactUI != null) {
            interactUI.SetActive(false);
        }
        inArea = false;
    }

}
