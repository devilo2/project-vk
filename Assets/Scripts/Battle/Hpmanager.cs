using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Hpmanager : MonoBehaviour
{


    public static int Hp = 6;
    [SerializeField] private Sprite[] Hitpoint;
    [SerializeField] private Image HpImage;
    // Start is called before the first frame update
    void Update()
    {
        // Hp ���� �迭 ���� ���� ����
        if (Hp >= 0 && Hp < Hitpoint.Length)
        {
            HpImage.sprite = Hitpoint[Hitpoint.Length - Hp - 1];
        }
        if (Hp == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
