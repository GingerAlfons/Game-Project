using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movespeed = 1f;
    public float acc = 0.1f;
    public float movedirection;
    void Start()
    {
        
    }
    public void move()
    {
        int rand = Random.Range(1, 3);
        if (rand == 1)
        {
            movedirection = -1f;
        }
        else
        {
            movedirection = 1f;
        }
    }
    void Update()
    {

    }
}
