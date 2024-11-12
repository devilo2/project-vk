using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Judgment : MonoBehaviour
{
    public const int STATUS_X_MAX = 6;
    public const int STATUS_Y_MAX = 11;

    //�������
    public enum JudgeResult
    {
        Pumble,
        Fail,
        Success,
        Special
    }



    //�����̸��� ǥ�󿡼��� ��ġ�� ǥ���ϴ� ����ü
    public struct Status
    {
        public string name;
        public int x;
        public int y;

        public Status(string name, int x, int y) : this()
        {
            this.name = name;
            this.x = x;
            this.y = y;
        }
    };

    //��� ������ ������ �迭
    private List<Status> statuses = new List<Status>();
    //Ȱ��ȭ �� ����
    private List<Status> availableStatuses = new List<Status>();
    //��Ȱ��ȭ �� ����
    private List<Status> disavaiableStatuses = new List<Status>();
    //���� ����
    private List<Status> havingStatuses = new List<Status>(); 
    
    public string LastJudgeStatName { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        //���ȸ�, ��ġ�� ���� ���Ͽ��� �о�鿩 ����Ʈ�� ����
        StreamReader sr = new StreamReader("Assets/Scripts/Judgment/stats.txt");
        while (sr.Peek() >= 0)
        {
            string line = sr.ReadLine();
            string[] lineSplit = line.Split(',');

            Status stat = new Status();
            stat.name = lineSplit[0];
            stat.x = int.Parse(lineSplit[1]);
            stat.y = int.Parse(lineSplit[2]);

            statuses.Add(stat);
        }

        sr.Close();


        //�׽�Ʈ �ڵ�
        havingStatuses.Add(SearchStatByName("���"));
        havingStatuses.Add(SearchStatByName("����"));
        havingStatuses.Add(SearchStatByName("���Լ�"));
        availableStatuses = havingStatuses.ToList();
        
    }

    public void AddStat(int x, int y)
    { 
        Status stat = GetStatus(x, y);
        havingStatuses.Add(stat);
    }

    //������ �̸����� ã�� ��ȯ
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
       
        return new Status("null", 0, 0);
    }
    // x, y ��ǥ�� ���� ã��
    public Status GetStatus(int x, int y)
    {
        for(int i = 0; i < statuses.Count; i++)
        {
            Status stat = statuses[i];
            if(stat.x == x && stat.y == y)
            {
                return stat;
            }
        }

        return new Status("null", 0, 0);
    }

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

        return "null";
    }


    //����ġ ���, name: ������ ���� �̸�
    int GetJudgeNum(string name)
    {
        int judgeNum = 5;
        Status judge_status = SearchStatByName(name);

        //���� ���ȿ� ���� ���
        if (availableStatuses.Contains(judge_status))
        {
            Debug.Log("GetJudgeNum: 5, Having Status");
            return judgeNum;
        }
        //���� ��� ����ġ ���
        else
        {
            Status minDistStatus = statuses[0];
            int minDist = 999;

            //ǥ�ּ� ���� ����� ������ �Ÿ��� ���� �⺻ġ�� ���Ѵ�..
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
            return judgeNum + minDist;
        }
    }

    //�ֻ��� ������
    int RollDIce()
    {
        return Random.Range(2, 13);        
    }

    //����ġ ����
    JudgeResult Judge(int number)
    {
        int dice = RollDIce();
        if (dice <= 2)
        {
            return JudgeResult.Pumble;
        }
        else if(dice >= 12)
        {
            return JudgeResult.Special;
        }
        else if (dice >= number)
        {
            return JudgeResult.Success;
        }

        return JudgeResult.Fail;
    }


    //
    public JudgeResult GetJudgeResult(string name)
    {
        LastJudgeStatName = name;
        return Judge(GetJudgeNum(name));
    }

    //������
    public JudgeResult ReJudge()
    {
        return GetJudgeResult(LastJudgeStatName);
    }

    //���� �׽�Ʈ
    void Judge_print(string name)
    {
        JudgeResult result = GetJudgeResult(name);
        if (result == JudgeResult.Success)
        {
            Debug.Log("����!!!!");
        }
        else if (result == JudgeResult.Fail)
        {
            Debug.Log("����!!!");
        }
        else if (result == JudgeResult.Special)
        {
            Debug.Log("�����!!!");
        }
        else
        {
            Debug.Log("�ߺ�!!!");
        }

    }

    //ü�� ���� ���� ���� ��Ȱ��ȭ
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

    //���� ȸ��
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

    //���� �ٽ� Ȱ��ȭ
    public void ResetStat()
    {
        availableStatuses = havingStatuses.ToList();
    }

    //���� ���� ���
    public void PrintStat()
    {
        Debug.Log("��ü ����:");
        for (int i = 0; i < havingStatuses.Count; i++)
        {
            Debug.Log(havingStatuses[i].name);
        }

        Debug.Log("������ ����");
        for (int i = 0; i < availableStatuses.Count; i++)
        {
            Debug.Log(availableStatuses[i].name);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Judge_print("���");
            Judge_print("���������");
            Judge_print("����Ȱ��");
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            PrintStat();
        }
    }
}
