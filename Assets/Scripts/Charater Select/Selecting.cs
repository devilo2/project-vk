using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using UnityEditor;

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
    Judgment judgment;

    const int MAX_STATUS_SELECT = 6;
    int selected_status_num = 0;
    Boolean[,] selectedStatus = new Boolean[Judgment.STATUS_X_MAX+1, Judgment.STATUS_Y_MAX+1];

    private void Start()
    {
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        judgment = GameObject.Find("Judgement Manger").GetComponent<Judgment>();
        MakeStatusSelectUI();
    }
    

    //스탯 초기화
    private void MakeStatusSelectUI()
    {
        //for(int i = 0; i< Judgment.STATUS_X_MAX; i++)
        //{
        //    for (int j = 0; j < Judgment.STATUS_Y_MAX; j++)
        //    {
        //        print(selectedStatus[i, j]);
        //    }
        //}

        GameObject statusUIPar = GameObject.Find("Canvas").transform.Find("Status").gameObject;
        for (int i = 1; i <= Judgment.STATUS_X_MAX; i++)
        {
            for (int j = 1; j <= Judgment.STATUS_Y_MAX; j++)
            {
                GameObject statusUI = statusUIPar.transform.Find(i.ToString()).transform.Find(j.ToString()).gameObject;
                int indexX = i;
                int indexY = j;
                statusUI.GetComponent<Button>().onClick.AddListener(() => StatusSelecting(indexX, indexY));
                //statusUI.GetComponent<Button>().onClick.AddListener(() => Debug.Log($"{statusUI.name}!!!"));
            }
        }
    }

    public void StatusSelecting(int x, int y)
    {
        print($"{x} {y}");
        GameObject statusUIPar = GameObject.Find("Canvas").transform.Find("Status").gameObject;
        GameObject statusUI = statusUIPar.transform.Find(x.ToString()).transform.Find(y.ToString()).gameObject;
        if (!selectedStatus[x, y])
        {
            if (selected_status_num < MAX_STATUS_SELECT)
            {
                Debug.Log($"Selecting {judgment.GetStatusName(x, y)} 선택");
                selectedStatus[x, y] = true;
                selected_status_num++;
            }
        }
        else
        {
            if (selected_status_num > 0)
            {
                Debug.Log($"Selecting {judgment.GetStatusName(x, y)} 취소");
                selectedStatus[x, y] = false;
                selected_status_num--;
            }
        }
    }

    public void StoreStat()
    {
        for (int i = 1; i <= Judgment.STATUS_X_MAX; i++)
        {
            for (int j = 1; j <= Judgment.STATUS_Y_MAX; j++)
            {
                if (selectedStatus[i, j])
                {
                   judgment.AddStat(i, j);
                }
            }
        }
        NextSelecting();
    }

    //이전 선택 특성을 비활성화 후 다음 선택 특성을 비활성화
    public void NextSelecting()
    {
        print($"{curSelecting}에서");
        string preSelectingName = curSelecting.ToString();
        curSelecting += 1;
        print($"{curSelecting}으로");

        if (curSelecting == SelectingStatus.END)
        {
            SceneManager.LoadScene("exploration");
            return;
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
    }
    
}
