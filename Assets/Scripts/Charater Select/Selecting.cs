using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Selecting : MonoBehaviour
{
    //���� �����ϰ� �ִ� Ư�� enum
    //�̸��� gameObject�� �̸��� ����
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
    
    //���� ���� Ư���� ��Ȱ��ȭ �� ���� ���� Ư���� ��Ȱ��ȭ
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
