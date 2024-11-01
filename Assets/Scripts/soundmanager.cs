using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class soundmanager : MonoBehaviour
{
    public Slider bgm_slider;
    AudioSource bgm_player;
    // Start is called before the first frame update

    void Awake(){
        bgm_player = GameObject.Find("BGMPlayer").GetComponent<AudioSource>();

        bgm_slider = bgm_slider.GetComponent<Slider>();

        bgm_slider.onValueChanged.AddListener(ChangeBgmSound);
    }

    void ChangeBgmSound(float value){
        bgm_player.volume = value;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
