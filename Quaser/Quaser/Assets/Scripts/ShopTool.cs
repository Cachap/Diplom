using UnityEngine;

public class ShopTool : MonoBehaviour
{
    private float tempAngle = 12f;

    public static int number = 0;

    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
		{
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(transform.rotation.x + tempAngle, transform.rotation.y, transform.rotation.z),
               Time.deltaTime * 15f);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            number = (int)(tempAngle / 12.0f % 30);
            tempAngle += 12f;
        }
    }
}
