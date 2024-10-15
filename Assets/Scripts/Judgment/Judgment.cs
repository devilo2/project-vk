using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class Judgment : MonoBehaviour
{

    //�������
    private enum JudgeResult
    {
        Pumble,
        Fail,
        Success,
        Special
    }



    //�����̸��� ǥ�󿡼��� ��ġ�� ǥ���ϴ� ����ü
    private struct Status
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
    private ArrayList statuses = new ArrayList();
    //���� ����
    private ArrayList havingStatuses = new ArrayList();


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
        havingStatuses.Add(SearchStat("����"));


        for (int i = 0; i < havingStatuses.Count; i++)
        {
            Debug.Log(((Status)havingStatuses[i]).name);
        }
    }

    //������ �̸����� ã�� ��ȯ
    Status SearchStat(string name)
    {
        for (int i = 0; i < statuses.Count; i++)
        {
            Status compare = (Status)statuses[i];
            if (compare.name == name)
            {
                return compare;
            }
        }
       
        return new Status("null", 0, 0);
    }

    //����ġ ���, name: ������ ���� �̸�
    int GetJudgeNum(string name)
    {
        int judgeNum = 5;
        Status judge_status = SearchStat(name);

        //���� ���ȿ� ���� ���
        if (havingStatuses.Contains(judge_status))
        {
            Debug.Log("GetJudgeNum: 5, Having Status");
            return judgeNum;
        }
        //���� ��� ����ġ ���
        else
        {
            Status minDistStatus = (Status)statuses[0];
            int minDist = 999;

            //ǥ�ּ� ���� ����� ������ �Ÿ��� ���� �⺻ġ�� ���Ѵ�..
            for (int i = 0; i < havingStatuses.Count; i++)
            {
                Status cmp = (Status)havingStatuses[i];
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

    //���� �׽�Ʈ
    void Judge_print(string name)
    {
        JudgeResult result = Judge(GetJudgeNum(name));
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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Judge_print("����");
            Judge_print("�����");
            Judge_print("��������");
        }
    }
}
