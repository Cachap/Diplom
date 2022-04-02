using UnityEngine;
using AxiOMADataTest;

public class ShopTool : MonoBehaviour
{
    private readonly float angle = 12f;

    public static int number = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IO_Digital_8_Control.arrayBool[0])
		{
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(transform.rotation.eulerAngles.x + angle, 
                    transform.rotation.y, 
                    transform.rotation.z),
                    15f);
            number++;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && IO_Digital_8_Control.arrayBool[1])
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(transform.rotation.eulerAngles.x - angle,
                    transform.rotation.y,
                    transform.rotation.z),
                    15f);
            number--;
        }
    }
}
