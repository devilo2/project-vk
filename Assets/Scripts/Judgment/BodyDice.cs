using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyDice : MonoBehaviour
{
    [SerializeField] private Sprite[] diceSides; // 주사위 면 이미지 배열 (6면의 주사위 이미지)
    [SerializeField] private Image diceImage1;  // 첫 번째 주사위 이미지 UI (첫 번째 주사위)
    [SerializeField] private Image diceImage2;  // 두 번째 주사위 이미지 UI (두 번째 주사위)
    [SerializeField] private Text resultText;   // 주사위 결과를 표시하는 텍스트 UI
    [SerializeField] private Text judgmentText; // 판정 결과를 표시하는 텍스트 UI
    [SerializeField] private Judgment judgment; // 판정 로직을 담당하는 클래스 참조
    public static int dice = 0;  // 주사위 합산 결과 저장 변수

    private bool isRolling = false; // 주사위를 굴리는 중인지 여부를 확인하는 변수

    private void Start()
    { 
        // 주사위 이미지를 초기화 (UI에서 할당된 이미지 가져오기)
        diceImage1 = diceImage1.GetComponent<Image>();
        diceImage2 = diceImage2.GetComponent<Image>();
    }

    // UI 버튼을 클릭 시 호출되는 메서드
    public void OnRollDiceButtonClicked()
    {
        if (judgment == null)
        {
            // Judgment 객체가 할당되지 않았다면 오류 메시지 출력 후 할당
            Debug.LogError("Judgment 객체가 할당되지 않았습니다!");
            judgment = GetComponent<Judgment>(); // Judgment 컴포넌트를 찾아서 할당
        }
        RollDice(); // 주사위를 굴리는 메서드 호출
    }

    // 주사위를 굴리는 메서드
    public void RollDice()
    {
        // 주사위가 굴려지지 않고 있을 때만 주사위를 굴리기
        if (!isRolling)
        {
            isRolling = true;
            StartCoroutine(RollTheDice()); // 주사위 굴리기 코루틴 호출
            dice = 0;
        }
    }
    // 이전 스탯에 대해 다시 판정
    public void ReJudge()
    {
        RollDice(); // 주사위 다시 굴리기
    }

    // 주사위를 굴리는 코루틴
    private IEnumerator RollTheDice()
    {
        
        int randomDiceSide1 = 0; // 첫 번째 주사위 면
        int randomDiceSide2 = 0; // 두 번째 주사위 면
        
        // 주사위를 20번 굴리면서 화면에 랜덤 이미지 업데이트 (애니메이션 효과)
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide1 = Random.Range(0, 6); // 첫 번째 주사위 랜덤 값 (0~5)
            randomDiceSide2 = Random.Range(0, 6); // 두 번째 주사위 랜덤 값 (0~5)
            diceImage1.sprite = diceSides[randomDiceSide1]; // 첫 번째 주사위 이미지 업데이트
            diceImage2.sprite = diceSides[randomDiceSide2]; // 두 번째 주사위 이미지 업데이트
            yield return new WaitForSeconds(0.06f); // 0.06초마다 주사위 이미지 변경
        }

        // 두 주사위 값을 합산하여 결과 처리 (주사위 값은 1부터 시작하므로 +2 보정)
        dice += randomDiceSide1 + randomDiceSide2 + 2;
        resultText.text = "주사위 합: " + dice.ToString(); // 주사위 합을 UI 텍스트로 표시

        string statName = judgment.LastJudgeStatName; // 임의의 스탯 이름 (UI에서 입력 가능한 스탯 이름으로 변경 가능)
        Judge(statName, dice); // 판정을 위한 메서드 호출

        isRolling = false; // 주사위 굴리는 상태를 종료
    }



    // 판정 로직을 처리하는 메서드
    private void Judge(string statName, int dice)
    {
        judgment.SetJudgeResult(statName, dice); // Judgment 클래스를 이용하여 판정 결과를 반환
        var result = judgment.Result;

        // 판정 결과에 따라 UI 업데이트
        switch (result)
        {
            case Judgment.JudgeResult.Pumble:
                judgmentText.text = "펌블!!!"; // 펌블 결과 시 텍스트 표시
                break;
            case Judgment.JudgeResult.Special:
                judgmentText.text = "스페셜!!!"; // 스페셜 결과 시 텍스트 표시
                break;
            case Judgment.JudgeResult.Success:
                judgmentText.text = "성공!!!"; // 성공 결과 시 텍스트 표시
                break;
            case Judgment.JudgeResult.Fail:
                judgmentText.text = "실패!!!"; // 실패 결과 시 텍스트 표시
                break;
        }
    }
}

