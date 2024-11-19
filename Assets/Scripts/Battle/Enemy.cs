using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public string Name { get; private set; }
    public int HP { get; private set; }
    public int plot { get; private set; }

    public Enemy(string name, int hp)
    {
        Name = name;
        HP = hp;
        plot = Random.Range(1, 7);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(HP <= 0)
        {
            Debug.Log($"{Name}이 죽었습니다.");
        }
    }

    public void Turn()
    {

    }
}
