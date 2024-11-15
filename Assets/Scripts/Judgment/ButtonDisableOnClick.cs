using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisableOnClick : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        // ���� ���� ������Ʈ�� Button ������Ʈ�� �����ɴϴ�.
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("Button component not found on this GameObject!");
        }
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void DisableButton()
    {
        if (button != null)
        {
            button.interactable = false; // ��ư�� ��Ȱ��ȭ
        }
    }
}
