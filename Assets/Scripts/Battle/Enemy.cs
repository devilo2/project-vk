using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public string Name { get; private set; } //���� �̸�
    public string Tag { get; private set; }
    public int HP { get; private set; } //���� ü��

    public Enemy(string name, int hp)
    {
        Name = name;
        HP = hp;
    }
    public Enemy(string tag)
    {
        Tag = tag;
    }
    
    //�� �� ó�� �ڵ�
    public void EnemyTurn(int playerPlot)
    {
        //������ �÷��� ������ �Ÿ��� ���� ��ų ���
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
