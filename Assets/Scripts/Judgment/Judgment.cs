using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Judgment : MonoBehaviour
{
    // 주사위 관련 필드
    [SerializeField] public BodyDice BodyDice; // 주사위 결과를 계산하는 BodyDice 클래스 참조

    public const int STATUS_X_MAX = 6; // 스탯의 X 크기 (예: 스탯 맵의 가로 크기)
    public const int STATUS_Y_MAX = 9; // 스탯의 Y 크기 (예: 스탯 맵의 세로 크기)
    public int DiceResult { get; private set; } // 주사위 결과 값
    public int judgment_value { get; private set; } // 판정 기준 값
    public JudgeResult Result { get; private set; } // 판정 결과 (Pumble, Fail, Success, Special)

    // 판정 결과를 나타내는 열거형
    public enum JudgeResult
    {
        Pumble, // 펌블 (최악의 결과)
        Fail,   // 실패
        Success,// 성공
        Special // 스페셜 (특수한 성공)
    }

    // 스탯 이름과 위치를 저장하는 구조체
    public struct Status
    {
        public string name; // 스탯의 이름
        public int x;       // X 좌표
        public int y;       // Y 좌표

        // 생성자
        public Status(string name, int x, int y) : this()
        {
            this.name = name;
            this.x = x;
            this.y = y;
        }
    };

    // 모든 스탯을 저장하는 리스트
    public List<Status> statuses = new List<Status>();
    // 활성화된 스탯 리스트
    public List<Status> availableStatuses = new List<Status>();
    // 비활성화된 스탯 리스트
    public List<Status> disavaiableStatuses = new List<Status>();
    // 가진 스탯 리스트
    public List<Status> havingStatuses = new List<Status>();

    public string LastJudgeStatName { get; private set; } // 마지막으로 판정된 스탯 이름

    // Start 메서드
    void Start()
    {
        // 스탯명과 위치를 "stats.txt" 파일에서 읽어 리스트에 저장
        StreamReader sr = new StreamReader("Assets/Scripts/Judgment/stats.txt");
        while (sr.Peek() >= 0)
        {
            string line = sr.ReadLine();
            string[] lineSplit = line.Split(',');

            // 각 라인에서 스탯 정보(name, x, y)를 추출
            Status stat = new Status();
            stat.name = lineSplit[0];
            stat.x = int.Parse(lineSplit[1]);
            stat.y = int.Parse(lineSplit[2]);

            statuses.Add(stat);
        }
        sr.Close();

        // 테스트 코드: 임의의 스탯들을 가진 상태로 설정
        havingStatuses.Add(SearchStatByName("사격"));
        havingStatuses.Add(SearchStatByName("운전"));
        havingStatuses.Add(SearchStatByName("잠입술"));
        availableStatuses = havingStatuses.ToList(); // 활성화된 스탯 리스트를 가진 스탯 리스트로 초기화
    }

    // 특정 좌표의 스탯을 가진 스탯 리스트에 추가
    public void AddStat(int x, int y)
    {
        Status stat = GetStatus(x, y);
        havingStatuses.Add(stat);
    }

    // 이름으로 스탯을 찾아 반환
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

        return new Status("null", 0, 0); // 스탯을 찾지 못한 경우 null 스탯 반환
    }

    // X, Y 좌표로 스탯을 찾아 반환
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

        return new Status("null", 0, 0); // 스탯을 찾지 못한 경우 null 스탯 반환
    }

    // X, Y 좌표로 스탯을 찾아 이름을 반환
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

        return "null"; // 스탯을 찾지 못한 경우 "null" 반환
    }

    // 판정 값을 계산하고 judgment_value에 저장
    void GetJudgeNum(string name)
    {
        int judgeNum = 5; // 기본 판정 값
        Status judge_status = SearchStatByName(name);

        // 가진 스탯에 해당 스탯이 있을 경우
        if (availableStatuses.Contains(judge_status))
        {
            Debug.Log("GetJudgeNum: 5, Having Status");
            judgment_value = judgeNum; // 판정 값 설정
            return;
        }
        // 가진 스탯에 없을 경우, 가장 가까운 스탯의 거리를 구해 판정 값에 추가
        else
        {
            Status minDistStatus = statuses[0];
            int minDist = 999;

            // 가장 가까운 스탯을 찾아 판정 값을 계산
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

            judgment_value = judgeNum + minDist; // 기본 판정 값에 거리만큼 더함
        }
    }

    // 이전 스탯에 대해 다시 판정
    public void ReJudge()
    {
        BodyDice.RollDice(); // 주사위 다시 굴리기
    }

    // 주사위 값과 스탯 이름을 기반으로 판정 결과를 반환
    public JudgeResult Judge(string name, int dice)
    {
        LastJudgeStatName = name; // 마지막 판정된 스탯 이름 저장
        GetJudgeNum(name); // 판정 기준 값 계산

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

    // 체력이 깎인 스탯을 비활성화 (건강 상태 관련 스탯 비활성화)
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

    // 비활성화된 스탯을 활성화 (건강 상태 회복)
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

    // 모든 스탯을 활성화 상태로 초기화
    public void ResetStat()
    {
        availableStatuses = havingStatuses.ToList(); // 가진 스탯 리스트로 초기화
    }

    // 가진 스탯과 가능한 스탯을 출력
    public void PrintStat()
    {
        Debug.Log("전체 스탯:");
        for (int i = 0; i < havingStatuses.Count; i++)
        {
            Debug.Log(havingStatuses[i].name); // 가진 스탯 출력
        }

        Debug.Log("가능한 스탯");
        for (int i = 0; i < availableStatuses.Count; i++)
        {
            Debug.Log(availableStatuses[i].name); // 가능한 스탯 출력
        }
    }

    // Update 메서드 (현재는 빈 상태, 필요 시 추가 기능 구현 가능)
    void Update()
    {

    }
}
