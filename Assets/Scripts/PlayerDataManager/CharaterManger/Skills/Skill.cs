using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Skill
{
    public enum SkillType { Attack, Support, }
    public string Name {  get; private set; } 
    public int Cost { get; private set; }
    public SkillType type { get; private set; }
    public Judgment.Status status { get; private set; }
    public Image image {  get; private set; }
    // Start is called before the first frame update

    public Skill(string name, int cost, SkillType type, Judgment.Status status)
    {
        Name = name;
        Cost = cost;
        this.type = type;
        this.status = status;
    }

    public virtual void UseSkill(Enemy enemy)
    {
        Debug.Log($"Skill: {Name} »ç¿ë");
    }
}
