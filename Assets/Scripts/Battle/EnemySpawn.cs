using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public Transform[] plotPositions; // �� �÷� ��ġ
    public GameObject enemyPrefab;    // �� ������
    private GameObject spawnedPrefab;

    // ���� ��ȯ�ϴ� �Լ�
    public void SpawnEnemies(Enemy[] enemies)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            // ���� �÷� ���� �´� ��ġ�� ������
            int plot = enemies[i].plot;
            
            // plot ���� �ش��ϴ� ��ġ�� ��������
            if (plot >= 1 && plot <= plotPositions.Length)
            {
                spawnedPrefab = Instantiate(enemyPrefab, plotPositions[plot - 1].transform.position, Quaternion.identity);
                spawnedPrefab.transform.SetParent(plotPositions[plot - 1].transform);
                Debug.Log($"{enemies[i].Name}��(��) {plot}�� ��ȯ�Ǿ����ϴ�.");
            }
        }
    }
}
