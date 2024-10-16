using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public GameObject judgeObject;
    private Judgment judge;

    private List<HealthStat> Health = new List<HealthStat>{HealthStat.Tech, HealthStat.Somatic, HealthStat.Medicine, HealthStat.Mosul, HealthStat.Tactics, HealthStat.Magic};
    private List<HealthStat> DamagedHealth = new List<HealthStat>();

    private List<object> item;
    private List<object> status;
    private List<object> keyItem;
    private List<object> skill;
    private List<object> Lethalmove;


    void Damaged(int num)
    {
        for (int i = 0; i < num; i++)
        {
            int select = Random.Range(0, Health.Count);
            HealthStat DamagedStat = Health[select];
            Debug.Log("Ã¼·Â±ïÀÓ:" + DamagedStat.ToString());
            Health.RemoveAt(select);

            DamagedHealth.Add(DamagedStat);

            judge.DisableStat(DamagedStat);
        }
    }
        // Start is called before the first frame update
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
