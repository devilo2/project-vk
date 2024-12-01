using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class numberofhealingcapsule : MonoBehaviour
{
    public PlayerData playerData;
    public Text num_heal_capsule;
    int heal_capsule;
    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        heal_capsule = PlayerData.SelfRecoveryPowerCapsule;
    }

    // Update is called once per frame
    void Update()
    {
        if (heal_capsule >= 0)
        {
            num_heal_capsule.text = heal_capsule.ToString();
        }
    }
}
