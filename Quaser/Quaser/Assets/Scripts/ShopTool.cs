using UnityEngine;

public class ShopTool : MonoBehaviour
{
    private float numberTool = 0;
    public static int number = 0;

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
		{
             transform.RotateAround(transform.position, Vector3.right, 360/30);

            numberTool = transform.rotation.eulerAngles.x / 12;
            number = (int)numberTool;
        }
    }
}
