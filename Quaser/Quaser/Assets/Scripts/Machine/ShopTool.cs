using UnityEngine;
using Assets.Scripts.Machine;
using System.Collections;

public class ShopTool : MonoBehaviour
{
    private readonly float angle = 12f;
    private readonly float speed = 12f;

    public static int number = 0;

    private int count = 1;
    private float x = 0;
    private bool start;

    public static PlcHandler plcHandler;

    private void Start()
    {
        plcHandler = new PlcHandler();
    }

    private void Update()
    {
       // plcHandler.ReadPlcRotate();

        //if (Input.GetKeyDown(KeyCode.Space) && !start)
        //{
        //    StartCoroutine(Rotate());
        //}
    }

    //private IEnumerator Rotate()
    //{
    //    start = true;

    //    while (x < angle * count)
    //    {
    //        x += Time.deltaTime * speed;
    //        transform.localRotation = Quaternion.Euler(x,0,0);

    //        yield return null;
    //    }
    //    transform.rotation = Quaternion.Euler(angle * count, 0, 0);

    //    StopCoroutine(Rotate());
    //    start = false;W

    //    if(count <= number)
    //       count++;
    //}
}
