using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class skill_select : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] select_skills;
    public GameObject[] skills;
    public Button next_select_btn;
    const int MAX_SKILL_SELECT = 4;
    int selected_skill_num = 0;

    public void increase_skill_select()
    {
        if (selected_skill_num == MAX_SKILL_SELECT)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                select_skills[i].GetComponent<Button>().interactable = false;
                next_select_btn.interactable = true;
            }
        }
        else
        {
            selected_skill_num++;
            print(selected_skill_num);
        }
    }

    void Start()
    {
        GameObject skilltab = GameObject.Find("Canvas").transform.Find("Skill").gameObject;
        skilltab.SetActive(true);
        skills = GameObject.FindGameObjectsWithTag("skill");
        select_skills = GameObject.FindGameObjectsWithTag("skill_select");
        next_select_btn.interactable = false;
        for (int i = 0; i < skills.Length; i++) {
            skills[i].SetActive(false);
        }
        skilltab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
