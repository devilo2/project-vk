using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//자가 수리형 동력 캡슐:  체력을 1점 회복하거나 상태이상을 1개 회복한다.
public class SelfRecoveryPowerCapsule : Tool
{
    public override void Use()
    {
        PlayerData playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        playerData.RandomHeal(1);
    }
}
