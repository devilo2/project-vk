using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnplayer : MonoBehaviour
{
    public GameObject prefab;  // ��ȯ�� UI �̹��� ������
    private BattleManager manager;  // BattleManager ��ü
    public GameObject[] spawnplot;  // ��ȯ ��ġ �迭 (1~6)
    private GameObject spawnedPrefab;  // �̹� ������ UI �̹��� ����

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<BattleManager>();  // BattleManager ��ü ã��
    }

    // Update is called once per frame
    public void spawn()
    {
        if (manager.playerPlot >= 1 && manager.playerPlot <= 6)
        {
            // �̹� ��ȯ�� UI �̹����� �ִٸ� ��ġ�� ����
            if (spawnedPrefab != null)
            {
                movePlayerToNewPlot(); // ���� ��ȯ�� �������� �̵�
                Debug.Log("���� UI �̹����� ���ο� ��ġ�� �̵��߽��ϴ�.");
            }
            else // ������ ���� ����
            {
                spawnedPrefab = Instantiate(prefab, spawnplot[manager.playerPlot - 1].transform.position, Quaternion.identity);
                spawnedPrefab.transform.SetParent(spawnplot[manager.playerPlot - 1].transform); // Canvas ���� ��ġ�ϵ��� ����
                spawnedPrefab.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // UI ��ġ ���߱�
                Debug.Log("UI �̹����� ���� ��ȯ�Ǿ����ϴ�.");
            }
        }
    }

    // UI �̹����� �ٽ� ��ȯ�� �� �ֵ��� �ʱ�ȭ�ϴ� �Լ�
    public void resetSpawn()
    {
        if (spawnedPrefab != null)
        {
            Destroy(spawnedPrefab);  // ��ȯ�� UI ��ü�� �ı�
            spawnedPrefab = null;  // ���� ���� �ʱ�ȭ
            Debug.Log("��ȯ�� UI �̹����� �ı��Ǿ����ϴ�.");
        }
    }

    // UI �̹����� ���ο� ��ġ�� �̵���Ű�� �Լ�
    public void movePlayerToNewPlot()
    {
        if (spawnedPrefab != null && manager.playerPlot >= 1 && manager.playerPlot <= 6)
        {
            // ���ο� ��ġ�� UI �̹����� �̵�
            Vector3 newPosition = spawnplot[manager.playerPlot - 1].transform.position;
            spawnedPrefab.transform.position = newPosition;
            spawnedPrefab.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;  // UI ��ġ ���߱�
            Debug.Log($"UI �̹����� ���ο� ��ġ�� �̵��߽��ϴ�: {newPosition}");
        }
    }
}
