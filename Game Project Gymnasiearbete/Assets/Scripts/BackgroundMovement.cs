using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public Transform camTf; //Main-kamerans transform
    public float bgMoveMultiplier; //Lägre närmare kameran
    

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
