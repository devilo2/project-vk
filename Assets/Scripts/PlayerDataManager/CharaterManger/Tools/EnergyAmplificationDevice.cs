using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//에너지 증폭 장치: 전투 중 이 턴에 사용할 수 있는 코스트를 2 증가시킨다.
public class EnergyAmplificationDevice : Tool
{
    BattleManager battleManager;
    public override void Use()
    {
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        battleManager.EnableEnergyAmplification();
    }
}
