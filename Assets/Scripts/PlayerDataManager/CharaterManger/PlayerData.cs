using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public GameObject judgeObject;
    private Judgment judge;

    private List<HealthStat> Health = new List<HealthStat>{HealthStat.Tech, HealthStat.Somatic, HealthStat.Medicine, HealthStat.Mosul, HealthStat.Tactics, HealthStat.Magic};
    private List<HealthStat> DamagedHealth = new List<HealthStat>();

    public Species species { get; set; }
    private List<object> item;
    private List<object> keyItem;
    private List<CommonSkill> commonSkill;
    private List<object> speciesSkill;


    //num�� Ƚ�� ��ŭ ������ ü���� ��´�.
    void Damaged(int num)
    {
        for (int i = 0; i < num; i++)
        {
            int select = Random.Range(0, Health.Count);
            HealthStat DamagedStat = Health[select];
            Debug.Log("ü�±���:" + DamagedStat.ToString());
            Health.RemoveAt(select);

            DamagedHealth.Add(DamagedStat);

            judge.DisableStat(DamagedStat);
        }
    }

    // Start is called before the first frame update
    //judgement ��������
    void Start()
    {
        judge = judgeObject.GetComponent<Judgment>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Damaged(1);
        }
    }
}