using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    public int duration;
    public string name;
    protected Judgment judgment;

    public StatusEffect(int duration, string name)
    {
        this.duration = duration;
        this.name = name;
        judgment = GameObject.Find("Judgement Manger").GetComponent<Judgment>();
    }

    public virtual void reduceDuration()
    {
        duration--;
    }
    //�÷��̾��  ȿ�� ����
    public virtual void ApplyEffect(PlayerData playerData)
    {

    }

    //������ ȿ�� ����
    public virtual void ApplyEffect(Enemy enemy)
    {
        
    }
}

public class Debuff : StatusEffect
{
    public Debuff(int duration, string name) : base(duration, name)
    {
    }
}

public class PainfulWoundEffect : Debuff
{
    public PainfulWoundEffect(int duration) : base(duration, "���뽺���� ��ó")
    {
    }



    public override void ApplyEffect(PlayerData playerData)
    {
        reduceDuration();
    }

    public override void ApplyEffect(Enemy enemy)
    {

        Judgment.JudgeResult judgeResult = judgment.SetJudgeResult("�Ǳ�", Random.Range(2, 13));
        if(judgeResult <= Judgment.JudgeResult.Fail)
        {
            enemy.EnemyDamage(1);
        }
        else if(judgeResult >= Judgment.JudgeResult.Special)
        {
            duration = 0;
        }
        reduceDuration();
    }
}

public class OverchargeEffect : Debuff
{
    public OverchargeEffect(int duration) : base(duration, "������")
    {
    }

    public override void ApplyEffect(PlayerData playerData)
    {
        playerData.Damaged(2);
    }
}
