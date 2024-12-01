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
    public EnemySpawn enemySpawn; // EnemySpawner �ν��Ͻ�
    PlayerData playerData; //�÷��̾� ������
    Judgment judgment;
    playerAni playerAnimator;


    public delegate void BattleEndedHandler();
    public event BattleEndedHandler OnBattleEnded;
    public int playerPlot = 1; //�÷�
    int curCost = 0; //���� �ڽ�Ʈ
    public static int battleturn = 0;

    BattleStatus curBattleStatus = BattleStatus.None; //���� ���� ����
    PlayerTurnStatus curPlayerTurnStatus = PlayerTurnStatus.Idle; //�÷��̾� �� ����

    public static int skillNum = 0; //��ų ��ȣ
    int toolNum = 0; //���� ��ȣ

    ToolOrSkill curToolOrSkill = ToolOrSkill.Tool; //���� ���� �Ǵ� ��ų ���

    const int enemyMax = 1; //�� �ִ� ��

    int SelectedEnemyNum = 0; //���õ� �� ��ȣ
    public Slider enemyHpSliders;  // ���� ü�� �� �����̴� �迭
    Boolean energyAmplification = false; //������ ���� ����

    //������ ���� Ȱ��ȭ
    public void EnableEnergyAmplification()
    {
        energyAmplification = true;
    }

    bool enterKey = false; //����Ű �ߺ��Է� ����
    bool escKey = false; //escŰ �ߺ��Է� ����
    bool waitJudgment = false; //�������� �������ΰ�


    //��ų, ����, ������ ����
    enum ToolOrSkill
    {
        Tool,
        Skill,
        NextTurn
    }

    //������ ���� ����
    enum BattleStatus
    {
        None = -1,
        PlotSelect,
        PlayerTurn,
        EnemyTurn
    }

    //�÷��̾��� ���� ����
    public enum PlayerTurnStatus
    {
        Idle, //��ų ����
        EnemySelect, //�� ���� 
        Use, //��ų ���
        End //�� ����
    }


    // Start is called before the first frame update
    void Start()
    {
        //�÷��̾� ������ �ʱ�ȭ
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        judgment = GameObject.Find("Judgement Manger").GetComponent<Judgment>();
        
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
        playerData.OverchargeUsed = false;
        
        for (int i = 0; i < enemyMax; i++)
        {
           playerData.enemies[i].SetPlot();
        }
        // �� ��ȯ
        enemySpawn.SpawnEnemies(playerData.enemies); // EnemySpawner�� ���� ���� ��ȯ
        if (enemyHpSliders != null)
        {
            UpdateEnemyHpBar();
        }
        battleturn += 1;
        Enemy.deamgReduce = 0;
    }


    // Update is called once per frame
    void Update()
    {
        enterKey = Input.GetKeyDown(KeyCode.Return);
        escKey = Input.GetKeyDown(KeyCode.Escape);

        //���� ���� ��ȯ
        switch (curBattleStatus)
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
                if (curPlayerTurnStatus == PlayerTurnStatus.End)
                {
                    judgment.diceReduce = 0;
                    curBattleStatus = BattleStatus.EnemyTurn;
                }
                break;
        }

        //���� ���¿� ���� ó��
        switch (curBattleStatus)
        {
            case BattleStatus.PlotSelect:
                PlotSelecing();
                break;
            case BattleStatus.PlayerTurn:
                PlayerTurn();
                UpdateEnemyHpBar();
                break;
            case BattleStatus.EnemyTurn:
                EnemyTurn();
                break;
        }
    }

    //ASŰ�� �÷�ġ ����
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

    //�÷��̾� �� ó��
    private void PlayerTurn()
    {
        //�÷��̾� �� ���� ����
        switch (curPlayerTurnStatus)
        {
            case PlayerTurnStatus.Idle:
                if (enterKey)
                {
                    //�� ����
                    if (curToolOrSkill == ToolOrSkill.NextTurn)
                    {
                        curPlayerTurnStatus = PlayerTurnStatus.End;
                        enterKey = false;
                        break;
                    }

                    //�ڽ�Ʈ ���� üũ
                    if (!IsSkillCostUnderPlot())
                    {
                        Debug.Log("�ڽ�Ʈ ����!");
                        curPlayerTurnStatus = PlayerTurnStatus.Idle;
                        break;
                    }

                    //�� ����
                    curPlayerTurnStatus = PlayerTurnStatus.EnemySelect;
                    enterKey = false;
                }
                break;

            case PlayerTurnStatus.EnemySelect:
                if (escKey)
                { //�������� ���ư���
                    curPlayerTurnStatus = PlayerTurnStatus.Idle;
                    escKey = false;
                }
                if (enterKey)
                { //��ų ���
                    curPlayerTurnStatus = PlayerTurnStatus.Use;
                    enterKey = false;
                }
                break;
        }

        //�÷��̾� �� ���¿� ���� ó��
        switch (curPlayerTurnStatus)
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
                    if (Input.GetKeyDown(KeyCode.S) && skillNum < playerData.getSkillCount() - 1)
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
                if (Mathf.Abs(playerData.enemies[SelectedEnemyNum].plot - playerPlot) <= playerData.getSkill(skillNum).Range)
                {
                    curPlayerTurnStatus = PlayerTurnStatus.Idle;
                    break;
                }
                if (!waitJudgment)
                {
                    waitJudgment = true;
                    StartCoroutine(WaitForJudgment());
                }
                break;



        }
    }

    //�������� �ҷ����� ��������� ��ٸ�
    public IEnumerator WaitForJudgment()
    {
        Skill skill = playerData.getSkill(skillNum);
        judgment.SetLastJudgeStatName(skill.DesignatedAttribute);
        SceneManager.LoadScene("Judgment", LoadSceneMode.Additive);


        //�������� ���
        yield return new WaitUntil(() => playerData.Judgeresult != Judgment.JudgeResult.None);

        //���� �� ����� ���� �ڽ�Ʈ ���� �� ��ų ���
        if (playerData.Judgeresult == Judgment.JudgeResult.Success || playerData.Judgeresult == Judgment.JudgeResult.Special)
        {
            curCost -= skill.Cost;
            skill.UseSkill(playerData.enemies[SelectedEnemyNum], playerData.Judgeresult); //��ų ���
            playerData.Judgeresult = Judgment.JudgeResult.None;

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

    //�� �� ó��
    public void EnemyTurn()
    {
        if (playerData.enemies == null || playerData.enemies.Length == 0)
        {
            Debug.LogError("enemies �迭�� �ʱ�ȭ���� �ʾҰų� ��� �ֽ��ϴ�!");
            return;
        }

        for (int i = 0; i < enemyMax; i++)
        {
            if (playerData.enemies[i] == null)
            {
                Debug.LogError($"enemies[{i}] ��ü�� null�Դϴ�!");
                continue;  // null�� ��ü�� �ѱ��, �ٸ� ��ü�� ��� ����
            }

            playerData.enemies[i].EnemyTurn(playerPlot);  // ���� �� ó��
        }

        curBattleStatus = BattleStatus.PlotSelect;
        playerPlot = 1;  // playerPlot�� 1�� ����
    }



    //��ų�� �ڽ�Ʈ �������� üũ
    private Boolean IsSkillCostUnderPlot()
    {
        //������ ���� Ȱ��ȭ �� �ڽ�Ʈ ����
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
        selectedSkill.UseSkill(playerData.enemies[SelectedEnemyNum], playerData.Judgeresult);
    }
    public void UseTool(int toolIndex)
    {
        // ���õ� ������ ���� ����
        Debug.Log($"Using Tool at Index {toolIndex}");
        // ������ ȿ���� ����
        if (toolIndex == 0)
        {
            PlayerData playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
            playerData.RandomHeal(1);
            PlayerData.SelfRecoveryPowerCapsule -= 1;

        }
        else if (toolIndex == 1)
        {
            EnableEnergyAmplification();
            PlayerData.EnableEnergy -= 1;
        }
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
    public void UpdateEnemyHpBar()
    {
        if (enemyHpSliders != null)
        {
            Slider currentEnemySlider = enemyHpSliders;
            Enemy selectedEnemy = playerData.enemies[SelectedEnemyNum];
            // ���� ���� ü�� ������ �����̴� �� ������Ʈ
            currentEnemySlider.value = (float)selectedEnemy.HP;
        }

    }
}
