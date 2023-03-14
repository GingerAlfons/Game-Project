using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public Transform camTf;
    public float bgMoveMultiplier;
    

    // Update is called once per frame
    void Update()
    {
        BgMovement();
    }
    //Flyttar bakgrunderna i relation med kameran i x-led
    public void BgMovement()
    {
        transform.position = new Vector3(camTf.position.x * bgMoveMultiplier, transform.position.y, transform.position.z);
    }
}
