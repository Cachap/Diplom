using UnityEngine;
using Assets.Scripts.Machine;

public class ShopTool : MonoBehaviour
{
    private readonly float angle = 12f;

    private readonly float speed = 10f;

    public static int number = 0;

    private PlcHandler plcHandler;

    private void Start()
    {
        plcHandler = new PlcHandler();
    }

    private void Update()
    {
        plcHandler.ReadIOPlc();

        Debug.Log(plcHandler.plcRotate[1]);
        Debug.Log(plcHandler.plcRotate[0]);

        if(plcHandler.plcRotate[1] || plcHandler.plcRotate[0])
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
               Quaternion.Euler(transform.rotation.eulerAngles.x + angle,
                   transform.rotation.y,
                   transform.rotation.z),
                   1f);

            //number = plcHandler.numberTool;
            //plcHandler.WriteIOPlc();
        }
    }
}
