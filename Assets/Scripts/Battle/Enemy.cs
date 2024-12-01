using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BattleManager;
using static Judgment;

public class Enemy
{
    public string Name { get; private set; } //적의 이름
    public string Tag { get; private set; }
    public int HP { get; private set; } //적의 체력
    private List<Debuff> debuffs;
    public int diceReduce = 0;
    public Judgment judgment;
    public int plot;
    public PlayerData playerData;
    public static int deamgReduce = 0;
    public event BattleEndedHandler OnBattleEnded;

    public void SetPlot()
    {
        plot = Random.Range(1, 6);
    }
    public Enemy(string name, int hp)
    {
        Name = name;
        HP = hp;
    }
    public Enemy(string tag)
    {
        Tag = tag;
    }
    public void AddDebuff(Debuff debuff)
    {
        debuffs.Add(debuff);
    }

    //적 턴 처리 코드
    public void EnemyTurn(int playerPlot)
    {
        // Judgment Manager 컴포넌트 가져오기, null 체크
        judgment = GameObject.Find("Judgement Manger")?.GetComponent<Judgment>();
        playerData = GameObject.Find("PlayerManager")?.GetComponent<PlayerData>();
        if (playerData == null)
        {
            Debug.LogError("PlayerData component not found!");
        }
        if (judgment == null)
        {
            Debug.LogError("Judgment Manager not found!");
            return;
        }


        // 주사위 감소가 있을 경우 처리
        if (diceReduce > 0)
        {
            judgment.diceReduce = diceReduce;
        }



        if (Mathf.Abs(plot - playerPlot) <= 1)
        {
            int dice = Random.Range(1, 13);

            if (judgment.SetJudgeResult("격투", dice) >= Judgment.JudgeResult.Success)
            {
                playerData.Damaged(1);
                Debug.Log("근접공격 성공");
            }
            else
            {
                Debug.Log("근접공격 실패");
            }
            
            OnBattleEnded?.Invoke();

        }
        else if (Mathf.Abs(plot - playerPlot) == 2 && Mathf.Abs(plot - playerPlot) == 4)
        {
            int dice = Random.Range(1, 13);

            if (judgment.SetJudgeResult("사격", dice) >= Judgment.JudgeResult.Success)
            {
                playerData.Damaged(2);
                Debug.Log("사격공격 성공");
            }
            else
            {
                Debug.Log("사격공격 실패");
            }
            
            OnBattleEnded?.Invoke();
        }
        else if (Mathf.Abs(plot - playerPlot) == 3)
        {
            int dice = Random.Range(1, 13);
            if (judgment.SetJudgeResult("방어마술", dice) >= Judgment.JudgeResult.Success)
            {
                deamgReduce += 1;
                Debug.Log("마력경화 성공");
            }
            else
            {
                Debug.Log("마력경화 실패");
            }
            OnBattleEnded?.Invoke();

        }
        else
        {
            OnBattleEnded?.Invoke();
        }

        // 적의 위치나 상태 설정
        SetPlot();

        // 디버그 로그로 적의 위치 출력
        Debug.Log($"Enemy: enemy plot:{plot}");
    }

    //적에게 데미지를 줌
    public void EnemyDamage(int damage)
    {
        HP -= damage - deamgReduce;
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
