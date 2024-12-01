using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnplayer : MonoBehaviour
{
    public GameObject prefab;  // 소환할 UI 이미지 프리팹
    private BattleManager manager;  // BattleManager 객체
    public GameObject[] spawnplot;  // 소환 위치 배열 (1~6)
    private GameObject spawnedPrefab;  // 이미 생성된 UI 이미지 추적

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<BattleManager>();  // BattleManager 객체 찾기
    }

    // Update is called once per frame
    public void spawn()
    {
        if (manager.playerPlot >= 1 && manager.playerPlot <= 6)
        {
            // 이미 소환된 UI 이미지가 있다면 위치를 갱신
            if (spawnedPrefab != null)
            {
                movePlayerToNewPlot(); // 기존 소환된 프리팹을 이동
                Debug.Log("기존 UI 이미지가 새로운 위치로 이동했습니다.");
            }
            else // 없으면 새로 생성
            {
                spawnedPrefab = Instantiate(prefab, spawnplot[manager.playerPlot - 1].transform.position, Quaternion.identity);
                spawnedPrefab.transform.SetParent(spawnplot[manager.playerPlot - 1].transform); // Canvas 내에 위치하도록 설정
                spawnedPrefab.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // UI 위치 맞추기
                Debug.Log("UI 이미지가 새로 소환되었습니다.");
            }
        }
    }

    // UI 이미지를 다시 소환할 수 있도록 초기화하는 함수
    public void resetSpawn()
    {
        if (spawnedPrefab != null)
        {
            Destroy(spawnedPrefab);  // 소환된 UI 객체를 파괴
            spawnedPrefab = null;  // 추적 변수 초기화
            Debug.Log("소환된 UI 이미지가 파괴되었습니다.");
        }
    }

    // UI 이미지를 새로운 위치로 이동시키는 함수
    public void movePlayerToNewPlot()
    {
        if (spawnedPrefab != null && manager.playerPlot >= 1 && manager.playerPlot <= 6)
        {
            // 새로운 위치로 UI 이미지를 이동
            Vector3 newPosition = spawnplot[manager.playerPlot - 1].transform.position;
            spawnedPrefab.transform.position = newPosition;
            spawnedPrefab.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;  // UI 위치 맞추기
            Debug.Log($"UI 이미지가 새로운 위치로 이동했습니다: {newPosition}");
        }
    }
}
