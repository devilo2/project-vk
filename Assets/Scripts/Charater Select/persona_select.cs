using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class persona_select : MonoBehaviour
{
    public GameObject[] persona;
    public GameObject[] select_persona;
    const int MAX_PERSONA_NUM = 1;
    public int selected_persona_num = 0;
    public Button next_select_btn;
    // Start is called before the first frame update

    public void personaselect()
    {
        if (selected_persona_num == MAX_PERSONA_NUM)
        {
            for (int i = 0; i < persona.Length; i++)
            {
                select_persona[i].GetComponent<Button>().interactable = false;
                next_select_btn.interactable = true;
            }
        }
        else
        {
            selected_persona_num++;
            print(selected_persona_num);
        }
    }
    void Start()
    {
        GameObject persona_tab = GameObject.Find("Canvas").transform.Find("Persona").gameObject;
        persona_tab.SetActive(true);
        persona = GameObject.FindGameObjectsWithTag("persona");
        select_persona = GameObject.FindGameObjectsWithTag("persona_select");
        next_select_btn.interactable = false;
        for (int i = 0; i < persona.Length; i++)
        {
            persona[i].SetActive(false);
        }
        persona_tab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
