using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    PlayerData playerData;

    int playerPlot = 1;

    BattleStatus curBattleStatus = BattleStatus.None;
    PlayerTurnStatus curPlayerTurnStatus = PlayerTurnStatus.Idle;

    int skillNum = 0;

    const int enemyMax = 3;

    Enemy[] enemies = new Enemy[enemyMax]; 
    int SelectedEnemyNum = 0;

    Boolean energyAmplification = false;
    public void EnableEnergyAmplification()
    {
        energyAmplification = true; 
    }

    bool enterKey = false;
    bool escKey = false;

    enum BattleStatus
    {
        None = -1,
        PlotSelect,
        PlayerTurn,
        EnemyTurn
    }

    enum PlayerTurnStatus
    {
        Idle,
        EnemySelect,
        Use,
        End
    }


    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        curBattleStatus = BattleStatus.PlotSelect;
    }

    // Update is called once per frame
    void Update()
    {
        enterKey = Input.GetKeyDown(KeyCode.Return);
        escKey = Input.GetKeyDown(KeyCode.Escape);
        switch(curBattleStatus)
        {
            case BattleStatus.PlotSelect:
                if (enterKey)
                {
                    curBattleStatus = BattleStatus.PlayerTurn;
                    Debug.Log($"BattleManager: 현재 플롯:{playerPlot}");
                    enterKey = false;
                }
                break;
        }


        switch(curBattleStatus)
        {
            case BattleStatus.PlotSelect:
                PlotSelecing();
                break;
            case BattleStatus.PlayerTurn:
                PlayerTurn();
                break;
            case BattleStatus.EnemyTurn:
                EnemyTurn();
                break;
        }
    }

    private void PlotSelecing()
    {
        if (Input.GetKeyDown(KeyCode.A) && playerPlot > 1)
        {
            playerPlot--;
            Debug.Log($"BattleManager: battle plot:{playerPlot}");
        }

        if (Input.GetKeyDown(KeyCode.D) && playerPlot < 6)
        {
            playerPlot++;
            Debug.Log($"BattleManager: battle plot:{playerPlot}");
        }
    }

    private void PlayerTurn()
    {
        switch(curPlayerTurnStatus)
        {
            case PlayerTurnStatus.Idle:
                if (enterKey) {
                    curPlayerTurnStatus = PlayerTurnStatus.EnemySelect;
                    enterKey = false;
                }
                break;

            case PlayerTurnStatus.EnemySelect:
                if (enterKey) {
                    curPlayerTurnStatus = PlayerTurnStatus.Idle;
                    enterKey = false;
                }
                if (escKey) {
                    if (IsSkillCostUnderPlot())
                        curPlayerTurnStatus = PlayerTurnStatus.Use;
                    else
                        Debug.Log("BattleManager: 코스트 부족");
                    escKey = false;
                }
                break;
        }


        

        switch(curPlayerTurnStatus)
        {
            case PlayerTurnStatus.Idle:
                if (Input.GetKeyDown(KeyCode.A) && skillNum > 0)
                {
                    skillNum--;
                    Debug.Log($"BattleManager: cur skill:{playerData.getSkill(skillNum).Name}");
                }

                if (Input.GetKeyDown(KeyCode.D) && skillNum < playerData.getSkillCount()-1) 
                {
                    skillNum++;
                    Debug.Log($"BattleManager: cur skill:{playerData.getSkill(skillNum).Name}");
                }
                break;
                
            case PlayerTurnStatus.EnemySelect:
                if (Input.GetKeyDown(KeyCode.W) && SelectedEnemyNum > 0)
                {
                    SelectedEnemyNum--;
                    Debug.Log($"BattleManager: cur enemy:{-1}");
                }

                if (Input.GetKeyDown(KeyCode.S) && SelectedEnemyNum < enemyMax - 1)
                {
                    SelectedEnemyNum++;
                    Debug.Log($"BattleManager: cur enemy:{-1}");
                }
                break;

            case PlayerTurnStatus.Use:
                Skill skill = playerData.getSkill(skillNum);
                skill.UseSkill(enemies[SelectedEnemyNum]);
                if (skill.type == Skill.SkillType.Attack)
                    curPlayerTurnStatus = PlayerTurnStatus.End;
                break;

        }
    }

    private void EnemyTurn()
    {
         
    }



    //스킬이 코스트 이하인지 체크
    private Boolean IsSkillCostUnderPlot()
    {
        int maxCost = playerPlot;
        if (energyAmplification)
        {
            maxCost += 2;
            energyAmplification = false;
        }

        if (playerData.getSkill(skillNum).Cost < maxCost)
            return true;

        return false;

    }

}
