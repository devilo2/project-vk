using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class Judgment : MonoBehaviour
{

    //판정결과
    private enum JudgeResult
    {
        Pumble,
        Fail,
        Success,
        Special
    }



    //스텟이름과 표상에서의 위치를 표현하는 구조체
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

    //모든 스탯을 저장한 배열
    private ArrayList statuses = new ArrayList();
    //가진 스탯
    private ArrayList havingStatuses = new ArrayList();


    // Start is called before the first frame update
    void Start()
    {
        //스탯명, 위치가 적힌 파일에서 읽어들여 리스트에 저장
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


        //테스트 코드
        havingStatuses.Add(SearchStat("포술"));


        for (int i = 0; i < havingStatuses.Count; i++)
        {
            Debug.Log(((Status)havingStatuses[i]).name);
        }
    }

    //스탯을 이름으로 찾아 반환
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

    //판정치 계산, name: 판정할 스탯 이름
    int GetJudgeNum(string name)
    {
        int judgeNum = 5;
        Status judge_status = SearchStat(name);

        //가진 스탯에 있을 경우
        if (havingStatuses.Contains(judge_status))
        {
            Debug.Log("GetJudgeNum: 5, Having Status");
            return judgeNum;
        }
        //없을 경우 판정치 계산
        else
        {
            Status minDistStatus = (Status)statuses[0];
            int minDist = 999;

            //표애서 가장 가까운 스탯의 거리를 구해 기본치에 더한다..
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

    //판정치 결정
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

    //판정 테스트
    void Judge_print(string name)
    {
        JudgeResult result = Judge(GetJudgeNum(name));
        if (result == JudgeResult.Success)
        {
            Debug.Log("성공!!!!");
        }
        else if (result == JudgeResult.Fail)
        {
            Debug.Log("실패!!!");
        }
        else if (result == JudgeResult.Special)
        {
            Debug.Log("스페셜!!!");
        }
        else
        {
            Debug.Log("펌블!!!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Judge_print("포술");
            Judge_print("비행술");
            Judge_print("지형점령");
        }
    }
}
