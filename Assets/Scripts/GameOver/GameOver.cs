using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // 타이틀 씬의 이름
    [SerializeField] private string titleSceneName = "title";

    private void Start()
    {
        // 코루틴 시작
        StartCoroutine(WaitAndLoadTitleScene());
    }

    private IEnumerator WaitAndLoadTitleScene()
    {
        // 10초 대기
        yield return new WaitForSeconds(5f);

        // 타이틀 씬으로 전환
        SceneManager.LoadScene(titleSceneName);
    }
}
