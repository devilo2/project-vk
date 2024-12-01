using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class number_of_returning_gear : MonoBehaviour
{
    public PlayerData playerData;
    public Text num_returning_gear;
    int returning_gear;
    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        returning_gear = PlayerData.ReturningGear;
    }

    // Update is called once per frame
    void Update()
    {
        if (returning_gear >= 0)
        {
            num_returning_gear.text = returning_gear.ToString();
        }
    }
}
