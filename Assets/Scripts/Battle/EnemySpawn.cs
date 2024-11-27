using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Transform[] plotPositions; // 각 플롯 위치
    public GameObject enemyPrefab;    // 적 프리팹
    private GameObject spawnedPrefab;

    // 적을 소환하는 함수
    public void SpawnEnemies(Enemy[] enemies)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            // 적의 플롯 값에 맞는 위치를 가져옴
            int plot = enemies[i].plot;
            
            // plot 값에 해당하는 위치를 가져오기
            if (plot >= 1 && plot <= plotPositions.Length)
            {
                spawnedPrefab = Instantiate(enemyPrefab, plotPositions[plot - 1].transform.position, Quaternion.identity);
                spawnedPrefab.transform.SetParent(plotPositions[plot - 1].transform);
                Debug.Log($"{enemies[i].Name}이(가) {plot}에 소환되었습니다.");
            }
        }
    }
}
