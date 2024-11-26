using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Plot Selection UI References")]
    public GameObject plotSelectionUI;  // Plot ���� UI �г�
    public GameObject battleUI;         // Battle UI �г�
    public Button[] plotButtons;        // Plot ���� ��ư�� (6��)
    private int selectedPlot = 0;       // ���� ���õ� Plot �ε��� (0���� 5����)

    [Header("Battle UI References")]
    public GameObject[] toolButtons;    // Tool ��ư �迭
    public GameObject[] skillButtons;   // Skill ��ư �迭
    public GameObject nextTurnButton;   // Next Turn ��ư
    private int currentCategory = 0;     // ���� ī�װ� (0 = Tool, 1 = Skill, 2 = Next Turn)
    private int currentIndex = 0;        // ���� ī�װ� ���� ��ư �ε���
    private Button currentButton;        // ���� ���õ� ��ư

    private BattleManager battleManager;

    void Start()
    {
        // �ʱ� ���� ����
        plotSelectionUI.SetActive(true);
        battleUI.SetActive(false);

        // BattleManager ����
        battleManager = FindObjectOfType<BattleManager>();

        // Plot ���� �ʱ�ȭ
        HighlightPlotButton(selectedPlot);

        // Battle �ɼ� �ʱ�ȭ
        HighlightButton(GetCurrentButton());

        // BattleManager���� �� ���� �� Plot ���� UI�� ���ư���� �˸�
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

    // Plot ���� ó��
    void HandlePlotSelection()
    {
        if (Input.GetKeyDown(KeyCode.A) && selectedPlot > 0) // �������� �̵�
        {
            selectedPlot--;
            HighlightPlotButton(selectedPlot);
        }
        else if (Input.GetKeyDown(KeyCode.D) && selectedPlot < plotButtons.Length - 1) // ���������� �̵�
        {
            selectedPlot++;
            HighlightPlotButton(selectedPlot);
        }

        if (Input.GetKeyDown(KeyCode.Return)) // ���� Ȯ��
        {
            OnPlotSelected(selectedPlot + 1);
        }
    }

    // Battle UI �ɼ� ó��
    void HandleBattleUI()
    {
        // A/D Ű�� ī�װ� �̵�
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentCategory = Mathf.Max(0, currentCategory - 1);
            currentIndex = 0; // ī�װ� ��ȯ �� �ε��� �ʱ�ȭ
            UpdateButtonHighlight();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentCategory = Mathf.Min(2, currentCategory + 1);
            currentIndex = 0; // ī�װ� ��ȯ �� �ε��� �ʱ�ȭ
            UpdateButtonHighlight();
        }

        // W/S Ű�� ��ư �̵�
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

        if (Input.GetKeyDown(KeyCode.Return)) // ���� ���õ� ��ư ����
        {
            ExecuteBattleOption();
        }
    }

        // Plot ��ư ���̶���Ʈ ó��
    void HighlightPlotButton(int index)
    {
        for (int i = 0; i < plotButtons.Length; i++)
        {
            ColorBlock colors = plotButtons[i].colors;
            colors.normalColor = (i == index) ? Color.yellow : Color.white; // ���õ� ��ư ���� ����
            plotButtons[i].colors = colors;
        }
    }

    // Battle �ɼ� ���̶���Ʈ ó��
    void HighlightButton(Button button)
    {
        if (button == null) return;

        ColorBlock colors = button.colors;
        colors.normalColor = Color.yellow; // ���̶���Ʈ ���� (�����)
        button.colors = colors;
    }

    // Battle �ɼ� ���̶���Ʈ ����
    void UpdateButtonHighlight()
    {
        if (currentButton != null)
        {
            RemoveHighlightButton(currentButton); // ���� ��ư�� ���̶���Ʈ ����
        }
        currentButton = GetCurrentButton(); // ���� ���õ� ��ư ������Ʈ
        HighlightButton(currentButton);    // �� ��ư ���̶���Ʈ
    }

    // ���� ���õ� ��ư ��������
    Button GetCurrentButton()
    {
        if (currentCategory == 0) // Tool ī�װ�
            return toolButtons[currentIndex].GetComponent<Button>();
        if (currentCategory == 1) // Skill ī�װ�
            return skillButtons[currentIndex].GetComponent<Button>();
        // Next Turn ī�װ�
        return nextTurnButton.GetComponent<Button>();
    }

    // ���� ī�װ��� ��ư ����Ʈ ��������
    GameObject[] GetCurrentButtonList()
    {
        if (currentCategory == 0) return toolButtons;
        if (currentCategory == 1) return skillButtons;
        return new GameObject[] { nextTurnButton };
    }

    // Plot ���� ó��
    void OnPlotSelected(int plot)
    {
        Debug.Log($"Selected Plot: {plot}");
        plotSelectionUI.SetActive(false);
        battleUI.SetActive(true);

        battleManager.InitializeBattle(plot);
        HighlightButton(GetCurrentButton()); // Battle UI�� ù ��ư ���̶���Ʈ
    }

    // Battle �ɼ� ����
    void ExecuteBattleOption()
    {
        switch (currentCategory)
        {
            case 0: // Tool
                Debug.Log($"Tool Selected: Index {currentIndex}");
                battleManager.UseTool(currentIndex); // Tool �ε����� ����
                break;

            case 1: // Skill
                Debug.Log($"Skill Selected: Index {currentIndex}");
                battleManager.UseSkill(currentIndex); // Skill �ε����� ����
                break;

            case 2: // NextTurn
                Debug.Log("Next Turn Selected");
                battleManager.EndPlayerTurn(); // �÷��̾� �� ����
                break;

            default:
                Debug.LogError("Invalid Battle Option");
                break;
        }
    }   

    // Battle�� ���� �� Plot ���� ȭ������ ���ư���
    void OnBattleEnded()
    {
        plotSelectionUI.SetActive(true); // Plot ���� ȭ�� Ȱ��ȭ
        battleUI.SetActive(false);       // Battle UI ��Ȱ��ȭ
    }

    // ���� ��ư ���̶���Ʈ ����
    void RemoveHighlightButton(Button button)
    {
        if (button == null) return;

        ColorBlock colors = button.colors;
        colors.normalColor = Color.white; // �⺻ ���� (���)
        button.colors = colors;
    }
}
