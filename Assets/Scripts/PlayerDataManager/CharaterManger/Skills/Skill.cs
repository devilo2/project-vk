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
    public virtual void UseSkill(GameObject target)
    {
        Debug.Log($"Using skill: {Name} on {target.name}");
    }
}

public class MeleeAttack : Skill // ����: ������ ����(�Ϲ� ����)
{
    public MeleeAttack() : base("Approach Attack", 0, SkillType.Attack, "Fighting", 1) { }

    public override void UseSkill(GameObject target)
    {
        Debug.Log($"Dealing 1 damage to {target.name}");
        // ���⼭ target�� ���� 1�� ���ظ� �ִ� ������ �߰�
    }
}

public class PainfulWound : Skill // ����: ���뽺���� ��ó
{
    public PainfulWound() : base("Painful Wound", 2, SkillType.Support, "Magic", 1) { }

    public override void UseSkill(GameObject target)
    {
        Debug.Log($"Applying painful wound to {target.name}");
        // ���� ���� �߰� �� �ϸ��� ���� ����
    }
}
public class DeviceDestructionBomb : Skill // ����: ��ġ �ı� ��ź
{
    public DeviceDestructionBomb() : base("Device Destruction Bomb", 2, SkillType.Device, "Device Magic", 1) { }

    public override void UseSkill(GameObject target)
    {
        Debug.Log($"Target {target.name} will have their action roll reduced by 3 next turn.");
        // Ÿ���� �ൿ ������ ������ �ִ� ���� �߰�
        // ���� ���, �ൿ ���� ���� �����ϴ� �ý��ۿ��� �� ���� ó��
    }
}
public class EnergyEmitter : Skill // ����: ������ ������
{
    public EnergyEmitter() : base("Energy Emitter", 2, SkillType.Attack, "Shooting", 3) { }

    public override void UseSkill(GameObject target)
    {
        bool isGiant = target.CompareTag("Giant"); // "�Ŵ�" �±װ� ���� ���
        int damage = isGiant ? 3 : 1;
        Debug.Log($"Firing energy emitter at {target.name}, dealing {damage} damage.");
        // ���ظ� �ִ� ���� �߰�
    }
}
public class SurvivalStrategy : Skill // �ΰ�: ���� ����
{
    public SurvivalStrategy() : base("Survival Strategy", 3, SkillType.Support, "Survival Art", 3) { }

    public override void UseSkill(GameObject target)
    {
        Debug.Log("Survival Strategy activated. If HP is reduced to 1, no damage will be taken this turn.");
        // ü�� ���� �� �������� ���� ���� �߰�
        // ���� ���, ĳ������ HP ���¸� �����ϴ� �ý��ۿ��� ������ üũ
    }
}
public class MagicShield : Skill // �ΰ�: ���� ����
{
    public MagicShield() : base("Magic Shield", 1, SkillType.Device, "Defense Magic", 1) { }

    public override void UseSkill(GameObject target)
    {
        Debug.Log("Magic Shield activated, reducing damage taken by 1 this turn.");
        // �ڽ��� �޴� ���ظ� ���ҽ�Ű�� ���� �߰�
    }
}
public class Swordsmanship : Skill // �ΰ�: ���˼�
{
    public Swordsmanship() : base("Swordsmanship", 1, SkillType.Attack, "Sword Art", 1) { }

    public override void UseSkill(GameObject target)
    {
        bool isSpecial = /* ������ '�����'�� ��� */ false;
        int damage = isSpecial ? 3 : 2;
        Debug.Log($"Swordsmanship used on {target.name}, dealing {damage} damage.");
        // ����� ������ ������ ��� ���� �߰�
    }
}
public class TrajectoryCalculation : Skill // ���丶��: ���� ���
{
    public TrajectoryCalculation() : base("Trajectory Calculation", 3, SkillType.Device, "Mechanical Manipulation", 3) { }

    public override void UseSkill(GameObject target)
    {
        Debug.Log("Trajectory Calculation activated, increasing the accuracy of ranged attacks by 4 this turn.");
        // ����� ���� ��ų�� ������ ������Ű�� ���� �߰�
    }
}
public class Overcharge : Skill // ���丶��: ������
{
    public Overcharge() : base("Overcharge", 4, SkillType.Support, "Recovery Magic", 4) { }

    public override void UseSkill(GameObject target)
    {
        Debug.Log("Overcharge used, restoring HP to 6 but causing 2 HP loss every turn.");
        // ü�� ���� �� �� �� ü�� ���� ���� �߰�
    }
}
public class ThunderSpear : Skill // ���丶��: ��â(������Ǿ�)
{
    public ThunderSpear() : base("Brain Spear", 2, SkillType.Attack, "Shooting", 4) { }

    public override void UseSkill(GameObject target)
    {
        bool canAttack = /* �Ÿ� ��� �� ���� �������� ���� */ true;
        if (canAttack)
        {
            Debug.Log($"Brain Spear used on {target.name}, dealing 3 damage.");
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
    public Beastification() : base("Beastification", 2, SkillType.Support, "Metamorphosis", 2) { }

    public override void UseSkill(GameObject target)
    {
        Debug.Log("Beastification activated, increasing melee damage by 1 this turn.");
        // ���� ������ ���� ���� �߰�
    }
}
public class HuntingTime : Skill // ����: ����� �ð�
{
    public HuntingTime() : base("Hunting Time", 2, SkillType.Device, "Will Power", 2) { }

    public override void UseSkill(GameObject target)
    {
        Debug.Log("Hunting Time activated, increasing the range of all skills by 1 this turn (except self-targeting skills).");
        // �����Ÿ� ���� ���� �߰� (�ڽſ��Ը� ����� ��ų ����)
    }
}
public class ClawStrike : Skill // ����: ������
{
    public ClawStrike() : base("Claw Strike", 1, SkillType.Attack, "Martial Arts", 2) { }

    public override void UseSkill(GameObject target)
    {
        bool isSpecial = /* ������ '�����'�� ��� */ false;
        int damage = 1;
        if (isSpecial)
        {
            Debug.Log("Special Claw Strike! Attacking again.");
            // �� �� �����ϴ� ���� �߰�
        }
        Debug.Log($"Claw Strike used on {target.name}, dealing {damage} damage.");
        // ���ظ� �ִ� ���� �߰�
    }
}
