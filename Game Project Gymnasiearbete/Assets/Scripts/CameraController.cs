using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Rigidbody2D rbCamera;
    public float moveSpeed = 1f;
    // Har inte använt denna något: public float acc = 0.1f;
    public float moveDirection;
    public float moveCooldown = 0f;
    void Start()
    {
        
    }
    
    void Update()
    {
        //Kollar om det är dags att kalla move funktionen annars minskar timern
        if (GameManager.Instance.startTimer <= 0)
        {
            if (moveCooldown <= 0)
            {
                move();
                //Debug.Log("Kallar move funktionen");
            }
            else
            {
                moveCooldown -= Time.deltaTime;
            }
        }
    }

    //Move funktionen
    public void move()
    {
        //Slumpar värdet på moveCooldown, moveSpeed och Riktningen
        moveCooldown = Random.Range(2, 5);
        moveSpeed = Random.Range(1, 5);
        int rand = Random.Range(1, 3);

        if (rand == 1)
        {
            moveDirection = 1;
        }
        else
        {
            moveDirection = -1;
        }

        //Ger kameran en velocity i x-led
        rbCamera.velocity = new Vector2(moveDirection * moveSpeed, 0f);
    }
}
