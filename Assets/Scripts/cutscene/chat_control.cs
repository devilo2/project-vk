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
        yield return StartCoroutine(NormalChat("19XX�� 12�� 8��\n" +
            "���� ������ ������ ��ѵ��� ���� �����Ѽ�����.\n" +
            "�� ����԰Ե� Ȳ���� ���� ���ԵȰ��� � ��ȣ�� �Ƿ� �����̾���\n" +
            "�簳���� ���� ������ ������ ��������� �߰ߵǾ��ٴ°��̴�.\n"));
        yield return StartCoroutine(NormalChat("���������� ������ ���̾����� �����ϱ⿡�� �ʹ� ū ���̾���\n" +
            "�̰��� �� ���� ���� 1����°�̴�. �׷����� �ұ��ϰ� ������ ���� �������� �ʴ� ������ �Ա����̾���\n" +
            "���̻��� �ѷ� �� ���� ������ �ʴ´�\n" +
            "���ñ��� �ܼ��� ������ �ʴ´ٸ� �׳� ���ư����� �ϴ� ����..."));
        yield return StartCoroutine(NormalChat("Ŀ�ٶ� ������ �Բ� ���� ���ִ� ���� ������ ������ ���̾���\n" +
            "���� �װ����� ������� �õ��� ���� ����ä ������ ��Ҽ����� ��������.\n"));
        yield return StartCoroutine(NormalChat("�׷��� �������� ��� 4��°��...\n" +
            "�����Ծ��� ���� �ķ��� ���� �� ������ �����̴�\n" +
            "�ݹ� �����Ϸ� �ðŶ�� ����� �̹� ������ �����̴�.\n" +
            "���� ������ �־��� �������� ���� ���� ���� �������ִ� �Ǹ��� �԰� ���� ������ �Ա��� ���� �ͻ��̴�."));
        yield return StartCoroutine(NormalChat("�������� �Ѿ���� ��Ŭ���Ͻÿ�."));
        if(Input.GetMouseButton(0))
        {
            SceneManager.LoadScene("exploration 1");
        }
    }
}
