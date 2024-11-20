using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class StatusAvailable : MonoBehaviour
{
    public Button[] StatusList;
    // Start is called before the first frame update
    [SerializeField] private Judgment judgement;

    void Awake(){

        if (judgement == null)
        {
            judgement = this.GetComponent<Judgment>();
        }

    }
    void Start()
    {
        for (int temp = 0; temp < StatusList.Length; temp++){
            StatusList[temp].interactable = false;
        }

        for (int s_temp = 0; s_temp < judgement.availableStatuses.Count; s_temp++){
            string status_name = judgement.availableStatuses[s_temp].name;
            Transform status_button = transform.Find(status_name);
            if (status_button != null)
            {
                GameObject status_button_obj = status_button.gameObject;
                Button button_component = status_button_obj.GetComponent<Button>();
                if (button_component != null)
                {
                    button_component.interactable = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
