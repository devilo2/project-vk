using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ڰ� ������ ���� ĸ��:  ü���� 1�� ȸ���ϰų� �����̻��� 1�� ȸ���Ѵ�.
public class SelfRecoveryPowerCapsule : Tool
{
    public override void Use()
    {
        PlayerData playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        playerData.RandomHeal(1);
    }
}
