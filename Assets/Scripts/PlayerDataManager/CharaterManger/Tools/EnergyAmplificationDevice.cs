using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//������ ���� ��ġ: ���� �� �� �Ͽ� ����� �� �ִ� �ڽ�Ʈ�� 2 ������Ų��.
public class EnergyAmplificationDevice : Tool
{
    BattleManager battleManager;
    public override void Use()
    {
        battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        battleManager.EnableEnergyAmplification();
    }
}
