using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Rigidbody2D rbCamera;
    public Transform redPlayerPos;
    public Transform greenPlayerPos;
    public Transform cameraPos;
    public float maxXConstant;
    public float minYConstant;
    public float moveSpeed = 1f;
    public float moveDirection;
    public float moveCooldown = 0f;
    
    void Update()
    {
        //Uppdaterar y-v�rdet enligt funktionen
        SetYValue();

        //Kollar om kameran h�ller p� att �ka ut ur mappen
        if ((cameraPos.position.x - maxXConstant) <= -60)
        {
            FixCameraMovement(1);
        }
        else if ((cameraPos.position.x + maxXConstant) >= 60)
        {
            FixCameraMovement(-1);
        }

        //Kollar om det �r dags att kalla move funktionen annars minskar timern
        if (GameManager.Instance.startTimer <= 0)
        {
            //Kollar om det �r dags att flytta kameran
            if (moveCooldown <= 0)
            {
                Move();
                Debug.Log("Kallar move funktionen");
            }
            else
            {
                moveCooldown -= Time.deltaTime;
            }
        }
    }

    //S�tter y-v�rdet till kameran baserat av spelarnas position och en konstant
    public void SetYValue()
    {

        float newCameraYValue = (redPlayerPos.position.y + greenPlayerPos.position.y) / 2;
        if (newCameraYValue <= minYConstant)
        {
            transform.position = new Vector3(transform.position.x, minYConstant, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, newCameraYValue, transform.position.z);
        }
    }

    //Move funktionen
    public void Move()
    {
        //Slumpar v�rdet p� moveCooldown, moveSpeed och Riktningen
        moveCooldown = Random.Range(2, 5);
        moveSpeed = Random.Range(2, 6);
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

    //�ndrar riktningen p� kameran om den h�ller p� att �ka ut ur mappen
    public void FixCameraMovement(int moveCorrection)
    {
        Debug.Log("F�rhindrar kameran fr�n att �ka ut ur mappen");
        moveCooldown = 8f;
        moveSpeed = Random.Range(2, 6);

        //Ger kameran en velocity i x-led
        rbCamera.velocity = new Vector2(moveCorrection * moveSpeed, 0f);
    }
}
