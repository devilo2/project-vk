using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class yamanshi_puzzle : MonoBehaviour
{
    public GameObject puzzleUI;
    GameObject puzzle_Input;
    public GameObject puzzle_Fail_UI;
    public GameObject puzzle_Success_UI;


    bool isPuzzleActive = false;
    // Start is called before the first frame update
    void Start()
    {
        puzzle_Input = puzzleUI.transform.GetChild(0).gameObject;
        puzzle_Input.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isPuzzleActive) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                Puzzle_Disable();
            }
            if(Input.GetKeyDown(KeyCode.Return)) {
                puzzle_Fail_UI.SetActive(false);
                puzzle_Success_UI.SetActive(false);

                string input_text = puzzle_Input.GetComponent<InputField>().text;
                if(input_text == "yamanashi") {
                    Puzzle_Success();
                    Puzzle_Disable();
                }
                else{
                    Puzzle_Fail();
               }
            }
        }
    }
    
    void Puzzle_Success() {
        Debug.Log("Puzzle Success");
        puzzle_Success_UI.SetActive(true);
    }

    void Puzzle_Fail() {
        Debug.Log("Puzzle Fail");
        puzzle_Fail_UI.SetActive(true);
    }

    public void Puzzle_Enable() {
        Time.timeScale = 0;
        puzzle_Input.GetComponent<InputField>().text = "";
        isPuzzleActive = true;
        puzzle_Input.SetActive(true);
    }

    public void Puzzle_Disable() {
        Time.timeScale = 1;
        isPuzzleActive = false;
        puzzle_Input.SetActive(false);

    }
}
