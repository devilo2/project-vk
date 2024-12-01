using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ending_cutscene : MonoBehaviour
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
        yield return StartCoroutine(NormalChat("유적의 여러함정들과 적들을 뚫고 당신은 마침내 지상으로 올라가는 계단을 발견했습니다.\n" +
            "계단 위로는 아주 밝은 빛이 뿜어져나오고있었습니다.\n" +
            "당신은 드디어 밖으로 나갈 수 있다는 희망을 가지고 계단을 뛰어올라갔습니다."));
        yield return StartCoroutine(NormalChat("쉬지 않고 계단을 뛰어올라간 끝에 당신은 따사로운 햇살을 맞이할 수 있었습니다.\n" +
            "그리고 주위에는 이전의 지진으로 인해 밖으로 나오게된 유적에 대해 취재하러온 기자들로 들벅거렸습니다."));
        yield return StartCoroutine(NormalChat("당신이 땅위로 올라온 것을 본 기자들이 당신을 인터뷰하기위해 당신의 주위로 몰려들었습니다." +
            "유적에 관한 인터뷰를 진행하면서 당신은 삽시간에 유명세를 얻었고..."));
        yield return StartCoroutine(NormalChat("부호에게 받았던 의뢰를 완수한 보상으로 당신은 많은 돈을 얻었습니다.\n" +
            "-happy end-"));
    }
}
