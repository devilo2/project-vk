using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exit_inventory : MonoBehaviour
{
    // Start is called before the first frame update

    public void close_inven(){
        SceneManager.UnloadSceneAsync("Inventory");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
