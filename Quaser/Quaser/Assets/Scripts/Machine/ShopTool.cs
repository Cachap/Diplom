using UnityEngine;
using AxiOMADataTest;

public class ShopTool : MonoBehaviour
{
    private float currentlyAngle = 12f;

    public static int number = 0;

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && IO_Digital_8_Control.arrayBool[0])
		{
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(transform.rotation.x + currentlyAngle, 
                    transform.rotation.y, 
                    transform.rotation.z),
                    15f);
        }
                 
        if (Input.GetKeyUp(KeyCode.Space))
        {
            number = (int)(currentlyAngle / 12.0f % 30);
            currentlyAngle += 12f;
            //CreateTools.tools[number].SetPositionPoint();
        }

        if (Input.GetKey(KeyCode.LeftShift) && IO_Digital_8_Control.arrayBool[1])
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(transform.rotation.x - currentlyAngle,
                    transform.rotation.y,
                    transform.rotation.z),
                    15f);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            number = (int)(currentlyAngle / 12.0f % 30);
            currentlyAngle -= 12f;
            //CreateTools.tools[number].SetPositionPoint();
        }
    }
}
