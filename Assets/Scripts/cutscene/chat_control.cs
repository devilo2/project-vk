using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        yield return StartCoroutine(NormalChat("19XX년 12월 8일\n" +
            "나는 일지를 덮으며 찌뿌둥한 몸을 일으켜세웠다.\n" +
            "이 빌어먹게도 황량한 곳에 오게된것은 어떤 부호의 의뢰 때문이었다\n" +
            "재개발을 위해 매입한 땅에서 고대유적이 발견되었다는것이다.\n"));
        yield return StartCoroutine(NormalChat("원래였으면 거절할 일이었지만 거절하기에는 너무 큰 돈이었다\n" +
            "이곳에 온 지도 벌써 1개월째이다. 그럼에도 불구하고 건져낸 것은 열리지도 않는 유적의 입구뿐이었다\n" +
            "더이상은 둘러 볼 곳도 보이지 않는다\n" +
            "오늘까지 단서가 보이지 않는다면 그냥 돌아가려고 하던 찰나..."));
        yield return StartCoroutine(NormalChat("커다란 굉음과 함께 내가 서있던 곳의 지반이 무너진 것이었다\n" +
            "나는 그곳에서 벗어나려는 시도도 하지 못한채 깊디깊은 어둠속으로 빨려들어갔다.\n"));
        yield return StartCoroutine(NormalChat("그렇게 고립된지도 어언 4일째다...\n" +
            "가져왔었던 물과 식량도 거의 다 떨어진 상태이다\n" +
            "금방 구조하러 올거라는 희망은 이미 버린지 오래이다.\n" +
            "이제 나에게 주어진 선택지는 나를 향해 입을 벌리고있는 악마의 입과 같은 유적의 입구에 들어가는 것뿐이다."));
        yield return StartCoroutine(NormalChat("다음으로 넘어가려면 좌클릭하시오."));
        if(Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("exploration 1");
        }
    }
}
