using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public GameObject judgeObject;
    private Judgment judge;

    private List<HealthStat> Health = new List<HealthStat>{HealthStat.Tech, HealthStat.Somatic, HealthStat.Medicine, HealthStat.Mosul, HealthStat.Tactics, HealthStat.Magic};
    private List<HealthStat> DamagedHealth = new List<HealthStat>();

    public Species species { get; set; }
    private List<object> item;
    private List<object> keyItem;
    private Skill[] skill;

    public int getSkillCount()
    {
        return skill.Length;
    }

    public Skill getSkill(int index)
    {
        return (skill[index]);
    }

    //num�� Ƚ�� ��ŭ ������ ü���� ��´�.
    void Damaged(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (Health.Count <= 0)
                return;

            int select = Random.Range(0, Health.Count);
            HealthStat DamagedStat = Health[select];
            Debug.Log("ü�±���:" + DamagedStat.ToString());
            Health.RemoveAt(select);

            DamagedHealth.Add(DamagedStat);

            judge.DisableStat(DamagedStat);
        }
    }

    //num�� Ƚ�� ��ŭ ������ ü���� ȸ���Ѵ�.
    public void RandomHeal(int num)
    {

        for (int i = 0; i < num; i++)
        {
            if (DamagedHealth.Count <= 0)
                return;

            int select = Random.Range(0, DamagedHealth.Count);
            HealthStat HealStat = DamagedHealth[select];
            Debug.Log("ü��ȸ��:" + HealStat.ToString());
            DamagedHealth.RemoveAt(select);

            Health.Add(HealStat);

            judge.EnableStat(HealStat);
        }
    }

    // Start is called before the first frame update
    //judgement ��������
    void Start()
    {
        judge = judgeObject.GetComponent<Judgment>();

        //�׽�Ʈ �ڵ�
        Skill testSkill1 = new Skill("�׽�Ʈ ��ų", 2);
        Skill testSkill2 = new Skill("�׽�Ʈ ��ų2", 10);

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
