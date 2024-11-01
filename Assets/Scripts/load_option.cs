using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_option : MonoBehaviour
{
    public void SceneChange(){
        SceneManager.LoadScene("Option", LoadSceneMode.Additive);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
