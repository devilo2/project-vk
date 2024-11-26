using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    PlayerData playerData;

    public delegate void BattleEndedHandler();
    public event BattleEndedHandler OnBattleEnded;
    public int playerPlot = 1;
    int curCost = 0;


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
    bool waitJudgment = false;

    enum ToolOrSkill
    {
        Tool,
        Skill,
        NextTurn
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
        //�÷��̾� ������ �ʱ�ȭ
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        curBattleStatus = BattleStatus.PlotSelect;
        waitJudgment = false;
        enterKey = false;
        escKey = false;
        curPlayerTurnStatus = PlayerTurnStatus.Idle;
        curToolOrSkill = ToolOrSkill.Tool;
        skillNum = 0;
        toolNum = 0;
        SelectedEnemyNum = 0;
        energyAmplification = false;
        enemies[0] = new Enemy("��1", 10);
        enemies[1] = new Enemy("��2", 10);
        enemies[2] = new Enemy("��3", 10); 
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
                    curCost = playerPlot;
                    curPlayerTurnStatus = PlayerTurnStatus.Idle;
                    curBattleStatus = BattleStatus.PlayerTurn;
                    Debug.Log($"BattleManager: ���� �÷�:{playerPlot}");
                    enterKey = false;
                }
                break;
            case BattleStatus.PlayerTurn:
                if(curPlayerTurnStatus == PlayerTurnStatus.End)
                {
                    curBattleStatus = BattleStatus.EnemyTurn;
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
                    if (curToolOrSkill == ToolOrSkill.NextTurn)
                    {
                        curPlayerTurnStatus = PlayerTurnStatus.End;
                        enterKey = false;
                        break;
                    }

                    if(!IsSkillCostUnderPlot())
                    {
                        Debug.Log("�ڽ�Ʈ ����!");
                        curPlayerTurnStatus = PlayerTurnStatus.Idle;
                        break;
                    }
                    curPlayerTurnStatus = PlayerTurnStatus.EnemySelect;
                    enterKey = false;
                }
                break;

            case PlayerTurnStatus.EnemySelect:
                if (escKey) {
                    curPlayerTurnStatus = PlayerTurnStatus.Idle;
                    escKey = false;
                }
                if (enterKey) {
                    curPlayerTurnStatus = PlayerTurnStatus.Use;
                    enterKey = false;
                }
                break;
        }


        

        switch(curPlayerTurnStatus)
        {
            
            case PlayerTurnStatus.Idle:
                // �÷��̾ AŰ�� ������ ���� ���� ��ȯ
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (curToolOrSkill > ToolOrSkill.Tool)
                    {
                        curToolOrSkill--;
                        Debug.Log(curToolOrSkill);
                    }
                }
                // �÷��̾ DŰ�� ������ ���� ���� ��ȯ
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (curToolOrSkill < ToolOrSkill.NextTurn)
                    {
                        curToolOrSkill++;
                        Debug.Log(curToolOrSkill);
                    }
                }
                

                // ��ų ���� ����� ���� ����
                if (curToolOrSkill == ToolOrSkill.Skill)
                {
                    // WŰ�� ���� ��ų ���� (0�� ��ų�� �ƴ� ���)
                    if (Input.GetKeyDown(KeyCode.W) && skillNum > 0)
                    {
                        skillNum--;
                        Debug.Log($"BattleManager: cur skill:{playerData.getSkill(skillNum).Name}");
                        Debug.Log($"BattleManager: cur cost:{curCost}");
                    }
                    // SŰ�� ���� ��ų ���� (������ ��ų�� �ƴ� ���)
                    if (Input.GetKeyDown(KeyCode.S) && skillNum < playerData.getSkillCount()-1) 
                    {
                        skillNum++;
                        Debug.Log($"BattleManager: cur skill:{playerData.getSkill(skillNum).Name}");
                        Debug.Log($"BattleManager: cur cost:{curCost}");
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
                //skill.DesignatedAttribute;
                if(!waitJudgment)
                {
                    waitJudgment = true;
                    StartCoroutine(WaitForJudgment());
                }
                break;

               

        }
    }

    
    private IEnumerator WaitForJudgment()
    {
        Skill skill = playerData.getSkill(skillNum);
        SceneManager.LoadScene("Judgment", LoadSceneMode.Additive);

        //�������� ���
       yield return new WaitUntil(() => playerData.Judgeresult != Judgment.JudgeResult.None);

        //���� �� ����� ���� �ڽ�Ʈ ���� �� ��ų ���
        curCost -= skill.Cost;
        if (playerData.Judgeresult == Judgment.JudgeResult.Success)
        {
            playerData.Judgeresult = Judgment.JudgeResult.None;
            skill.UseSkill(enemies[SelectedEnemyNum]);
            // ���� ��ų�� ��� �� ����, �ƴ� ��� ��� ���·� ���ư�
            if (skill.Type == Skill.SkillType.Attack)
            {
                curPlayerTurnStatus = PlayerTurnStatus.End;
                yield return null;
            }
        }

        curPlayerTurnStatus = PlayerTurnStatus.Idle;
        waitJudgment = false;
    }

    private void EnemyTurn()
    {
        for (int i = 0; i < enemyMax; i++)
        {
            enemies[i].EnemyTurn(playerPlot);
        }
         curBattleStatus = BattleStatus.PlotSelect;
         playerPlot = 1;
    }



    //��ų�� �ڽ�Ʈ �������� üũ
    private Boolean IsSkillCostUnderPlot()
    {
        if (energyAmplification)
        {
            curCost += 2;
            energyAmplification = false;
        }

        if (playerData.getSkill(skillNum).Cost < curCost)
            return true;

        return false;

    }

    public void InitializeBattle(int selectedPlot)
    {
        playerPlot = selectedPlot;
        curBattleStatus = BattleStatus.PlayerTurn;
        Debug.Log($"BattleManager Initialized with Plot: {playerPlot}");
    }

    public void SelectToolMode()
    {
        curToolOrSkill = ToolOrSkill.Tool;
        Debug.Log("Tool Mode Selected");
    }

    public void SelectSkillMode()
    {
        curToolOrSkill = ToolOrSkill.Skill;
        Debug.Log("Skill Mode Selected");
    }

    public void UseSkill(int skillIndex)
    {
        Debug.Log($"Using Skill at Index {skillIndex}");
        Skill selectedSkill = playerData.getSkill(skillIndex);

        // ��ų ȿ�� ����
        selectedSkill.UseSkill(enemies[SelectedEnemyNum]);
    }
    public void UseTool(int toolIndex)
    {
        // ���õ� ������ ���� ����
        Debug.Log($"Using Tool at Index {toolIndex}");
        // ������ ȿ���� ����
    }

    public void EndPlayerTurn()
    {
        // �� ���� �� �̺�Ʈ ȣ��
        OnBattleEnded?.Invoke(); // UIManager�� OnBattleEnded ȣ��
    }

    public int GetSkillCount()
    {
        return playerData.getSkillCount();
    }



}
