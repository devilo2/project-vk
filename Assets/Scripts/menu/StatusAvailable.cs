using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StatusAvailable : MonoBehaviour
{
    [SerializeField] private GameObject status; // ��ư���� �����ϴ� ������Ʈ
    private PlayerData playerData; // playerData ��ũ��Ʈ

    // Start is called before the first frame update
    void Start()
    {
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        if (status == null)
        {
            Debug.LogError("Status ������Ʈ�� ������� �ʾҽ��ϴ�!");
            return;
        }

        // ��� ��ư �ʱ�ȭ: status ������Ʈ �� ��ư �˻�
        Button[] buttons = status.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = false; // ��� ��ư ��Ȱ��ȭ
        }

        // Ȱ��ȭ�ؾ� �� ��ư �̸� ��������
        List<PlayerData.Status> statusObjects = playerData.availableStatuses;
        if (statusObjects == null || statusObjects.Count == 0)
        {
            Debug.LogWarning("Ȱ��ȭ�� ���°� �����ϴ�.");
            return;
        }

        // Status ��ü���� ��ư �̸� ����Ʈ ����
        List<string> havingStatuses = statusObjects.Select(s => s.name).ToList();

        // havingStatuses ����Ʈ�� ���Ե� ��ư�� "Button"�̶�� �̸��� ���� ��ư Ȱ��ȭ
        foreach (Button button in buttons)
        {
            if (havingStatuses.Contains(button.gameObject.name) || button.gameObject.name == "Button")
            {
                button.interactable = true;
            }
        }
    }
}
