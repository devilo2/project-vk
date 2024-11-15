using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisableOnClick : MonoBehaviour
{
    private Button button;

    void Awake()
    {
        // 현재 게임 오브젝트의 Button 컴포넌트를 가져옵니다.
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("Button component not found on this GameObject!");
        }
    }

    // 버튼 클릭 시 호출되는 메서드
    public void DisableButton()
    {
        if (button != null)
        {
            button.interactable = false; // 버튼을 비활성화
        }
    }
}
