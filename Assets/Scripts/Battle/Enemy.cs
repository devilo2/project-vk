using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public string Name { get; private set; } //���� �̸�
    public string Tag { get; private set; }
    public int HP { get; private set; } //���� ü��
    private List<Debuff> debuffs;
    public int diceReduce = 0;
    public Judgment judgment;

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
        judgment = GameObject.Find("Judgement Manger").GetComponent<Judgment>();
        if(diceReduce > 0)
        {
            judgment.diceReduce = diceReduce;
        }
        //������ �÷��� ������ �Ÿ��� ���� ��ų ���
        foreach(Debuff debuff in debuffs)
        {
            debuff.ApplyEffect(this);
            if(debuff.duration <= 0)
            {
                debuffs.Remove(debuff);
            }
        }
        int plot = Random.Range(1, 6);
        Debug.Log($"Enemy: enemy plot:{plot}");
    }

    //������ �������� ��
    public void EnemyDamage(int damage)
    {
        HP -= damage;
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
