using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Judgment : MonoBehaviour
{
    // 주사위 관련 필드
    PlayerData playerData;

    public int DiceResult { get; private set; } // 주사위 결과 값
    public int judgment_value { get; private set; } // 판정 기준 값
    public JudgeResult Result { get; private set; } // 판정 결과 (Pumble, Fail, Success, Special)

    public int diceReduce = 0;
    // 판정 결과를 나타내는 열거형
    public enum JudgeResult
    {
        None, //없음
        Pumble, // 펌블 (최악의 결과)
        Fail,   // 실패
        Success,// 성공
        Special // 스페셜 (특수한 성공)
    }

    // 스탯 이름과 위치를 저장하는 구조체
    public string LastJudgeStatName { get; private set; } // 마지막으로 판정된 스탯 이름

    // Start 메서드
    void Start()
    {
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
    }


    // 판정 값을 계산하고 judgment_value에 저장
    void GetJudgeNum(string name)
    {
        int judgeNum = 5; // 기본 판정 값
        PlayerData.Status judge_status = playerData.SearchStatByName(name);

        // 가진 스탯에 해당 스탯이 있을 경우
        if (playerData.availableStatuses.Contains(judge_status))
        {
            Debug.Log("GetJudgeNum: 5, Having Status");
            judgment_value = judgeNum; // 판정 값 설정
            return;
        }
        // 가진 스탯에 없을 경우, 가장 가까운 스탯의 거리를 구해 판정 값에 추가
        else
        {
            PlayerData.Status minDistStatus = playerData.statuses[0];
            int minDist = 999;

            // 가장 가까운 스탯을 찾아 판정 값을 계산
            for (int i = 0; i < playerData.availableStatuses.Count; i++)
            {
                PlayerData.Status cmp = (PlayerData.Status)playerData.availableStatuses[i];
                int dist = Mathf.Abs(cmp.x - judge_status.x) + Mathf.Abs(cmp.y - judge_status.y);
                if (dist < minDist)
                {
                    minDist = dist;
                }
            }

            Debug.Log(string.Format("GetJudgeNum: +{0} not Having Status", minDist));

            judgment_value = judgeNum + minDist; // 기본 판정 값에 거리만큼 더함
        }
    }

    public void SetLastJudgeStatName(string name)
    {
        LastJudgeStatName = name;
    }
    // 주사위 값과 스탯 이름을 기반으로 판정 결과를 반환
    public JudgeResult SetJudgeResult(string name, int dice)
    {
        LastJudgeStatName = name; // 마지막 판정된 스탯 이름 저장
        GetJudgeNum(name); // 판정 기준 값 계산

        dice = dice - diceReduce;
        diceReduce = 0;
        DiceResult = dice; // 주사위 결과 저장

        // 판정 기준에 따른 결과 설정
        if (dice <= 2)
        {
            Result = JudgeResult.Pumble; // 주사위 값이 2 이하일 경우 펌블
        }
        else if (dice >= 12)
        {
            Result = JudgeResult.Special; // 주사위 값이 12 이상일 경우 스페셜
        }
        else if (dice >= judgment_value)
        {
            Result = JudgeResult.Success; // 주사위 값이 판정 기준 이상일 경우 성공
        }
        else
        {
            Result = JudgeResult.Fail; // 그 외의 경우 실패
        }

        return Result;
    }

    
    // Update 메서드 (현재는 빈 상태, 필요 시 추가 기능 구현 가능)
    void Update()
    {

    }
}