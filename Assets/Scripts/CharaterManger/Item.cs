using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Rendering;
using UnityEngine;
using UnityEngine.UI;


//�������� ǥ���ϴ� Ŭ����
public class Item
{   
    public string name { get; set; }
    protected Image itemImage;  
    protected int positionx; //�κ��丮������ ��ġ
    protected int positiony;

    public Item()
    {
        name = string.Empty;
    }

    public Item(string name)
    {
        this.name = name;
    }
}

