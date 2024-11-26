using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Plot Selection UI References")]
    public GameObject plotSelectionUI;  // Plot 선택 UI 패널
    public GameObject battleUI;         // Battle UI 패널
    public Button[] plotButtons;        // Plot 선택 버튼들 (6개)
    private int selectedPlot = 0;       // 현재 선택된 Plot 인덱스 (0부터 5까지)

    [Header("Battle UI References")]
    public GameObject[] toolButtons;    // Tool 버튼 배열
    public GameObject[] skillButtons;   // Skill 버튼 배열
    public GameObject nextTurnButton;   // Next Turn 버튼
    private int currentCategory = 0;     // 현재 카테고리 (0 = Tool, 1 = Skill, 2 = Next Turn)
    private int currentIndex = 0;        // 현재 카테고리 내의 버튼 인덱스
    private Button currentButton;        // 현재 선택된 버튼

    private BattleManager battleManager;

    void Start()
    {
        // 초기 상태 설정
        plotSelectionUI.SetActive(true);
        battleUI.SetActive(false);

        // BattleManager 연결
        battleManager = FindObjectOfType<BattleManager>();

        // Plot 선택 초기화
        HighlightPlotButton(selectedPlot);

        // Battle 옵션 초기화
        HighlightButton(GetCurrentButton());

        // BattleManager에게 턴 종료 후 Plot 선택 UI로 돌아가라고 알림
        battleManager.OnBattleEnded += OnBattleEnded;
    }

    void Update()
    {
        if (plotSelectionUI.activeSelf)
        {
            HandlePlotSelection();
        }
        else if (battleUI.activeSelf)
        {
            HandleBattleUI();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        }
    }

    // Plot 선택 처리
    void HandlePlotSelection()
    {
        if (Input.GetKeyDown(KeyCode.A) && selectedPlot > 0) // 왼쪽으로 이동
        {
            selectedPlot--;
            HighlightPlotButton(selectedPlot);
        }
        else if (Input.GetKeyDown(KeyCode.D) && selectedPlot < plotButtons.Length - 1) // 오른쪽으로 이동
        {
            selectedPlot++;
            HighlightPlotButton(selectedPlot);
        }

        if (Input.GetKeyDown(KeyCode.Return)) // 선택 확인
        {
            OnPlotSelected(selectedPlot + 1);
        }
    }

    // Battle UI 옵션 처리
    void HandleBattleUI()
    {
        // A/D 키로 카테고리 이동
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentCategory = Mathf.Max(0, currentCategory - 1);
            currentIndex = 0; // 카테고리 전환 시 인덱스 초기화
            UpdateButtonHighlight();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentCategory = Mathf.Min(2, currentCategory + 1);
            currentIndex = 0; // 카테고리 전환 시 인덱스 초기화
            UpdateButtonHighlight();
        }

        // W/S 키로 버튼 이동
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentIndex = Mathf.Max(0, currentIndex - 1);
            UpdateButtonHighlight();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentIndex = Mathf.Min(GetCurrentButtonList().Length - 1, currentIndex + 1);
            UpdateButtonHighlight();
        }

        if (Input.GetKeyDown(KeyCode.Return)) // 현재 선택된 버튼 실행
        {
            ExecuteBattleOption();
        }
    }

        // Plot 버튼 하이라이트 처리
    void HighlightPlotButton(int index)
    {
        for (int i = 0; i < plotButtons.Length; i++)
        {
            ColorBlock colors = plotButtons[i].colors;
            colors.normalColor = (i == index) ? Color.yellow : Color.white; // 선택된 버튼 색상 강조
            plotButtons[i].colors = colors;
        }
    }

    // Battle 옵션 하이라이트 처리
    void HighlightButton(Button button)
    {
        if (button == null) return;

        ColorBlock colors = button.colors;
        colors.normalColor = Color.yellow; // 하이라이트 색상 (노란색)
        button.colors = colors;
    }

    // Battle 옵션 하이라이트 갱신
    void UpdateButtonHighlight()
    {
        if (currentButton != null)
        {
            RemoveHighlightButton(currentButton); // 이전 버튼의 하이라이트 제거
        }
        currentButton = GetCurrentButton(); // 현재 선택된 버튼 업데이트
        HighlightButton(currentButton);    // 새 버튼 하이라이트
    }

    // 현재 선택된 버튼 가져오기
    Button GetCurrentButton()
    {
        if (currentCategory == 0) // Tool 카테고리
            return toolButtons[currentIndex].GetComponent<Button>();
        if (currentCategory == 1) // Skill 카테고리
            return skillButtons[currentIndex].GetComponent<Button>();
        // Next Turn 카테고리
        return nextTurnButton.GetComponent<Button>();
    }

    // 현재 카테고리의 버튼 리스트 가져오기
    GameObject[] GetCurrentButtonList()
    {
        if (currentCategory == 0) return toolButtons;
        if (currentCategory == 1) return skillButtons;
        return new GameObject[] { nextTurnButton };
    }

    // Plot 선택 처리
    void OnPlotSelected(int plot)
    {
        Debug.Log($"Selected Plot: {plot}");
        plotSelectionUI.SetActive(false);
        battleUI.SetActive(true);

        battleManager.InitializeBattle(plot);
        HighlightButton(GetCurrentButton()); // Battle UI의 첫 버튼 하이라이트
    }

    // Battle 옵션 실행
    void ExecuteBattleOption()
    {
        switch (currentCategory)
        {
            case 0: // Tool
                Debug.Log($"Tool Selected: Index {currentIndex}");
                battleManager.UseTool(currentIndex); // Tool 인덱스를 전달
                break;

            case 1: // Skill
                Debug.Log($"Skill Selected: Index {currentIndex}");
                battleManager.UseSkill(currentIndex); // Skill 인덱스를 전달
                break;

            case 2: // NextTurn
                Debug.Log("Next Turn Selected");
                battleManager.EndPlayerTurn(); // 플레이어 턴 종료
                break;

            default:
                Debug.LogError("Invalid Battle Option");
                break;
        }
    }   

    // Battle이 끝난 후 Plot 선택 화면으로 돌아가기
    void OnBattleEnded()
    {
        plotSelectionUI.SetActive(true); // Plot 선택 화면 활성화
        battleUI.SetActive(false);       // Battle UI 비활성화
    }

    // 이전 버튼 하이라이트 제거
    void RemoveHighlightButton(Button button)
    {
        if (button == null) return;

        ColorBlock colors = button.colors;
        colors.normalColor = Color.white; // 기본 색상 (흰색)
        button.colors = colors;
    }
}
