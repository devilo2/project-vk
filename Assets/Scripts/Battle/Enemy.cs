using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public string Name { get; private set; }
    public string Tag { get; private set; }
    public int HP { get; private set; }


    public Enemy(string name, int hp)
    {
        Name = name;
        HP = hp;
    }
    public Enemy(string tag)
    {
        Tag = tag;
    }
    // Start is called before the first frame update
    
    public void EnemyTurn(int playerPlot)
    {
        int plot = Random.Range(1, 6);
        Debug.Log($"Enemy: enemy plot:{plot}");
    }
}
