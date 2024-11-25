using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Skill
{
    public enum SkillType { Attack, Support, Device } // 스킬의 타입
    public string Name { get; private set; } // 스킬명
    public int Cost { get; private set; } // 스킬 코스트
    public SkillType Type { get; private set; } // 스킬의 타입 정의
    public string DesignatedAttribute { get; private set; } // 스킬의 세부 특기
    public int Range { get; private set; } // 스킬의 사정거리
    public Image SkillImage { get; private set; } // 스킬의 이미지

    public Skill(string name, int cost, SkillType type, string designatedAttribute, int range)
    {
        Name = name;
        Cost = cost;
        Type = type;
        DesignatedAttribute = designatedAttribute;
        Range = range;
    }

    // 기본적으로 모든 스킬은 UseSkill을 가짐
    public virtual void UseSkill(Enemy enemy)
    {
        Debug.Log($"Using skill: {Name} on {enemy.Name}");
    }
}

public class MeleeAttack : Skill // 공통: 접근전 공격(일반 공격)
{
    public MeleeAttack() : base("근접 공격", 0, SkillType.Attack, "격투술", 1) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log($"Dealing 1 damage to {enemy}");
        // 여기서 target에 대해 1의 피해를 주는 로직을 추가
    }
}

public class PainfulWound : Skill // 범용: 고통스러운 상처
{
    public PainfulWound() : base("고통스러운 상처", 2, SkillType.Support, "강화마술", 1) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log($"Applying painful wound to {enemy}");
        // 출혈 상태 추가 및 턴마다 피해 로직
    }
}
public class DeviceDestructionBomb : Skill // 범용: 장치 파괴 폭탄
{
    public DeviceDestructionBomb() : base("장치 파괴 폭탄", 2, SkillType.Device, "장치마술", 1) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log($"Target {enemy} will have their action roll reduced by 3 next turn.");
        // 타겟의 행동 판정에 영향을 주는 로직 추가
        // 예를 들어, 행동 판정 값을 추적하는 시스템에서 값 감소 처리
    }
}
public class EnergyEmitter : Skill // 범용: 에너지 방출포
{
    public EnergyEmitter() : base("에너지 방출포", 2, SkillType.Attack, "사격", 3) { }

    public override void UseSkill(Enemy enemy)
    {
        bool isGiant = enemy.Tag == "Giant"; ; // "거대" 태그가 붙은 경우
        int damage = isGiant ? 3 : 1;
        Debug.Log($"Firing energy emitter at {enemy}, dealing {damage} damage.");
        // 피해를 주는 로직 추가
    }
}
public class SurvivalStrategy : Skill // 인간: 생존 전략
{
    public SurvivalStrategy() : base("생존 전략", 3, SkillType.Support, "생존술", 3) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log("Survival Strategy activated. If HP is reduced to 1, no damage will be taken this turn.");
        // 체력 감소 시 데미지를 막는 로직 추가
        // 예를 들어, 캐릭터의 HP 상태를 추적하는 시스템에서 조건을 체크
    }
}
public class MagicShield : Skill // 인간: 마술 방패
{
    public MagicShield() : base("마술 방패", 1, SkillType.Device, "방어마술", 1) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log("Magic Shield activated, reducing damage taken by 1 this turn.");
        // 자신이 받는 피해를 감소시키는 로직 추가
    }
}
public class Swordsmanship : Skill // 인간: 마검술
{
    public Swordsmanship() : base("마검술", 1, SkillType.Attack, "검술", 1) { }

    public override void UseSkill(Enemy enemy)
    {
        bool isSpecial = /* 판정이 '스페셜'인 경우 */ false;
        int damage = isSpecial ? 3 : 2;
        Debug.Log($"Swordsmanship used on {enemy}, dealing {damage} damage.");
        // 스페셜 판정과 데미지 계산 로직 추가
    }
}
public class TrajectoryCalculation : Skill // 오토마톤: 궤적 계산
{
    public TrajectoryCalculation() : base("궤적 계산", 3, SkillType.Device, "기계조작술", 3) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log("Trajectory Calculation activated, increasing the accuracy of ranged attacks by 4 this turn.");
        // 사격전 공격 스킬의 판정을 증가시키는 로직 추가
    }
}
public class Overcharge : Skill // 오토마톤: 과충전
{
    public Overcharge() : base("과충전", 4, SkillType.Support, "회복마술", 4) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log("Overcharge used, restoring HP to 6 but causing 2 HP loss every turn.");
        // 체력 복구 및 매 턴 체력 감소 로직 추가
    }
}
public class ThunderSpear : Skill // 오토마톤: 뇌창(썬더스피어)
{
    public ThunderSpear() : base("뇌창", 2, SkillType.Attack, "사격", 4) { }

    public override void UseSkill(Enemy enemy)
    {
        bool canAttack = /* 거리 계산 후 공격 가능한지 여부 */ true;
        if (canAttack)
        {
            Debug.Log($"Brain Spear used on {enemy}, dealing 3 damage.");
            // 타겟에게 데미지를 주는 로직 추가
        }
        else
        {
            Debug.Log("Target is too close to attack.");
        }
    }
}
public class Beastification : Skill // 수인: 야수화
{
    public Beastification() : base("야수화", 2, SkillType.Support, "이형화", 2) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log("Beastification activated, increasing melee damage by 1 this turn.");
        // 근접 데미지 증가 로직 추가
    }
}
public class HuntingTime : Skill // 수인: 사냥의 시간
{
    public HuntingTime() : base("사냥의 시간", 2, SkillType.Device, "의기", 2) { }

    public override void UseSkill(Enemy enemy)
    {
        Debug.Log("Hunting Time activated, increasing the range of all skills by 1 this turn (except self-targeting skills).");
        // 사정거리 증가 로직 추가 (자신에게만 사용인 스킬 제외)
    }
}
public class ClawStrike : Skill // 수인: 할퀴기
{
    public ClawStrike() : base("할퀴기", 1, SkillType.Attack, "격투술", 2) { }

    public override void UseSkill(Enemy enemy)
    {
        bool isSpecial = /* 판정이 '스페셜'인 경우 */ false;
        int damage = 1;
        if (isSpecial)
        {
            Debug.Log("Special Claw Strike! Attacking again.");
            // 두 번 공격하는 로직 추가
        }
        Debug.Log($"Claw Strike used on {enemy}, dealing {damage} damage.");
        // 피해를 주는 로직 추가
    }
}
