using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPlayerDataScene : MonoBehaviour
{
    private static LoadPlayerDataScene loadingManager = null;
    private void Awake()
    {
        if(loadingManager == null)
        {
            loadingManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        string curSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("PlayerDataManager");
        if(curSceneName != "Charater Select")
            SceneManager.LoadScene(curSceneName);
        Destroy(this.gameObject);
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
