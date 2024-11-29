using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Judgment : MonoBehaviour
{
    // �ֻ��� ���� �ʵ�
    PlayerData playerData;

    public int DiceResult { get; private set; } // �ֻ��� ��� ��
    public int judgment_value { get; private set; } // ���� ���� ��
    public JudgeResult Result { get; private set; } // ���� ��� (Pumble, Fail, Success, Special)

    public int diceReduce = 0;
    // ���� ����� ��Ÿ���� ������
    public enum JudgeResult
    {
        None, //����
        Pumble, // �ߺ� (�־��� ���)
        Fail,   // ����
        Success,// ����
        Special // ����� (Ư���� ����)
    }

    // ���� �̸��� ��ġ�� �����ϴ� ����ü
    public string LastJudgeStatName { get; private set; } // ���������� ������ ���� �̸�

    // Start �޼���
    void Start()
    {
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
    }


    // ���� ���� ����ϰ� judgment_value�� ����
    void GetJudgeNum(string name)
    {
        int judgeNum = 5; // �⺻ ���� ��
        PlayerData.Status judge_status = playerData.SearchStatByName(name);

        // ���� ���ȿ� �ش� ������ ���� ���
        if (playerData.availableStatuses.Contains(judge_status))
        {
            Debug.Log("GetJudgeNum: 5, Having Status");
            judgment_value = judgeNum; // ���� �� ����
            return;
        }
        // ���� ���ȿ� ���� ���, ���� ����� ������ �Ÿ��� ���� ���� ���� �߰�
        else
        {
            PlayerData.Status minDistStatus = playerData.statuses[0];
            int minDist = 999;

            // ���� ����� ������ ã�� ���� ���� ���
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

            judgment_value = judgeNum + minDist; // �⺻ ���� ���� �Ÿ���ŭ ����
        }
    }

    public void SetLastJudgeStatName(string name)
    {
        LastJudgeStatName = name;
    }
    // �ֻ��� ���� ���� �̸��� ������� ���� ����� ��ȯ
    public JudgeResult SetJudgeResult(string name, int dice)
    {
        LastJudgeStatName = name; // ������ ������ ���� �̸� ����
        GetJudgeNum(name); // ���� ���� �� ���

        dice = dice - diceReduce;
        diceReduce = 0;
        DiceResult = dice; // �ֻ��� ��� ����

        // ���� ���ؿ� ���� ��� ����
        if (dice <= 2)
        {
            Result = JudgeResult.Pumble; // �ֻ��� ���� 2 ������ ��� �ߺ�
        }
        else if (dice >= 12)
        {
            Result = JudgeResult.Special; // �ֻ��� ���� 12 �̻��� ��� �����
        }
        else if (dice >= judgment_value)
        {
            Result = JudgeResult.Success; // �ֻ��� ���� ���� ���� �̻��� ��� ����
        }
        else
        {
            Result = JudgeResult.Fail; // �� ���� ��� ����
        }

        return Result;
    }

    
    // Update �޼��� (����� �� ����, �ʿ� �� �߰� ��� ���� ����)
    void Update()
    {

    }
}