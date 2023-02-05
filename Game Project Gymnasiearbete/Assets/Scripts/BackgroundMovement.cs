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

    public void BgMovement()
    {
        transform.position = new Vector3(camTf.position.x * bgMoveMultiplier, transform.position.y, transform.position.z);
    }
}
