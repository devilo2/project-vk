using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour
{
    PlayerData playerData;

    int playerPlot = 1;

    BattleStatus curBattleStatus = BattleStatus.None;
    PlayerTurnStatus curPlayerTurnStatus = PlayerTurnStatus.Idle;
    EnemyTurnStatus curEnemyturnStatus = EnemyTurnStatus.EnemyTurn;

    int skillNum = 0;

    Enemy[] enemies = new Enemy[3]; 
    int enemyNum = 0;
    int enemyMax = 0;

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
        SkillUse,
        End
    }

    enum EnemyTurnStatus
    {
        EnemyTurn,
        End
    }


    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        curBattleStatus = BattleStatus.PlotSelect;

        //테스트 코드
        Enemy test_enemy = new Enemy("테스트 적");
        enemies[0] = test_enemy;
    }

    // Update is called once per frame
    void Update()
    {
        //키가 연속적으로 인식되므로 if문에서 키 입력을 false로 바꿔준다.
        enterKey = Input.GetKeyDown(KeyCode.Return); 
        escKey = Input.GetKeyDown(KeyCode.Escape);

        switch (curBattleStatus)
        {
            case BattleStatus.PlotSelect:
                if (enterKey)
                {
                    curBattleStatus = BattleStatus.PlayerTurn;
                    skillNum = 0;
                    curPlayerTurnStatus = PlayerTurnStatus.Idle;
                    Debug.Log($"BattleManager: 현재 플롯:{playerPlot}");
                    Debug.Log("플레이어턴");
                    enterKey = false;
                }
                break;
            case BattleStatus.PlayerTurn:
                if (curPlayerTurnStatus == PlayerTurnStatus.End)
                {
                    curBattleStatus = BattleStatus.EnemyTurn;
                    Debug.Log("적턴");
                }
                break;
            case BattleStatus.EnemyTurn:
                if(curEnemyturnStatus == EnemyTurnStatus.End)
                {
                    curBattleStatus = BattleStatus.PlayerTurn;
                    Debug.Log("플레이어턴");
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
        

        switch (curPlayerTurnStatus)
        {
            case PlayerTurnStatus.Idle:
                if (enterKey)
                {
                    curPlayerTurnStatus = PlayerTurnStatus.EnemySelect;
                    Debug.Log("EnemySelect로 이동");
                    enterKey = false;
                }

                break;

            case PlayerTurnStatus.EnemySelect:
                if (escKey)
                {
                    curPlayerTurnStatus = PlayerTurnStatus.Idle;
                    Debug.Log("Idle로 이동");
                    escKey = false;
                }
                if (enterKey)
                {
                    if (IsSkillCostUnderPlot())
                    {
                        curPlayerTurnStatus = PlayerTurnStatus.SkillUse;
                        Debug.Log("SkillUse로 이동");
                    }
                    else
                        Debug.Log("BattleManager: 코스트 부족");
                    enterKey = false;
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
                if (Input.GetKeyDown(KeyCode.W) && enemyNum > 0)
                {
                    enemyNum--;
                    Debug.Log($"BattleManager: cur enemy:{-1}");
                }

                if (Input.GetKeyDown(KeyCode.S) && enemyNum < enemyMax - 1)
                {
                    enemyNum++;
                    Debug.Log($"BattleManager: cur enemy:{-1}");
                }
                break;
            case PlayerTurnStatus.SkillUse:
                playerData.getSkill(skillNum).UseSkill(enemies[enemyNum]);
                curPlayerTurnStatus = PlayerTurnStatus.End;
                break;

        }
    }

    private void EnemyTurn()
    {
         
    }



    //스킬의 코스트 체크하는 함수
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
