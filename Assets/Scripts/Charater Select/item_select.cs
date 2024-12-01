using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class item_select : MonoBehaviour
{
    public PlayerData playerData;
    public GameObject[] item;
    public GameObject[] select_item;
    const int MAX_ITEM_NUM = 2;
    public int selected_item_num = 0;
    public Button next_select_btn;

    // Start is called before the first frame update
    public void press_enable_energy()
    {
        if (selected_item_num == MAX_ITEM_NUM)
        {
            for (int i = 0; i < item.Length; i++)
            {
                item[i].GetComponent<Button>().interactable = false;
            }
            next_select_btn.interactable = true;
        }
        else
        {
            selected_item_num++;
            PlayerData.EnableEnergy += 1;
            print("Enable_Energy" + selected_item_num);
        }
        
    }
    public void press_returning_gear()
    {
        if (selected_item_num == MAX_ITEM_NUM)
        {
            for (int i = 0; i < item.Length; i++)
            {
                item[i].GetComponent<Button>().interactable = false;
            }
            next_select_btn.interactable = true;
        }
        else
        {
            selected_item_num++;
            PlayerData.ReturningGear += 1;
            print("Returning_Gear" + selected_item_num);
        }
    }
    public void press_self_recovery_power_capsule()
    {
        if (selected_item_num == MAX_ITEM_NUM)
        {
            for (int i = 0; i < item.Length; i++)
            {
                item[i].GetComponent<Button>().interactable = false;
            }
            next_select_btn.interactable = true;
        }
        else
        {
            selected_item_num++;
            PlayerData.SelfRecoveryPowerCapsule += 1;
            print("Self_Recovery_Power_Capsule" + selected_item_num);
        }
    }
    void Start()
    {
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        if (playerData == null)
        {
            Debug.LogError("PlayerManager not found or PlayerData component missing.");
        }

        item = GameObject.FindGameObjectsWithTag("item");
        if (item.Length == 0)
        {
            Debug.LogError("No items found with the 'item' tag.");
        }

        select_item = GameObject.FindGameObjectsWithTag("item_select");
        if (select_item.Length == 0)
        {
            Debug.LogError("No items found with the 'item_select' tag.");
        }

        next_select_btn = GameObject.Find("NextSelectButton").GetComponent<Button>();
        if (next_select_btn == null)
        {
            Debug.LogError("Next select button not found.");
        }
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        GameObject item_tab = GameObject.Find("Canvas").transform.Find("Tool").gameObject;
        item_tab.SetActive(true);
        item = GameObject.FindGameObjectsWithTag("item");
        select_item = GameObject.FindGameObjectsWithTag("item_select");
        next_select_btn.interactable = false;
        for (int i = 0; i < item.Length; i++)
        {
            item[i].GetComponent<Button>();
            item[i].SetActive(false);
        }
        item_tab.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
