using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Judgment : MonoBehaviour
{
    // �ֻ��� ���� �ʵ�
    [SerializeField] public BodyDice BodyDice; // �ֻ��� ����� ����ϴ� BodyDice Ŭ���� ����

    public const int STATUS_X_MAX = 6; // ������ X ũ�� (��: ���� ���� ���� ũ��)
    public const int STATUS_Y_MAX = 9; // ������ Y ũ�� (��: ���� ���� ���� ũ��)
    public int DiceResult { get; private set; } // �ֻ��� ��� ��
    public int judgment_value { get; private set; } // ���� ���� ��
    public JudgeResult Result { get; private set; } // ���� ��� (Pumble, Fail, Success, Special)

    // ���� ����� ��Ÿ���� ������
    public enum JudgeResult
    {
        Pumble, // �ߺ� (�־��� ���)
        Fail,   // ����
        Success,// ����
        Special // ����� (Ư���� ����)
    }

    // ���� �̸��� ��ġ�� �����ϴ� ����ü
    public struct Status
    {
        public string name; // ������ �̸�
        public int x;       // X ��ǥ
        public int y;       // Y ��ǥ

        // ������
        public Status(string name, int x, int y) : this()
        {
            this.name = name;
            this.x = x;
            this.y = y;
        }
    };

    // ��� ������ �����ϴ� ����Ʈ
    public List<Status> statuses = new List<Status>();
    // Ȱ��ȭ�� ���� ����Ʈ
    public List<Status> availableStatuses = new List<Status>();
    // ��Ȱ��ȭ�� ���� ����Ʈ
    public List<Status> disavaiableStatuses = new List<Status>();
    // ���� ���� ����Ʈ
    public List<Status> havingStatuses = new List<Status>();

    public string LastJudgeStatName { get; private set; } // ���������� ������ ���� �̸�

    // Start �޼���
    void Start()
    {
        // ���ȸ��� ��ġ�� "stats.txt" ���Ͽ��� �о� ����Ʈ�� ����
        StreamReader sr = new StreamReader("Assets/Scripts/Judgment/stats.txt");
        while (sr.Peek() >= 0)
        {
            string line = sr.ReadLine();
            string[] lineSplit = line.Split(',');

            // �� ���ο��� ���� ����(name, x, y)�� ����
            Status stat = new Status();
            stat.name = lineSplit[0];
            stat.x = int.Parse(lineSplit[1]);
            stat.y = int.Parse(lineSplit[2]);

            statuses.Add(stat);
        }
        sr.Close();

        // �׽�Ʈ �ڵ�: ������ ���ȵ��� ���� ���·� ����
        havingStatuses.Add(SearchStatByName("���"));
        havingStatuses.Add(SearchStatByName("����"));
        havingStatuses.Add(SearchStatByName("���Լ�"));
        availableStatuses = havingStatuses.ToList(); // Ȱ��ȭ�� ���� ����Ʈ�� ���� ���� ����Ʈ�� �ʱ�ȭ
    }

    // Ư�� ��ǥ�� ������ ���� ���� ����Ʈ�� �߰�
    public void AddStat(int x, int y)
    {
        Status stat = GetStatus(x, y);
        havingStatuses.Add(stat);
    }

    // �̸����� ������ ã�� ��ȯ
    public Status SearchStatByName(string name)
    {
        for (int i = 0; i < statuses.Count; i++)
        {
            Status compare = statuses[i];
            if (compare.name == name)
            {
                return compare;
            }
        }

        return new Status("null", 0, 0); // ������ ã�� ���� ��� null ���� ��ȯ
    }

    // X, Y ��ǥ�� ������ ã�� ��ȯ
    public Status GetStatus(int x, int y)
    {
        for (int i = 0; i < statuses.Count; i++)
        {
            Status stat = statuses[i];
            if (stat.x == x && stat.y == y)
            {
                return stat;
            }
        }

        return new Status("null", 0, 0); // ������ ã�� ���� ��� null ���� ��ȯ
    }

    // X, Y ��ǥ�� ������ ã�� �̸��� ��ȯ
    public string GetStatusName(int x, int y)
    {
        for (int i = 0; i < statuses.Count; i++)
        {
            Status stat = statuses[i];
            if (stat.x == x && stat.y == y)
            {
                return stat.name;
            }
        }

        return "null"; // ������ ã�� ���� ��� "null" ��ȯ
    }

    // ���� ���� ����ϰ� judgment_value�� ����
    void GetJudgeNum(string name)
    {
        int judgeNum = 5; // �⺻ ���� ��
        Status judge_status = SearchStatByName(name);

        // ���� ���ȿ� �ش� ������ ���� ���
        if (availableStatuses.Contains(judge_status))
        {
            Debug.Log("GetJudgeNum: 5, Having Status");
            judgment_value = judgeNum; // ���� �� ����
            return;
        }
        // ���� ���ȿ� ���� ���, ���� ����� ������ �Ÿ��� ���� ���� ���� �߰�
        else
        {
            Status minDistStatus = statuses[0];
            int minDist = 999;

            // ���� ����� ������ ã�� ���� ���� ���
            for (int i = 0; i < availableStatuses.Count; i++)
            {
                Status cmp = (Status)availableStatuses[i];
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

    // ���� ���ȿ� ���� �ٽ� ����
    public void ReJudge()
    {
        BodyDice.RollDice(); // �ֻ��� �ٽ� ������
    }

    // �ֻ��� ���� ���� �̸��� ������� ���� ����� ��ȯ
    public JudgeResult Judge(string name, int dice)
    {
        LastJudgeStatName = name; // ������ ������ ���� �̸� ����
        GetJudgeNum(name); // ���� ���� �� ���

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

    // ü���� ���� ������ ��Ȱ��ȭ (�ǰ� ���� ���� ���� ��Ȱ��ȭ)
    public void DisableStat(HealthStat stat)
    {
        for (int i = availableStatuses.Count - 1; i >= 0; i--)
        {
            if (availableStatuses[i].x == (int)stat)
            {
                Debug.Log("����:" + availableStatuses[i].name);
                disavaiableStatuses.Add(availableStatuses[i]);
                availableStatuses.RemoveAt(i);
            }
        }
    }

    // ��Ȱ��ȭ�� ������ Ȱ��ȭ (�ǰ� ���� ȸ��)
    public void EnableStat(HealthStat stat)
    {
        for (int i = disavaiableStatuses.Count - 1; i >= 0; i--)
        {
            if (disavaiableStatuses[i].x == (int)stat)
            {
                Debug.Log("����:" + disavaiableStatuses[i].name);
                availableStatuses.Add(disavaiableStatuses[i]);
                disavaiableStatuses.RemoveAt(i);
            }
        }
    }

    // ��� ������ Ȱ��ȭ ���·� �ʱ�ȭ
    public void ResetStat()
    {
        availableStatuses = havingStatuses.ToList(); // ���� ���� ����Ʈ�� �ʱ�ȭ
    }

    // ���� ���Ȱ� ������ ������ ���
    public void PrintStat()
    {
        Debug.Log("��ü ����:");
        for (int i = 0; i < havingStatuses.Count; i++)
        {
            Debug.Log(havingStatuses[i].name); // ���� ���� ���
        }

        Debug.Log("������ ����");
        for (int i = 0; i < availableStatuses.Count; i++)
        {
            Debug.Log(availableStatuses[i].name); // ������ ���� ���
        }
    }

    // Update �޼��� (����� �� ����, �ʿ� �� �߰� ��� ���� ����)
    void Update()
    {

    }
}
