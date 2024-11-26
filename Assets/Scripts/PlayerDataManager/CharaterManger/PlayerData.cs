using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEditor.Build;
using UnityEditorInternal;
using System.Linq;
using System.IO;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public List<HealthStat> Health = new List<HealthStat>{HealthStat.Tech, HealthStat.Somatic, HealthStat.Medicine, HealthStat.Mosul, HealthStat.Tactics, HealthStat.Magic};
    public List<HealthStat> DamagedHealth = new List<HealthStat>();

    public Judgment.JudgeResult Judgeresult { get; set; }
    public Species species { get; set; }
    private Item[] items;
    private List<object> keyItems;
    private Skill[] skill;

    //스탯
    public const int STATUS_X_MAX = 6; // 스탯의 X 크기 (예: 스탯 맵의 가로 크기)Assets/Scripts/PlayerDataManager/CharaterManger/PlayerData.cs
    public const int STATUS_Y_MAX = 9; // 스탯의 Y 크기 (예: 스탯 맵의 세로 크기)

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
                Hpmanager.Hp -= 1;
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
                Hpmanager.Hp += 1;
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

    //------------------------------------------------
    public int getItemCount(int index)
    {
        return items[index].ItemCount;
    }

    public int getSkillCount()
    {
        return skill.Length;
    }

    public Skill getSkill(int index)
    {
        return (skill[index]);
    }

    //num의 횟수 만큼 랜덤한 체력을 깍는다.
    void Damaged(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (Health.Count <= 0)
                return;

            int select = Random.Range(0, Health.Count);
            HealthStat DamagedStat = Health[select];
            Debug.Log("체력깍임:" + DamagedStat.ToString());
            Health.RemoveAt(select);

            DamagedHealth.Add(DamagedStat);

                DisableStat(DamagedStat);
        }
    }

    //num의 횟수 만큼 랜덤한 체력을 회복한다.
    public void RandomHeal(int num)
    {

        for (int i = 0; i < num; i++)
        {
            if (DamagedHealth.Count <= 0)
                return;

            int select = Random.Range(0, DamagedHealth.Count);
            HealthStat HealStat = DamagedHealth[select];
            Debug.Log("체력회복:" + HealStat.ToString());
            DamagedHealth.RemoveAt(select);

            Health.Add(HealStat);

            EnableStat(HealStat);
        }
    }

    // Start is called before the first frame update
    //judgement 가져오기
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

        //테스트 코드
        Skill testSkill1 = new Skill("테스트 스킬", 2, Skill.SkillType.Attack, "사격", 3);
        Skill testSkill2 = new Skill("테스트 스킬2", 6, Skill.SkillType.Support, "방어마술", 1);


        skill = new Skill[] {testSkill1, testSkill2};
    }

        

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Damaged(1);
        }

        if (Input.GetKeyDown(KeyCode.Comma))
        {
            RandomHeal(1);
        }
    }
}
