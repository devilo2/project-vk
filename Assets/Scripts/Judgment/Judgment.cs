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

    //판정결과
    public enum JudgeResult
    {
        Pumble,
        Fail,
        Success,
        Special
    }



    //스텟이름과 표상에서의 위치를 표현하는 구조체
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

    //모든 스탯을 저장한 배열
    private List<Status> statuses = new List<Status>();
    //활성화 된 스탯
    private List<Status> availableStatuses = new List<Status>();
    //비활성화 된 스탯
    private List<Status> disavaiableStatuses = new List<Status>();
    //가진 스탯
    private List<Status> havingStatuses = new List<Status>(); 
    
    public string LastJudgeStatName { get; private set; }


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
        havingStatuses.Add(SearchStatByName("사격"));
        havingStatuses.Add(SearchStatByName("운전"));
        havingStatuses.Add(SearchStatByName("잠입술"));
        availableStatuses = havingStatuses.ToList();
        
    }

    public void AddStat(int x, int y)
    { 
        Status stat = GetStatus(x, y);
        havingStatuses.Add(stat);
    }

    //스탯을 이름으로 찾아 반환
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
    // x, y 좌표로 스탯 찾기
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


    //판정치 계산, name: 판정할 스탯 이름
    int GetJudgeNum(string name)
    {
        int judgeNum = 5;
        Status judge_status = SearchStatByName(name);

        //가진 스탯에 있을 경우
        if (availableStatuses.Contains(judge_status))
        {
            Debug.Log("GetJudgeNum: 5, Having Status");
            return judgeNum;
        }
        //없을 경우 판정치 계산
        else
        {
            Status minDistStatus = statuses[0];
            int minDist = 999;

            //표애서 가장 가까운 스탯의 거리를 구해 기본치에 더한다..
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

    //주사위 굴리기
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


    //
    public JudgeResult GetJudgeResult(string name)
    {
        LastJudgeStatName = name;
        return Judge(GetJudgeNum(name));
    }

    //재판정
    public JudgeResult ReJudge()
    {
        return GetJudgeResult(LastJudgeStatName);
    }

    //판정 테스트
    void Judge_print(string name)
    {
        JudgeResult result = GetJudgeResult(name);
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

    //체력 깍인 줄의 스탯 비활성화
    public void DisableStat(HealthStat stat)
    {
        for (int i = availableStatuses.Count - 1; i >= 0; i--)
        {
            if (availableStatuses[i].x == (int)stat)
            {
                Debug.Log("삭제:" + availableStatuses[i].name);
                disavaiableStatuses.Add(availableStatuses[i]);
                availableStatuses.RemoveAt(i);
            }
        }
    }

    //스탯 회복
    public void EnableStat(HealthStat stat)
    {
        for (int i = disavaiableStatuses.Count - 1; i >= 0; i--)
        {
            if (disavaiableStatuses[i].x == (int)stat)
            {
                Debug.Log("복원:" + disavaiableStatuses[i].name);
                availableStatuses.Add(disavaiableStatuses[i]);
                disavaiableStatuses.RemoveAt(i);
            }
        }
    }

    //스탯 다시 활성화
    public void ResetStat()
    {
        availableStatuses = havingStatuses.ToList();
    }

    //가진 스탯 출력
    public void PrintStat()
    {
        Debug.Log("전체 스탯:");
        for (int i = 0; i < havingStatuses.Count; i++)
        {
            Debug.Log(havingStatuses[i].name);
        }

        Debug.Log("가능한 스탯");
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
            Judge_print("사격");
            Judge_print("비행기조종");
            Judge_print("지형활용");
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            PrintStat();
        }
    }
}
