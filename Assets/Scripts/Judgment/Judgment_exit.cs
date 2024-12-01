using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class Judgment_exit : MonoBehaviour
{
    public Button button;
    public Button exitButton;
    PlayerData playerData = null;
    Judgment judgment;
    private void Start()
    {
        judgment = GameObject.Find("Judgement Manger").GetComponent<Judgment>();
        playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
    }
    private void Update()
    {
        if (button.interactable == false)
        {
            exitButton.interactable = true;
        }
        else
        {
            exitButton.interactable = false;
        }
    }
    // Start is called before the first frame update
    public void ExitScene()
    {


        if (playerData != null)
        {
            playerData.Judgeresult = judgment.Result;
        }
        
        Debug.Log(playerData.Judgeresult);    
        SceneManager.UnloadSceneAsync("judgment");
    }
}
