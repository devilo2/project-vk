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
        yield return StartCoroutine(NormalChat("뭐 대충 탐험가인 주인공에게 고대유적 탐사에 대한 의뢰가 들어왔다는 내용"));
        yield return StartCoroutine(NormalChat("대충 유적에 도착 후 탐사를 진해하던 도중 의문의 땅울림이 발생함"));
        yield return StartCoroutine(NormalChat("땅울림으로인해 유적의 지하에 떨어져 고립됨"));
    }
}
