using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Judgment_exit : MonoBehaviour
{
    public Button button;
    public Button exitButton;

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
        SceneManager.UnloadSceneAsync("judgment");
    }
}
