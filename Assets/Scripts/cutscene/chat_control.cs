using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chat_control : MonoBehaviour
{
    public Text chat_text;
    public string writerText = "";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TextPractice());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator NormalChat(string Narration)
    {
        int a = 0;
        writerText = "";
        for (a = 0; a < Narration.Length; a++)
        {
            writerText += Narration[a];
            chat_text.text = writerText;
            yield return null;
        }

        while (true)
        {
            if (Input.GetMouseButton(0))
            {
                break;
            }
            yield return null;
        }
    }

    IEnumerator TextPractice()
    {
        yield return StartCoroutine(NormalChat("�� ���� Ž�谡�� ���ΰ����� ������� Ž�翡 ���� �Ƿڰ� ���Դٴ� ����"));
        yield return StartCoroutine(NormalChat("���� ������ ���� �� Ž�縦 �����ϴ� ���� �ǹ��� ���︲�� �߻���"));
        yield return StartCoroutine(NormalChat("���︲�������� ������ ���Ͽ� ������ ����"));
    }
}
