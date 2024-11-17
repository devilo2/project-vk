using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Rendering;
using UnityEngine;
using UnityEngine.UI;


//아이템을 표현하는 클래스
public abstract class Item
{   
    public string name { get; set; }
    protected Image itemImage;  
    protected int positionx; //인벤토리에서의 위치
    protected int positiony;
    protected int itemCount;
    public int ItemCount {  get; set; }
    public Item()
    {
        name = string.Empty;
    }

    public Item(string name)
    {
        this.name = name;
    }
}

