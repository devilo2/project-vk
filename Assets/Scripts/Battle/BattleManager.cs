using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    PlayerData playerData;

    int playerPlot = 1;

    BattleStatus curBattleStatus = BattleStatus.None;
    PlayerTurnStatus curPlayerTurnStatus = PlayerTurnStatus.Idle;

    int skillNum = 0;
    int toolNum = 0;

    ToolOrSkill curToolOrSkill = ToolOrSkill.Tool;

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

    enum ToolOrSkill
    {
        Tool,
        Skill
    }

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
                    Debug.Log($"BattleManager: ���� �÷�:{playerPlot}");
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
                        Debug.Log("BattleManager: �ڽ�Ʈ ����");
                    escKey = false;
                }
                break;
        }


        

        switch(curPlayerTurnStatus)
        {
            
            case PlayerTurnStatus.Idle:
                // �÷��̾ AŰ�� ������ ���� ���� ���� ��ȯ
                if (Input.GetKeyDown(KeyCode.A))
                    curToolOrSkill = ToolOrSkill.Tool;
                // �÷��̾ DŰ�� ������ ��ų ���� ���� ��ȯ
                if (Input.GetKeyDown(KeyCode.D))
                    curToolOrSkill = ToolOrSkill.Skill;

                // ��ų ���� ����� ���� ����
                if (curToolOrSkill == ToolOrSkill.Skill)
                {
                    // WŰ�� ���� ��ų ���� (0�� ��ų�� �ƴ� ���)
                    if (Input.GetKeyDown(KeyCode.W) && skillNum > 0)
                    {
                        skillNum--;
                        Debug.Log($"BattleManager: cur skill:{playerData.getSkill(skillNum).Name}");
                    }
                    // SŰ�� ���� ��ų ���� (������ ��ų�� �ƴ� ���)
                    if (Input.GetKeyDown(KeyCode.S) && skillNum < playerData.getSkillCount()-1) 
                    {
                        skillNum++;
                        Debug.Log($"BattleManager: cur skill:{playerData.getSkill(skillNum).Name}");
                    }
                }
                // ���� ���� ����� ���� ����
                if (curToolOrSkill == ToolOrSkill.Tool)
                {
                    // WŰ�� ���� ���� ���� (0�� ������ �ƴ� ���)
                    if (Input.GetKeyDown(KeyCode.W) && toolNum > 0)
                    {
                        toolNum--;
                        Debug.Log($"BattleManager: cur tool:{toolNum}");
                    }
                    // SŰ�� ���� ���� ���� (������ ������ �ƴ� ���)
                    if (Input.GetKeyDown(KeyCode.S) && toolNum < 2)
                    {
                        toolNum++;
                        Debug.Log($"BattleManager: cur tool:{toolNum}");
                    }
                }
                break;

            case PlayerTurnStatus.EnemySelect:
                // WŰ�� ������ ���� �� ���� (ù ��° ���� �ƴ� ���)
                if (Input.GetKeyDown(KeyCode.W) && SelectedEnemyNum > 0)
                {
                    SelectedEnemyNum--;
                    Debug.Log($"BattleManager: cur enemy:{SelectedEnemyNum}");
                }

                // SŰ�� ������ ���� �� ���� (������ ���� �ƴ� ���)
                if (Input.GetKeyDown(KeyCode.S) && SelectedEnemyNum < enemyMax - 1)
                {
                    SelectedEnemyNum++;
                    Debug.Log($"BattleManager: cur enemy:{SelectedEnemyNum}");
                }
                break;

            case PlayerTurnStatus.Use:
                // ���õ� ��ų�� �����ͼ� ���õ� ������ ���
                SceneManager.LoadScene("Judgment", LoadSceneMode.Additive);
                Skill skill = playerData.getSkill(skillNum);
                //skill.DesignatedAttribute;
                skill.UseSkill(enemies[SelectedEnemyNum]);
                
                // ���� ��ų�� ��� �� ����, �ƴ� ��� ��� ���·� ���ư�
                if (skill.Type == Skill.SkillType.Attack)
                    curPlayerTurnStatus = PlayerTurnStatus.End;
                else
                    curPlayerTurnStatus = PlayerTurnStatus.Idle;
                break;

        }
    }

    private void EnemyTurn()
    {
         
    }



    //��ų�� �ڽ�Ʈ �������� üũ
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
