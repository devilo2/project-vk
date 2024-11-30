using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyDice : MonoBehaviour
{
    [SerializeField] private Sprite[] diceSides; // �ֻ��� �� �̹��� �迭 (6���� �ֻ��� �̹���)
    [SerializeField] private Image diceImage1;  // ù ��° �ֻ��� �̹��� UI (ù ��° �ֻ���)
    [SerializeField] private Image diceImage2;  // �� ��° �ֻ��� �̹��� UI (�� ��° �ֻ���)
    [SerializeField] private Text resultText;   // �ֻ��� ����� ǥ���ϴ� �ؽ�Ʈ UI
    [SerializeField] private Text judgmentText; // ���� ����� ǥ���ϴ� �ؽ�Ʈ UI
    [SerializeField] private Judgment judgment; // ���� ������ ����ϴ� Ŭ���� ����
    public static int dice = 0;  // �ֻ��� �ջ� ��� ���� ����

    private bool isRolling = false; // �ֻ����� ������ ������ ���θ� Ȯ���ϴ� ����

    private void Start()
    { 
        // �ֻ��� �̹����� �ʱ�ȭ (UI���� �Ҵ�� �̹��� ��������)
        diceImage1 = diceImage1.GetComponent<Image>();
        diceImage2 = diceImage2.GetComponent<Image>();
    }

    // UI ��ư�� Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnRollDiceButtonClicked()
    {
        if (judgment == null)
        {
            // Judgment ��ü�� �Ҵ���� �ʾҴٸ� ���� �޽��� ��� �� �Ҵ�
            Debug.LogError("Judgment ��ü�� �Ҵ���� �ʾҽ��ϴ�!");
            judgment = GetComponent<Judgment>(); // Judgment ������Ʈ�� ã�Ƽ� �Ҵ�
        }
        RollDice(); // �ֻ����� ������ �޼��� ȣ��
    }

    // �ֻ����� ������ �޼���
    public void RollDice()
    {
        // �ֻ����� �������� �ʰ� ���� ���� �ֻ����� ������
        if (!isRolling)
        {
            isRolling = true;
            StartCoroutine(RollTheDice()); // �ֻ��� ������ �ڷ�ƾ ȣ��
            dice = 0;
        }
    }
    // ���� ���ȿ� ���� �ٽ� ����
    public void ReJudge()
    {
        RollDice(); // �ֻ��� �ٽ� ������
    }

    // �ֻ����� ������ �ڷ�ƾ
    private IEnumerator RollTheDice()
    {
        
        int randomDiceSide1 = 0; // ù ��° �ֻ��� ��
        int randomDiceSide2 = 0; // �� ��° �ֻ��� ��
        
        // �ֻ����� 20�� �����鼭 ȭ�鿡 ���� �̹��� ������Ʈ (�ִϸ��̼� ȿ��)
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide1 = Random.Range(0, 6); // ù ��° �ֻ��� ���� �� (0~5)
            randomDiceSide2 = Random.Range(0, 6); // �� ��° �ֻ��� ���� �� (0~5)
            diceImage1.sprite = diceSides[randomDiceSide1]; // ù ��° �ֻ��� �̹��� ������Ʈ
            diceImage2.sprite = diceSides[randomDiceSide2]; // �� ��° �ֻ��� �̹��� ������Ʈ
            yield return new WaitForSeconds(0.06f); // 0.06�ʸ��� �ֻ��� �̹��� ����
        }

        // �� �ֻ��� ���� �ջ��Ͽ� ��� ó�� (�ֻ��� ���� 1���� �����ϹǷ� +2 ����)
        dice += randomDiceSide1 + randomDiceSide2 + 2;
        resultText.text = "�ֻ��� ��: " + dice.ToString(); // �ֻ��� ���� UI �ؽ�Ʈ�� ǥ��

        string statName = judgment.LastJudgeStatName; // ������ ���� �̸� (UI���� �Է� ������ ���� �̸����� ���� ����)
        Judge(statName, dice); // ������ ���� �޼��� ȣ��

        isRolling = false; // �ֻ��� ������ ���¸� ����
    }



    // ���� ������ ó���ϴ� �޼���
    private void Judge(string statName, int dice)
    {
        judgment.SetJudgeResult(statName, dice); // Judgment Ŭ������ �̿��Ͽ� ���� ����� ��ȯ
        var result = judgment.Result;

        // ���� ����� ���� UI ������Ʈ
        switch (result)
        {
            case Judgment.JudgeResult.Pumble:
                judgmentText.text = "�ߺ�!!!"; // �ߺ� ��� �� �ؽ�Ʈ ǥ��
                break;
            case Judgment.JudgeResult.Special:
                judgmentText.text = "�����!!!"; // ����� ��� �� �ؽ�Ʈ ǥ��
                break;
            case Judgment.JudgeResult.Success:
                judgmentText.text = "����!!!"; // ���� ��� �� �ؽ�Ʈ ǥ��
                break;
            case Judgment.JudgeResult.Fail:
                judgmentText.text = "����!!!"; // ���� ��� �� �ؽ�Ʈ ǥ��
                break;
        }
    }
}

