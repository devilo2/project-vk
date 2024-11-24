using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class show_skill : MonoBehaviour
{
    PlayerData playerData;
    // Start is called before the first frame update

    public void showing_skills(){
        GameObject human_skill_tab = GameObject.Find("인간스킬탭");
        GameObject automaton_skill_tab = GameObject.Find("오토마톤 스킬탭");
        GameObject furry_skill_tab = GameObject.Find("수인 스킬탭");

        human_skill_tab.SetActive(false);
        automaton_skill_tab.SetActive(false);
        furry_skill_tab.SetActive(false);

        int specis = 1;

        if (specis == 0){
            human_skill_tab.SetActive(true);
        }

        if (specis == 1){
            automaton_skill_tab.SetActive(true);
        }

        if (specis == 2){
            furry_skill_tab.SetActive(true);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
