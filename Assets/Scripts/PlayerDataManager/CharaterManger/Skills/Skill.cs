using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
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

    public Skill(string name, int cost, SkillType type, string designatedAttribute, int range)
    {
        Name = name;
        Cost = cost;
        Type = type;
        DesignatedAttribute = designatedAttribute;
        Range = range;
    }

    // �⺻������ ��� ��ų�� UseSkill�� ����
    public virtual void UseSkill(Enemy enemy)
    {
        Debug.Log($"Using skill: {Name} on {enemy.Name}");
    }
}

public class MeleeAttack : Skill // ����: ������ ����(�Ϲ� ����)
{
    public MeleeAttack() : base("���� ����", 0, SkillType.Attack, "������", 1) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log($"Dealing 1 damage to {enemy}");
        // ���⼭ target�� ���� 1�� ���ظ� �ִ� ������ �߰�
    }
}

public class PainfulWound : Skill // ����: ���뽺���� ��ó
{
    public PainfulWound() : base("���뽺���� ��ó", 2, SkillType.Support, "��ȭ����", 1) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log($"Applying painful wound to {enemy}");
        // ���� ���� �߰� �� �ϸ��� ���� ����
    }
}
public class DeviceDestructionBomb : Skill // ����: ��ġ �ı� ��ź
{
    public DeviceDestructionBomb() : base("��ġ �ı� ��ź", 2, SkillType.Device, "��ġ����", 1) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log($"Target {enemy} will have their action roll reduced by 3 next turn.");
        // Ÿ���� �ൿ ������ ������ �ִ� ���� �߰�
        // ���� ���, �ൿ ���� ���� �����ϴ� �ý��ۿ��� �� ���� ó��
    }
}
public class EnergyEmitter : Skill // ����: ������ ������
{
    public EnergyEmitter() : base("������ ������", 2, SkillType.Attack, "���", 3) { }

    public override void UseSkill(Enemy enemy)
    {
        bool isGiant = enemy.Tag == "Giant"; ; // "�Ŵ�" �±װ� ���� ���
        int damage = isGiant ? 3 : 1;
        Debug.Log($"Firing energy emitter at {enemy}, dealing {damage} damage.");
        // ���ظ� �ִ� ���� �߰�
    }
}
public class SurvivalStrategy : Skill // �ΰ�: ���� ����
{
    public SurvivalStrategy() : base("���� ����", 3, SkillType.Support, "������", 3) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log("Survival Strategy activated. If HP is reduced to 1, no damage will be taken this turn.");
        // ü�� ���� �� �������� ���� ���� �߰�
        // ���� ���, ĳ������ HP ���¸� �����ϴ� �ý��ۿ��� ������ üũ
    }
}
public class MagicShield : Skill // �ΰ�: ���� ����
{
    public MagicShield() : base("���� ����", 1, SkillType.Device, "����", 1) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log("Magic Shield activated, reducing damage taken by 1 this turn.");
        // �ڽ��� �޴� ���ظ� ���ҽ�Ű�� ���� �߰�
    }
}
public class Swordsmanship : Skill // �ΰ�: ���˼�
{
    public Swordsmanship() : base("���˼�", 1, SkillType.Attack, "�˼�", 1) { }

    public override void UseSkill(Enemy enemy)
    {
        bool isSpecial = /* ������ '�����'�� ��� */ false;
        int damage = isSpecial ? 3 : 2;
        Debug.Log($"Swordsmanship used on {enemy}, dealing {damage} damage.");
        // ����� ������ ������ ��� ���� �߰�
    }
}
public class TrajectoryCalculation : Skill // ���丶��: ���� ���
{
    public TrajectoryCalculation() : base("���� ���", 3, SkillType.Device, "������ۼ�", 3) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log("Trajectory Calculation activated, increasing the accuracy of ranged attacks by 4 this turn.");
        // ����� ���� ��ų�� ������ ������Ű�� ���� �߰�
    }
}
public class Overcharge : Skill // ���丶��: ������
{
    public Overcharge() : base("������", 4, SkillType.Support, "ȸ������", 4) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log("Overcharge used, restoring HP to 6 but causing 2 HP loss every turn.");
        // ü�� ���� �� �� �� ü�� ���� ���� �߰�
    }
}
public class ThunderSpear : Skill // ���丶��: ��â(������Ǿ�)
{
    public ThunderSpear() : base("��â", 2, SkillType.Attack, "���", 4) { }

    public override void UseSkill(Enemy enemy)
    {
        bool canAttack = /* �Ÿ� ��� �� ���� �������� ���� */ true;
        if (canAttack)
        {
            Debug.Log($"Brain Spear used on {enemy}, dealing 3 damage.");
            // Ÿ�ٿ��� �������� �ִ� ���� �߰�
        }
        else
        {
            Debug.Log("Target is too close to attack.");
        }
    }
}
public class Beastification : Skill // ����: �߼�ȭ
{
    public Beastification() : base("�߼�ȭ", 2, SkillType.Support, "����ȭ", 2) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log("Beastification activated, increasing melee damage by 1 this turn.");
        // ���� ������ ���� ���� �߰�
    }
}
public class HuntingTime : Skill // ����: ����� �ð�
{
    public HuntingTime() : base("����� �ð�", 2, SkillType.Device, "�Ǳ�", 2) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log("Hunting Time activated, increasing the range of all skills by 1 this turn (except self-targeting skills).");
        // �����Ÿ� ���� ���� �߰� (�ڽſ��Ը� ����� ��ų ����)
    }
}
public class ClawStrike : Skill // ����: ������
{
    public ClawStrike() : base("������", 1, SkillType.Attack, "������", 2) { }

    public override void UseSkill(Enemy enemy)
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
}
