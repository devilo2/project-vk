using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camer : MonoBehaviour
{
    //Player Æ÷Áö¼Ç 
    public Transform playerTransform;

    public float speed;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;


    // Start is called before the first frame update
    public float cameraSpeed = 5.0f;

    public GameObject player;

    private void Update()
    {
        Vector3 dir = player.transform.position - this.transform.position;
        Vector3 moveVector = new Vector3(dir.x * cameraSpeed * Time.deltaTime, dir.y * cameraSpeed * Time.deltaTime, 0.0f);
        this.transform.Translate(moveVector);
    }
}
