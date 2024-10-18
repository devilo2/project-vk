using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class character_move : MonoBehaviour
{
    public float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 speed_vec = Vector2.zero; 
        if (Input.GetKey(KeyCode.W))
        {
            speed_vec.y += speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            speed_vec.y -= speed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            speed_vec.x -= speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            speed_vec.x += speed;
        }

        //transform.Translate(speed_vec * Time.deltaTime);
        GetComponent<Rigidbody2D>().velocity = speed_vec;
    }
}
