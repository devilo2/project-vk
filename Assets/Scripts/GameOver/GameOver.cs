using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Ÿ��Ʋ ���� �̸�
    [SerializeField] private string titleSceneName = "title";

    private void Start()
    {
        // �ڷ�ƾ ����
        StartCoroutine(WaitAndLoadTitleScene());
    }

    private IEnumerator WaitAndLoadTitleScene()
    {
        // 10�� ���
        yield return new WaitForSeconds(5f);

        // Ÿ��Ʋ ������ ��ȯ
        SceneManager.LoadScene(titleSceneName);
    }
}
