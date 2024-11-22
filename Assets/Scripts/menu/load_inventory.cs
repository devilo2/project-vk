using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load_inventory : MonoBehaviour
{
    // Start is called before the first frame update
    public void load_inven(){
        SceneManager.LoadScene("Inventory", LoadSceneMode.Additive);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
