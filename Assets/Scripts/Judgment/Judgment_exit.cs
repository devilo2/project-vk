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

    [SerializeField] private Judgment judgment;

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
        PlayerData playerData = null;
        try
        {
            playerData = GameObject.Find("PlayerManager").GetComponent<PlayerData>();
        }
        catch
        {

        }

        if (playerData != null)
        {
            playerData.Judgeresult = judgment.Result;
        }
        
        Debug.Log(judgment.Result);    
        SceneManager.UnloadSceneAsync("judgment");
    }
}
