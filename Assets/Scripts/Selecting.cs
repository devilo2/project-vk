using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Selecting : MonoBehaviour
{
    //현재 선택하고 있는 특성 enum
    //이름은 gameObject의 이름과 같음
    private enum SelectingStatus
    {
        Species,
        Status,
        Skill,
        Persona,
        Tool,
        END
    }

    SelectingStatus curSelecting = SelectingStatus.Species;

    PlayerData playerData;
    private void Start()
    {
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
    }
    
    //이전 선택 특성을 비활성화 후 다음 선택 특성을 비활성화
    public void NextSelecting()
    {
        
        string preSelectingName = curSelecting.ToString();
        curSelecting += 1;
        
        if (curSelecting == SelectingStatus.END)
        {
            SceneManager.LoadScene("exploration");
        }

        string curSelectingName = curSelecting.ToString();
        GameObject.Find(preSelectingName).SetActive(false);
        GameObject.Find("Canvas").transform.Find(curSelectingName).gameObject.SetActive(true);
    }

    public void SetSpiecsHuman()
    {
        playerData.species = Species.Human;
        NextSelecting();
    }

    public void SetSpiecsAutomaton()
    {
        playerData.species = Species.Automaton;
        NextSelecting();
    }

    public void SetSpiecsFurry()
    {
        playerData.species = Species.Furry;
        NextSelecting();
    }

    public void ButtonTest()
    {
        NextSelecting();
        print(curSelecting);
    }
    
}
