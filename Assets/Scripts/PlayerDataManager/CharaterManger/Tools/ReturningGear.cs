using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//되돌아가는 톱니바퀴: 자신의 행위판정을 다시 하게 한다.
public class ReturningGear : Tool
{
    public override void Use()
    {
        Judgment judgment = GameObject.Find("Judgement Manger").GetComponent<Judgment>();
    }
}
