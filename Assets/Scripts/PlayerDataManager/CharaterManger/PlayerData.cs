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
    public List<Debuff> debuffs = new List<Debuff>();
    public bool OverchargeUsed = false;


    public bool damagePass = false;
    public int damageReduce = 0;

    //����
    public const int STATUS_X_MAX = 6; // ������ X ũ�� (��: ���� ���� ���� ũ��)Assets/Scripts/PlayerDataManager/CharaterManger/PlayerData.cs
    public const int STATUS_Y_MAX = 9; // ������ Y ũ�� (��: ���� ���� ���� ũ��)

    //����
    public static int EnableEnergy = 0;
    public static int ReturningGear = 1;
    public static int SelfRecoveryPowerCapsule = 0;


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

    const int enemyMax = 1; //�� �ִ� ��
    public Enemy[] enemies = new Enemy[enemyMax];

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
                Hpmanager.Hp -= 1;
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
                Hpmanager.Hp += 1;
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

    //num�� Ƚ�� ��ŭ ������ ü���� ��´�.
    public void Damaged(int num)
    {
        if(damagePass)
        {
            damagePass = false;
            return;
        }

        num = num - damageReduce;
        damageReduce = 0;

        for (int i = 0; i < num; i++)
        {
            if (Health.Count <= 0)
                return;

            int select = Random.Range(0, Health.Count);
            HealthStat DamagedStat = Health[select];
            Debug.Log("ü�±���:" + DamagedStat.ToString());
            Health.RemoveAt(select);

            DamagedHealth.Add(DamagedStat);

                DisableStat(DamagedStat);
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

            EnableStat(HealStat);
        }
    }

    // Start is called before the first frame update
    //judgement ��������
    void Start()
    { 
        // ���ȸ�� ��ġ�� "stats.txt" ���Ͽ��� �о� ����Ʈ�� ����
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
        havingStatuses.Add(SearchStatByName("���ݸ���"));
        havingStatuses.Add(SearchStatByName("��ȣ��"));
        havingStatuses.Add(SearchStatByName("�˼�"));
        havingStatuses.Add(SearchStatByName("����"));
        havingStatuses.Add(SearchStatByName("���Լ�"));
        havingStatuses.Add(SearchStatByName("�����"));
        availableStatuses = havingStatuses.ToList(); // Ȱ��ȭ�� ���� ����Ʈ�� ���� ���� ����Ʈ�� �ʱ�ȭ

        //�׽�Ʈ �ڵ�
        Skill meleeAttack = new MeleeAttack();
        Skill EnergyEmitter = new EnergyEmitter();
        Skill Swordsmanship = new Swordsmanship();
        Skill MagicShield = new MagicShield();
        Skill DeviceDestructionBomb = new DeviceDestructionBomb();

        skill = new Skill[] {meleeAttack, EnergyEmitter, Swordsmanship, MagicShield, DeviceDestructionBomb };

        enemies[0] = new Enemy("����", 7);
    }

    public void AddDebuff(Debuff debuff)
    {
        debuffs.Add(debuff);
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
