using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    PlayerData playerData;

    private int playerPlot = 1;

    private BattleStatus curBattleStatus = BattleStatus.None;
    private PlayerTurnStatus curPlayerTurnStatus = PlayerTurnStatus.Idle;

    private int skillNum = 0;

    private Enemy[] enemies; 
    private int enemyNum = 0;
    private int enemyMax = 0;

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
        SkillUse
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
        switch(curBattleStatus)
        {
            case BattleStatus.PlotSelect:
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    curBattleStatus = BattleStatus.PlayerTurn;
                    Debug.Log($"ÇöÀç ÇÃ·Ô:{playerPlot}");
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
            Debug.Log($"battle plot:{playerPlot}");
        }

        if (Input.GetKeyDown(KeyCode.D) && playerPlot < 6)
        {
            playerPlot++;
            Debug.Log($"battle plot:{playerPlot}");
        }
    }

    private void PlayerTurn()
    {
        switch(curPlayerTurnStatus)
        {
            case PlayerTurnStatus.Idle:
                if (Input.GetKeyDown(KeyCode.Return))
                    curPlayerTurnStatus = PlayerTurnStatus.EnemySelect;
                break;

            case PlayerTurnStatus.EnemySelect:
                if (Input.GetKeyDown(KeyCode.Return))
                    curPlayerTurnStatus = PlayerTurnStatus.Idle;
                if (Input.GetKeyDown(KeyCode.Escape))
                    curPlayerTurnStatus = PlayerTurnStatus.SkillUse;
                break;
        }


        

        switch(curPlayerTurnStatus)
        {
            case PlayerTurnStatus.Idle:
                if (Input.GetKeyDown(KeyCode.A) && skillNum > 0)
                {
                    skillNum--;
                    Debug.Log($"cur skill:{playerData.getSkill(skillNum).Name}");
                }

                if (Input.GetKeyDown(KeyCode.D) && skillNum < playerData.getSkillCount()-1) 
                {
                    skillNum++;
                    Debug.Log($"cur skill:{playerData.getSkill(skillNum).Name}");
                }
                break;
            case PlayerTurnStatus.EnemySelect:
                if (Input.GetKeyDown(KeyCode.W) && enemyNum > 0)
                {
                    skillNum--;
                    Debug.Log($"cur enemy:{-1}");
                }

                if (Input.GetKeyDown(KeyCode.S) && enemyNum < enemyMax - 1)
                {
                    skillNum++;
                    Debug.Log($"cur enemy:{-1}");
                }
                break;
        }
    }

    private void EnemyTurn()
    {
        
    }

}
