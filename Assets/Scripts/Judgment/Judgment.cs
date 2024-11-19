using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Judgment : MonoBehaviour
{
    // 주사위 관련 필드
    [SerializeField] Sprite[] diceSides; // 주사위 면 이미지 배열
    [SerializeField] Image diceImage1; // 첫 번째 주사위 이미지 UI
    [SerializeField] Image diceImage2; // 두 번쨰 주사위 이미지 UI
    private bool isRolling = false; // 주사위가 굴러가는 중인지 여부

    public const int STATUS_X_MAX = 6; //스탯X크기
    public const int STATUS_Y_MAX = 9; //스탯Y크기
    private int dice = 0; //현재 주사위값
    private int judgment_value; //판정기준치
    private JudgeResult judgeResult; //판정 결과

    public JudgeResult result { get { return judgeResult; } } //판정 결과값을 외부에 전달

    // 판정 결과 열거형
    public enum JudgeResult
    {
        Pumble,
        Fail,
        Success,
        Special
    }

    // 스탯 이름과 표상에서의 위치를 표현하는 구조체
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

    // UI 출력 필드
    [SerializeField] Text resultText; // 판정 결과값 UI 출력 필드
    [SerializeField] Text judgmentText; // 판정 (성공 등등) UI 출력

    // Start 메서드
    void Start()
    {
        // 주사위 이미지 초기화
        diceImage1 = diceImage1.GetComponent<Image>();
        diceImage2 = diceImage2.GetComponent<Image>();

        // 스탯명과 위치가 적힌 파일에서 읽어들여 리스트에 저장
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

        // 테스트 코드
        havingStatuses.Add(SearchStatByName("사격"));
        havingStatuses.Add(SearchStatByName("운전"));
        havingStatuses.Add(SearchStatByName("잠입술"));
        availableStatuses = havingStatuses.ToList();
    }


    //가진 스탯에 해당 좌표의 스탯 추가
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
        for (int i = 0; i < statuses.Count; i++)
        {
            Status stat = statuses[i];
            if (stat.x == x && stat.y == y)
            {
                return stat;
            }
        }

        return new Status("null", 0, 0);
    }

    // x, y 좌표로 스탯을 찾아 이름 반환
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


    //판정치 계산, name: 판정할 스탯 이름 judgement_value에 저장
    void GetJudgeNum(string name)
    {
        int judgeNum = 5;
        Status judge_status = SearchStatByName(name);

        //가진 스탯에 있을 경우
        if (availableStatuses.Contains(judge_status))
        {
            Debug.Log("GetJudgeNum: 5, Having Status");
            judgment_value = judgeNum;
            return;
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

            judgment_value = judgeNum + minDist;
        }
    }


    // 주사위 굴리기 메서드 (두 개의 주사위)
    public void RollDice()
    {
        if (!isRolling)
        {
            isRolling = true;
            StartCoroutine(RollTheDice());
        }
    }

    //화면에 주사위가 굴러가는 이펙트를 표시한다.
    private IEnumerator RollTheDice()
    {   
        int randomDiceSide1 = 0;
        int randomDiceSide2 = 0;

        // 주사위를 20번 굴려 무작위로 설정
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide1 = Random.Range(0, 6);
            randomDiceSide2 = Random.Range(0, 6);
            diceImage1.sprite = diceSides[randomDiceSide1]; // 첫 번째 주사위 면
            diceImage2.sprite = diceSides[randomDiceSide2]; // 두 번째 주사위 면
            yield return new WaitForSeconds(0.06f);
        }

        // 두 주사위 값 합산하여 결과 처리
        dice = randomDiceSide1 + randomDiceSide2 + 2; // +2는 주사위 값이 1부터 시작하므로 보정
        resultText.text = "주사위 합: " + dice.ToString();

        isRolling = false;
        Judge("사격");    
    }

    // UI 버튼을 클릭 시 호출할 메서드
    public void OnRollDiceButtonClicked()
    {
        RollDice(); // 주사위 굴리기 메서드를 호출
    }

    // 이전 스탯에 대해 다시 판정
    public void ReJudge()
    {
        RollDice();
    }


    // 특정 스탯에 대해 판정 결과를 출력하고 결과를 judgeResult에 저장
    void Judge(string name)
    {
        LastJudgeStatName = name;
        GetJudgeNum(name); //judgment_value 값 결정

        if (dice <= 2)
        {
            judgmentText.text = "펌블!!!";
            judgeResult = JudgeResult.Pumble;
            return;
        }
        else if (dice >= 12)
        {
            judgmentText.text = "스페셜!!!";
            judgeResult = JudgeResult.Special;
            return;
        }
        else if (dice >= judgment_value)
        {
            judgmentText.text = "성공!!!!";
            judgeResult = JudgeResult.Success;
            return;
        }

        judgmentText.text = "실패!!!";
        judgeResult = JudgeResult.Fail;
        return;
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

    //활성화된 스탯을 가진 스탯으로 초기화 한다.
    public void ResetStat()
    {
        availableStatuses = havingStatuses.ToList();
    }


    // 전체 스탯 출력
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

    // Update 메서드
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Judge("사격");
            Judge("비행기조종");
            Judge("지형활용");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            PrintStat();
        }
    }
}
