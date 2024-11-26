using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public string Name { get; private set; } //적의 이름
    public string Tag { get; private set; }
    public int HP { get; private set; } //적의 체력

    public Enemy(string name, int hp)
    {
        Name = name;
        HP = hp;
    }
    public Enemy(string tag)
    {
        Tag = tag;
    }
    
    //적 턴 처리 코드
    public void EnemyTurn(int playerPlot)
    {
        //임의의 플롯을 선택해 거리에 따라 스킬 사용
        int plot = Random.Range(1, 6);
        Debug.Log($"Enemy: enemy plot:{plot}");
    }

    //적에게 데미지를 줌
    public void EnemyDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            DieEvent();
        }
    }

    //적애게 치유를 함
    public void EnemyHeal(int heal)
    {
        HP += heal;
    }

    //적이 죽었을 때의 처리
    public void DieEvent()
    {
        Debug.Log($"{Name} 처치");
    }
}
