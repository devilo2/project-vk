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
    public GameObject enemyselectUI;    // 적 패널 
    public Button[] plotButtons;        // Plot 선택 버튼들 (6개)
    private int selectedPlot = 0;       // 현재 선택된 Plot 인덱스 (0부터 5까지)

    [Header("Battle UI References")]
    public GameObject[] toolButtons;    // Tool 버튼 배열
    public GameObject[] skillButtons;   // Skill 버튼 배열
    public GameObject nextTurnButton;   // Next Turn 버튼
    private int currentCategory = 0;     // 현재 카테고리 (0 = Tool, 1 = Skill, 2 = Next Turn)
    private int currentIndex = 0;        // 현재 카테고리 내의 버튼 인덱스
    private Button currentButton;        // 현재 선택된 버튼

    [Header("Enemy Selection UI References")]
    public GameObject[] enemySelectionButtons;   // UI 버튼으로 각 적을 선택
    public Text selectedEnemyText;               // 선택된 적을 표시할 텍스트
    private int selectedEnemyNum = 0;            // 현재 선택된 적의 인덱스
    private int enemyMax = 5;                    // 적의 최대 개수 (예시로 5개로 설정, 필요에 따라 수정)

    private BattleManager battleManager;
    private spawnplayer spawnplayer;

    void Start()
    {
        // 초기 상태 설정
        plotSelectionUI.SetActive(true);
        battleUI.SetActive(false);
        enemyselectUI.SetActive(false);

        // BattleManager 연결
        battleManager = FindObjectOfType<BattleManager>();
        spawnplayer = FindObjectOfType<spawnplayer>();

        // Plot 선택 초기화
        HighlightPlotButton(selectedPlot);

        // Battle 옵션 초기화
        HighlightButton(GetCurrentButton());

        // BattleManager에게 턴 종료 후 Plot 선택 UI로 돌아가라고 알림
        battleManager.OnBattleEnded += OnBattleEnded;

        // 초기 적 선택 UI 설정
        UpdateEnemySelectionUI();
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
        else if (enemyselectUI.activeSelf)
        {
            HandleEnemySelection();  // 적 선택 처리
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
            if (spawnplayer != null)
            {
                spawnplayer.spawn();
            }
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

    // 적 선택 처리
    void HandleEnemySelection()
    {
        // W/S 키로 이전/다음 적 선택
        if (Input.GetKeyDown(KeyCode.W) && selectedEnemyNum > 0)  // W 키로 이전 적 선택
        {
            selectedEnemyNum--;
            UpdateEnemySelectionUI();  // UI 업데이트
            Debug.Log($"Selected Enemy: {selectedEnemyNum}");
        }
        else if (Input.GetKeyDown(KeyCode.S) && selectedEnemyNum < enemyMax - 1)  // S 키로 다음 적 선택
        {
            selectedEnemyNum++;
            UpdateEnemySelectionUI();  // UI 업데이트
            Debug.Log($"Selected Enemy: {selectedEnemyNum}");
        }

        // Return 키로 선택된 적 확정
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log($"Enemy {selectedEnemyNum + 1} selected for action.");
            // 선택된 적에 대한 행동을 추가하는 코드 (예: 공격)
            OnEnemySelected();  // 적 선택 완료 후 처리
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
        enemyselectUI.SetActive(true); // 적 선택 UI 활성화

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
                battleUI.SetActive(false);  // Battle UI 비활성화
                enemyselectUI.SetActive(true); // 적 선택 UI 활성화
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 이전 버튼 하이라이트 제거
    void RemoveHighlightButton(Button button)
    {
        if (button == null) return;

        ColorBlock colors = button.colors;
        colors.normalColor = Color.white; // 기본 색상 (흰색)
        button.colors = colors;
    }

    // 선택된 적을 UI에 반영
    void UpdateEnemySelectionUI()
    {
        // 선택된 적의 텍스트 표시
        selectedEnemyText.text = $"Selected Enemy: {selectedEnemyNum + 1}";  // 적 번호 표시

        // 적 버튼 강조 (현재 선택된 적 버튼만 강조)
        for (int i = 0; i < enemySelectionButtons.Length; i++)
        {
            Button button = enemySelectionButtons[i].GetComponent<Button>();
            ColorBlock colors = button.colors;

            if (i == selectedEnemyNum)
            {
                colors.normalColor = Color.yellow; // 선택된 버튼은 노란색으로 강조
            }
            else
            {
                colors.normalColor = Color.white;  // 나머지 버튼은 기본 색상
            }

            button.colors = colors;

            // 버튼 활성화/비활성화 처리 (선택된 상태일 때만 활성화)
            button.interactable = i == selectedEnemyNum;  // 선택된 버튼만 상호작용 가능
        }
    }

    // 적 선택 완료 후 처리
    void OnEnemySelected()
    {
        battleUI.SetActive(true); // Battle UI 활성화
    }
}

