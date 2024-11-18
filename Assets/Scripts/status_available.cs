using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class status_available : MonoBehaviour
{
    // Start is called before the first frame update
    public Button[] StatusList;

    public void SetCurrentStatus(int param){
        for (int temp = 0; temp < StatusList.Length; temp++){
            StatusList[temp].interactable = false;
        }
        for (int s_temp = 0; s_temp < StatusList.Length; s_temp++){
            bool result = Judgment.availableStatuses.Any();
        }
    }
    
}
