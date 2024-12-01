using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Skill
{
    public enum SkillType { Attack, Support, Device } // ��ų�� Ÿ��
    public string Name { get; private set; } // ��ų��
    public int Cost { get; private set; } // ��ų �ڽ�Ʈ
    public SkillType Type { get; private set; } // ��ų�� Ÿ�� ����
    public string DesignatedAttribute { get; private set; } // ��ų�� ���� Ư��
    public int Range { get; private set; } // ��ų�� �����Ÿ�
    public Image SkillImage { get; private set; } // ��ų�� �̹���
    public PlayerData playerData;

    public Skill(string name, int cost, SkillType type, string designatedAttribute, int range)
    {
        Name = name;
        Cost = cost;
        Type = type;
        DesignatedAttribute = designatedAttribute;
        Range = range;

    }
    public void InitializePlayerData()
    {
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
    }


    // �⺻������ ��� ��ų�� UseSkill�� ����
    public virtual void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        Debug.Log($"Using skill: {Name} on {enemy.Name}");
    }
}

public class MeleeAttack : Skill // ����: ������ ����(�Ϲ� ����)
{
    public MeleeAttack() : base("���� ����", 0, SkillType.Attack, "������", 1) { }

    public override void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        if (judgeResult == Judgment.JudgeResult.Success && judgeResult == Judgment.JudgeResult.Special)
        {
            enemy.EnemyDamage(1);
            Debug.Log($"Dealing 1 damage to {enemy}");
        }
        Debug.Log("Fail");
        
        // ���⼭ target�� ���� 1�� ���ظ� �ִ� ������ �߰�
    }
}

public class PainfulWound : Skill // ����: ���뽺���� ��ó
{
    public PainfulWound() : base("���뽺���� ��ó", 2, SkillType.Support, "��ȭ����", 1) { }

    public override void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        enemy.AddDebuff(new PainfulWoundEffect(100));
        Debug.Log($"Applying painful wound to {enemy}");
        // ���� ���� �߰� �� �ϸ��� ���� ����
    }
}
public class DeviceDestructionBomb : Skill // ����: ��ġ �ı� ��ź
{
    public DeviceDestructionBomb() : base("��ġ �ı� ��ź", 2, SkillType.Device, "��ġ����", 1) { }

    public override void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        if (judgeResult == Judgment.JudgeResult.Success && judgeResult == Judgment.JudgeResult.Special)
        {
            enemy.diceReduce += 3;
            Debug.Log($"Target {enemy} will have their action roll reduced by 3 next turn.");
        }
        Debug.Log("Fail");

        // Ÿ���� �ൿ ������ ������ �ִ� ���� �߰�
        // ���� ���, �ൿ ���� ���� �����ϴ� �ý��ۿ��� �� ���� ó��
    }
}
public class EnergyEmitter : Skill // ����: ������ ������
{
    public EnergyEmitter() : base("������ ������", 2, SkillType.Attack, "���ݸ���", 3) { }

    public override void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        if (judgeResult == Judgment.JudgeResult.Success && judgeResult == Judgment.JudgeResult.Special)
        {
            if (enemy.Tag == "Giant")
            {
                int damage = 3;
                enemy.EnemyDamage(damage);
                Debug.Log($"Firing energy emitter at {enemy}, dealing {damage} damage.");
            }
            else
            {
                enemy.EnemyDamage(1);
                Debug.Log($"Firing energy emitter at {enemy}, dealing 1 damage.");
            }
            Debug.Log("Fail");
        }
            
        
        // ���ظ� �ִ� ���� �߰�
    }
}
public class SurvivalStrategy : Skill // �ΰ�: ���� ����
{
    public SurvivalStrategy() : base("���� ����", 3, SkillType.Support, "������", 3) { }

    public override void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        if (judgeResult == Judgment.JudgeResult.Success && judgeResult == Judgment.JudgeResult.Special)
        {
            if (playerData.Health.Count == 1)
            {
                playerData.damagePass = true;
            }
            Debug.Log("Survival Strategy activated. If HP is reduced to 1, no damage will be taken this turn.");
            
        }
        Debug.Log("Fail");

        // ü�� ���� �� �������� ���� ���� �߰�
        // ���� ���, ĳ������ HP ���¸� �����ϴ� �ý��ۿ��� ������ üũ
    }
}
public class MagicShield : Skill // �ΰ�: ���� ����
{
    public MagicShield() : base("���� ����", 1, SkillType.Device, "����", 1) { }

    public override void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        if (judgeResult == Judgment.JudgeResult.Success && judgeResult == Judgment.JudgeResult.Special)
        {
            playerData.damageReduce += 1;
            Debug.Log("Magic Shield activated, reducing damage taken by 1 this turn.");
        }
            
        Debug.Log("Fail");
        // �ڽ��� �޴� ���ظ� ���ҽ�Ű�� ���� �߰�
    }
}
public class Swordsmanship : Skill // �ΰ�: ���˼�
{
    public Swordsmanship() : base("���˼�", 1, SkillType.Attack, "�˼�", 1) { }

    public override void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        if (judgeResult == Judgment.JudgeResult.Success && judgeResult == Judgment.JudgeResult.Special)
        {
            bool isSpecial = judgeResult == Judgment.JudgeResult.Special;
            int damage = isSpecial ? 3 : 2;
            enemy.EnemyDamage(damage);
            Debug.Log($"Swordsmanship used on {enemy}, dealing {damage} damage.");
        }
            
        Debug.Log("Fail");
        // ����� ������ ������ ��� ���� �߰�
    }
}
public class TrajectoryCalculation : Skill // ���丶��: ���� ���
{
    public TrajectoryCalculation() : base("���� ���", 3, SkillType.Device, "������ۼ�", 3) { }

    public override void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        if (judgeResult == Judgment.JudgeResult.Success && judgeResult == Judgment.JudgeResult.Special)
        {
            Judgment judgment = GameObject.Find("Judgement Manger").GetComponent<Judgment>();
            judgment.diceReduce -= 4;
            Debug.Log("Trajectory Calculation activated, increasing the accuracy of ranged attacks by 4 this turn.");
        }
            
        Debug.Log("Fail");
        // ����� ���� ��ų�� ������ ������Ű�� ���� �߰�
    }
}
public class Overcharge : Skill // ���丶��: ������
{
    public Overcharge() : base("������", 4, SkillType.Support, "ȸ������", 4) { }

    public override void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        if (judgeResult == Judgment.JudgeResult.Success && judgeResult == Judgment.JudgeResult.Special)
        {
            playerData.OverchargeUsed = true;
            playerData.RandomHeal(6);
            playerData.AddDebuff(new OverchargeEffect(100));
            Debug.Log("Overcharge used, restoring HP to 6 but causing 2 HP loss every turn.");
        }
           
        Debug.Log("Fail");
        // ü�� ���� �� �� �� ü�� ���� ���� �߰�
    }
}
public class ThunderSpear : Skill // ���丶��: ��â(������Ǿ�)
{
    private int lastUsedTurn = -1; // ������ ���� �� (-1�� ������ �ʾ����� �ǹ�)
    private int cooldownTurns = 1; // ��ų ��ٿ�(���� �Ͽ��� ��� �Ұ�)

    public ThunderSpear() : base("��â", 2, SkillType.Attack, "���", 4) { }

    public override void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        int currentTurn = BattleManager.battleturn; // BattleManager���� ���� �� ��������

        // ��ٿ� Ȯ��
        if (currentTurn <= lastUsedTurn + cooldownTurns)
        {
            Debug.Log("Thunder Spear is on cooldown. Can use again on next turn.");
            return;
        }

        // �Ÿ� ��� (�� �κ��� �������� ����)
        bool canAttack = true; // ��: �Ÿ� ��� ������ ���� ����
        if (canAttack)
        {
            enemy.EnemyDamage(3);
            Debug.Log($"Thunder Spear used on {enemy}, dealing 3 damage.");

            // ��ų ��� �� ���� �� ����
            lastUsedTurn = currentTurn;
        }
        else
        {
            Debug.Log("Target is too close to attack.");
        }
    }
}

public class Beastification : Skill
{
    public static bool beastMode = false; // ���� �߼�ȭ ���� ����
    private int activationTurn = -1; // �߼�ȭ�� Ȱ��ȭ�� ��

    public Beastification() : base("�߼�ȭ", 2, SkillType.Support, "����ȭ", 2) { }

    public override void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        // ���� �� ��������
        int currentTurn = BattleManager.battleturn;

        // �߼�ȭ Ȱ��ȭ
        beastMode = true;
        activationTurn = currentTurn;

        Debug.Log("Beastification activated, increasing melee damage by 1 this turn.");
    }

    public void UpdateBeastMode()
    {
        // ���� �� ��������
        int currentTurn = BattleManager.battleturn;

        // �߼�ȭ�� ����� ���� ������ ��Ȱ��ȭ
        if (beastMode && currentTurn > activationTurn)
        {
            beastMode = false;
            Debug.Log("Beastification effect has ended.");
        }
    }
}
// ����� �ð� ���� ���� ���� �Ϸ�
public class HuntingTime : Skill // ����: ����� �ð�
{
    public HuntingTime() : base("����� �ð�", 2, SkillType.Device, "�Ǳ�", 2) { }

    public override void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        Debug.Log("Hunting Time activated, increasing the range of all skills by 1 this turn (except self-targeting skills).");
        // �����Ÿ� ���� ���� �߰� (�ڽſ��Ը� ����� ��ų ����)
    }
}
public class ClawStrike : Skill // ����: ������
{
    public ClawStrike() : base("������", 1, SkillType.Attack, "������", 2) { }

    public override void UseSkill(Enemy enemy, Judgment.JudgeResult judgeResult)
    {
        if (judgeResult == Judgment.JudgeResult.Success && judgeResult == Judgment.JudgeResult.Special)
        {
            bool isSpecial = /* ������ '�����'�� ��� */ false;
            int damage = 1;
            if (isSpecial)
            {
                Debug.Log("Special Claw Strike! Attacking again.");
                // �� �� �����ϴ� ���� �߰�
            }
            Debug.Log($"Claw Strike used on {enemy}, dealing {damage} damage.");
            // ���ظ� �ִ� ���� �߰�
        }

        Debug.Log("Fail");
    }
}
