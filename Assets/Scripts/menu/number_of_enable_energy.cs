using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class number_of_enable_energy : MonoBehaviour
{
    public PlayerData playerData;
    public Text num_enable_energy;
    int enable_energy;
    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        enable_energy = PlayerData.EnableEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if (enable_energy >= 0)
        {
            num_enable_energy.text = enable_energy.ToString();
        }
    }
}
