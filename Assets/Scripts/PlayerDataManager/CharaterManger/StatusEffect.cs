using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    public int duration;
    public string name;
    Judgment judgment;

    public StatusEffect(int duration, string name)
    {
        this.duration = duration;
        this.name = name;
        judgement = GameObject.Find("Judgement Manger").GetComponent<Judgment>();
    }

    //플레이어에게  효과 적용
    public virtual void ApplyEffect(PlayerData playerData)
    {

    }

    //적에게 효과 적용
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
    public PainfulWoundEffect(int duration, string name) : base(duration, name)
    {
    }

    public override void ApplyEffect(PlayerData playerData)
    {

    }

    public override void ApplyEffect(Enemy enemy)
    {
        Judgment.JudgeResult judgeResult = judgement.SetJudgeResult("의기", Random.Range(2, 13));
        if(judgeResult <= Judgment.JudgeResult.Fail)
        {
            enemy.Damaged(1);
        }
    }
}
