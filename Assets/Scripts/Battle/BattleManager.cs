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
                // 플레이어가 A키를 누르면 도구 선택 모드로 전환
                if (Input.GetKeyDown(KeyCode.A))
                    curToolOrSkill = ToolOrSkill.Tool;
                // 플레이어가 D키를 누르면 스킬 선택 모드로 전환
                if (Input.GetKeyDown(KeyCode.D))
                    curToolOrSkill = ToolOrSkill.Skill;

                // 스킬 선택 모드일 때의 동작
                if (curToolOrSkill == ToolOrSkill.Skill)
                {
                    // W키로 이전 스킬 선택 (0번 스킬이 아닐 경우)
                    if (Input.GetKeyDown(KeyCode.W) && skillNum > 0)
                    {
                        skillNum--;
                        Debug.Log($"BattleManager: cur skill:{playerData.getSkill(skillNum).Name}");
                    }
                    // S키로 다음 스킬 선택 (마지막 스킬이 아닐 경우)
                    if (Input.GetKeyDown(KeyCode.S) && skillNum < playerData.getSkillCount()-1) 
                    {
                        skillNum++;
                        Debug.Log($"BattleManager: cur skill:{playerData.getSkill(skillNum).Name}");
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
                SceneManager.LoadScene("Judgment", LoadSceneMode.Additive);
                Skill skill = playerData.getSkill(skillNum);
                //skill.DesignatedAttribute;
                skill.UseSkill(enemies[SelectedEnemyNum]);
                
                // 공격 스킬인 경우 턴 종료, 아닌 경우 대기 상태로 돌아감
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
