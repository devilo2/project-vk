using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class change_to_exploration : MonoBehaviour
{
    // Start is called before the first frame update
    public void load_exploration()
    {
        SceneManager.LoadScene("cutscene1");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
