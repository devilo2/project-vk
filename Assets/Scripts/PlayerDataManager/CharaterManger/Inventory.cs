using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    Item[][] items;
    int curXpos; //인벤토리에서 현재 좌표
    int curYpos;

    // Start is called before the first frame update
    void Start()
    {
        curXpos = 0;
        curYpos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
