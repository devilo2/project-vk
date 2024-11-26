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
        //플레이어 데이터 초기화
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
        enemies[0] = new Enemy("적1", 10);
        enemies[1] = new Enemy("적2", 10);
        enemies[2] = new Enemy("적3", 10); 
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
                    Debug.Log($"BattleManager: 현재 플롯:{playerPlot}");
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
                        Debug.Log("코스트 부족!");
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
                // 플레이어가 A키를 누르면 이전 모드로 전환
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (curToolOrSkill > ToolOrSkill.Tool)
                    {
                        curToolOrSkill--;
                        Debug.Log(curToolOrSkill);
                    }
                }
                // 플레이어가 D키를 누르면 다음 모드로 전환
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (curToolOrSkill < ToolOrSkill.NextTurn)
                    {
                        curToolOrSkill++;
                        Debug.Log(curToolOrSkill);
                    }
                }
                

                // 스킬 선택 모드일 때의 동작
                if (curToolOrSkill == ToolOrSkill.Skill)
                {
                    // W키로 이전 스킬 선택 (0번 스킬이 아닐 경우)
                    if (Input.GetKeyDown(KeyCode.W) && skillNum > 0)
                    {
                        skillNum--;
                        Debug.Log($"BattleManager: cur skill:{playerData.getSkill(skillNum).Name}");
                        Debug.Log($"BattleManager: cur cost:{curCost}");
                    }
                    // S키로 다음 스킬 선택 (마지막 스킬이 아닐 경우)
                    if (Input.GetKeyDown(KeyCode.S) && skillNum < playerData.getSkillCount()-1) 
                    {
                        skillNum++;
                        Debug.Log($"BattleManager: cur skill:{playerData.getSkill(skillNum).Name}");
                        Debug.Log($"BattleManager: cur cost:{curCost}");
                    }
                }
                // 도구 선택 모드일 때의 동작
                if (curToolOrSkill == ToolOrSkill.Tool)
                {
                    // W키로 이전 도구 선택 (0번 도구가 아닐 경우)
                    if (Input.GetKeyDown(KeyCode.W) && toolNum > 0)
                    {
                        toolNum--;
                        Debug.Log($"BattleManager: cur tool:{toolNum}");
                    }
                    // S키로 다음 도구 선택 (마지막 도구가 아닐 경우)
                    if (Input.GetKeyDown(KeyCode.S) && toolNum < 2)
                    {
                        toolNum++;
                        Debug.Log($"BattleManager: cur tool:{toolNum}");
                    }
                }

                break;

            case PlayerTurnStatus.EnemySelect:
                // W키를 눌러서 이전 적 선택 (첫 번째 적이 아닐 경우)
                if (Input.GetKeyDown(KeyCode.W) && SelectedEnemyNum > 0)
                {
                    SelectedEnemyNum--;
                    Debug.Log($"BattleManager: cur enemy:{SelectedEnemyNum}");
                }

                // S키를 눌러서 다음 적 선택 (마지막 적이 아닐 경우)
                if (Input.GetKeyDown(KeyCode.S) && SelectedEnemyNum < enemyMax - 1)
                {
                    SelectedEnemyNum++;
                    Debug.Log($"BattleManager: cur enemy:{SelectedEnemyNum}");
                }
                break;

            case PlayerTurnStatus.Use:
                // 선택된 스킬을 가져와서 선택된 적에게 사용
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

        //판정씬을 대기
       yield return new WaitUntil(() => playerData.Judgeresult != Judgment.JudgeResult.None);

        //판정 후 결과에 따라 코스트 차감 및 스킬 사용
        curCost -= skill.Cost;
        if (playerData.Judgeresult == Judgment.JudgeResult.Success)
        {
            playerData.Judgeresult = Judgment.JudgeResult.None;
            skill.UseSkill(enemies[SelectedEnemyNum]);
            // 공격 스킬인 경우 턴 종료, 아닌 경우 대기 상태로 돌아감
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



    //스킬이 코스트 이하인지 체크
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

        // 스킬 효과 적용
        selectedSkill.UseSkill(enemies[SelectedEnemyNum]);
    }
    public void UseTool(int toolIndex)
    {
        // 선택된 도구에 따라 동작
        Debug.Log($"Using Tool at Index {toolIndex}");
        // 도구의 효과를 구현
    }

    public void EndPlayerTurn()
    {
        // 턴 종료 후 이벤트 호출
        OnBattleEnded?.Invoke(); // UIManager의 OnBattleEnded 호출
    }

    public int GetSkillCount()
    {
        return playerData.getSkillCount();
    }



}
