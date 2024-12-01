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
        yield return StartCoroutine(NormalChat("�̻��� ������ �ܿ� Ǯ�� ������ ���� �� �־���."));
        yield return StartCoroutine(NormalChat("�ٵ� �� �տ� �� ū����.... ��?..!"));
        yield return StartCoroutine(NormalChat("�������� ��Ű�� ���ΰ� �̷�!!"));
        yield return StartCoroutine(NormalChat("�� ���� ���ÿ� ���� �α�ô�� ���� ���� õõ�� �����δ�. "));
        yield return StartCoroutine(NormalChat("�� ������ ������ ����δ�. �ο�� �� �ۿ� ���� �ǰ�?"));
        yield return StartCoroutine(NormalChat("�������� �Ѿ���� ��Ŭ���Ͻÿ�."));
        if (Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("Battle");
        }
    }
}
