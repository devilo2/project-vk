using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEditor.Build;
using UnityEditorInternal;
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

            //judge.DisableStat(DamagedStat);
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

            //judge.EnableStat(HealStat);
        }
    }

    // Start is called before the first frame update
    //judgement 가져오기
    void Start()
    { 

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
