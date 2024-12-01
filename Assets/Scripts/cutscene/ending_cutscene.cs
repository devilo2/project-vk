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
        yield return StartCoroutine(NormalChat("������ ����������� ������ �հ� ����� ��ħ�� �������� �ö󰡴� ����� �߰��߽��ϴ�.\n" +
            "��� ���δ� ���� ���� ���� �վ����������־����ϴ�.\n" +
            "����� ���� ������ ���� �� �ִٴ� ����� ������ ����� �پ�ö󰬽��ϴ�."));
        yield return StartCoroutine(NormalChat("���� �ʰ� ����� �پ�ö� ���� ����� ����ο� �޻��� ������ �� �־����ϴ�.\n" +
            "�׸��� �������� ������ �������� ���� ������ �����Ե� ������ ���� �����Ϸ��� ���ڵ�� ����ŷȽ��ϴ�."));
        yield return StartCoroutine(NormalChat("����� ������ �ö�� ���� �� ���ڵ��� ����� ���ͺ��ϱ����� ����� ������ ����������ϴ�." +
            "������ ���� ���ͺ並 �����ϸ鼭 ����� ��ð��� ������ �����..."));
        yield return StartCoroutine(NormalChat("��ȣ���� �޾Ҵ� �Ƿڸ� �ϼ��� �������� ����� ���� ���� ������ϴ�.\n" +
            "-happy end-"));
    }
}
