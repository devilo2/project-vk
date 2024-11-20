using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StatusAvailable : MonoBehaviour
{
    [SerializeField] private GameObject status; // 버튼들을 포함하는 오브젝트
    [SerializeField] private Judgment judgment; // Judgment 스크립트

    // Start is called before the first frame update
    void Start()
    {
        if (judgment == null || status == null)
        {
            Debug.LogError("Judgment 또는 Status 오브젝트가 연결되지 않았습니다!");
            return;
        }

        // 모든 버튼 초기화: status 오브젝트 내 버튼 검색
        Button[] buttons = status.GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = false; // 모든 버튼 비활성화
        }

        // 활성화해야 할 버튼 이름 가져오기
        List<Judgment.Status> statusObjects = judgment.availableStatuses;
        if (statusObjects == null || statusObjects.Count == 0)
        {
            Debug.LogWarning("활성화할 상태가 없습니다.");
            return;
        }

        // Status 객체에서 버튼 이름 리스트 추출
        List<string> havingStatuses = statusObjects.Select(s => s.name).ToList();

        // havingStatuses 리스트에 포함된 버튼과 "Button"이라는 이름을 가진 버튼 활성화
        foreach (Button button in buttons)
        {
            if (havingStatuses.Contains(button.gameObject.name) || button.gameObject.name == "Button")
            {
                button.interactable = true;
            }
        }
    }
}
