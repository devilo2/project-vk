using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Skill
{
    public string Name {  get; private set; } 
    public int Cost { get; private set; }
    public Image image {  get; private set; }
    // Start is called before the first frame update

    public Skill(string name, int cost)
    {
        Name = name;
        Cost = cost;
    }

    public virtual void UseSkill(Enemy enemy)
    {
        Debug.Log($"Skill: {Name} »ç¿ë");
    }
}
