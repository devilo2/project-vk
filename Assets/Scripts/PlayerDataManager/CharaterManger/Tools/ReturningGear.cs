using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ǵ��ư��� ��Ϲ���: �ڽ��� ���������� �ٽ� �ϰ� �Ѵ�.
public class ReturningGear : Tool
{
    public override void Use()
    {
        Judgment judgment = GameObject.Find("Judgement Manger").GetComponent<Judgment>();
    }
}
