using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class golem : MonoBehaviour
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
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }
    }

    IEnumerator TextPractice()
    {
        yield return StartCoroutine(NormalChat("이상한 문제를 겨우 풀고 밖으로 나올 수 있었다."));
        yield return StartCoroutine(NormalChat("근데 저 앞에 저 큰것은.... 골렘?..!"));
        yield return StartCoroutine(NormalChat("유적지를 지키던 골렘인가 이런!!"));
        yield return StartCoroutine(NormalChat("그 말과 동시에 나의 인기척을 느낀 골렘이 천천히 움직인다. "));
        yield return StartCoroutine(NormalChat("날 보내줄 생각은 없어보인다. 싸우는 수 밖에 없는 건가?"));
        yield return StartCoroutine(NormalChat("다음으로 넘어가려면 좌클릭하시오."));
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("Battle");
        }
    }
}
