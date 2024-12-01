using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BattleManager;
using static Judgment;

public class Enemy
{
    public string Name { get; private set; } //���� �̸�
    public string Tag { get; private set; }
    public int HP { get; private set; } //���� ü��
    private List<Debuff> debuffs;
    public int diceReduce = 0;
    public Judgment judgment;
    public int plot;
    public PlayerData playerData;
    public static int deamgReduce = 0;
    public event BattleEndedHandler OnBattleEnded;

    public void SetPlot()
    {
        plot = Random.Range(1, 6);
    }
    public Enemy(string name, int hp)
    {
        Name = name;
        HP = hp;
    }
    public Enemy(string tag)
    {
        Tag = tag;
    }
    public void AddDebuff(Debuff debuff)
    {
        debuffs.Add(debuff);
    }

    //�� �� ó�� �ڵ�
    public void EnemyTurn(int playerPlot)
    {
        // Judgment Manager ������Ʈ ��������, null üũ
        judgment = GameObject.Find("Judgement Manger")?.GetComponent<Judgment>();
        playerData = GameObject.Find("PlayerManager")?.GetComponent<PlayerData>();
        if (playerData == null)
        {
            Debug.LogError("PlayerData component not found!");
        }
        if (judgment == null)
        {
            Debug.LogError("Judgment Manager not found!");
            return;
        }


        // �ֻ��� ���Ұ� ���� ��� ó��
        if (diceReduce > 0)
        {
            judgment.diceReduce = diceReduce;
        }



        if (Mathf.Abs(plot - playerPlot) <= 1)
        {
            int dice = Random.Range(1, 13);

            if (judgment.SetJudgeResult("����", dice) >= Judgment.JudgeResult.Success)
            {
                playerData.Damaged(1);
                Debug.Log("�������� ����");
            }
            else
            {
                Debug.Log("�������� ����");
            }
            
            OnBattleEnded?.Invoke();

        }
        else if (Mathf.Abs(plot - playerPlot) == 2 && Mathf.Abs(plot - playerPlot) == 4)
        {
            int dice = Random.Range(1, 13);

            if (judgment.SetJudgeResult("���", dice) >= Judgment.JudgeResult.Success)
            {
                playerData.Damaged(2);
                Debug.Log("��ݰ��� ����");
            }
            else
            {
                Debug.Log("��ݰ��� ����");
            }
            
            OnBattleEnded?.Invoke();
        }
        else if (Mathf.Abs(plot - playerPlot) == 3)
        {
            int dice = Random.Range(1, 13);
            if (judgment.SetJudgeResult("����", dice) >= Judgment.JudgeResult.Success)
            {
                deamgReduce += 1;
                Debug.Log("���°�ȭ ����");
            }
            else
            {
                Debug.Log("���°�ȭ ����");
            }
            OnBattleEnded?.Invoke();

        }
        else
        {
            OnBattleEnded?.Invoke();
        }

        // ���� ��ġ�� ���� ����
        SetPlot();

        // ����� �α׷� ���� ��ġ ���
        Debug.Log($"Enemy: enemy plot:{plot}");
    }

    //������ �������� ��
    public void EnemyDamage(int damage)
    {
        HP -= damage - deamgReduce;
        if (HP <= 0)
        {
            HP = 0;
            DieEvent();
        }
    }

    //���ְ� ġ���� ��
    public void EnemyHeal(int heal)
    {
        HP += heal;
    }

    //���� �׾��� ���� ó��
    public void DieEvent()
    {
        Debug.Log($"{Name} óġ");
    }
}
