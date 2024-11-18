using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class close_inventory : MonoBehaviour
{
    public void close_inven(){
        SceneManager.UnloadSceneAsync("Inventory");
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
