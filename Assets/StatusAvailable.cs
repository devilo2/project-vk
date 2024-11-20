using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusAvailable : MonoBehaviour
{
    public Button[] StatusList;
    // Start is called before the first frame update
    void Start()
    {
        for (int temp = 0; temp < StatusList.Length; temp++){
            StatusList[temp].interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
