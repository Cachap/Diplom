using UnityEngine;
using Assets.Scripts.Machine;
using System.Collections;

public class ShopTool : MonoBehaviour
{
    //Параметры для вращения магазина
    private readonly float angle = 12f;
    private readonly float speed = 12f;

    //Номер сменяемого инструмента
    public static int number = 0;
    //Текущий номер инструмента
    private byte count = 0;
    //Тееущий угол поворота
    private float x = 0;
    //Состояние, которое необходимо для полного поворота
    private PlcHandler.ShopToolStates temporaryState;
    //Для корутины
    private bool start;

    public static PlcHandler plcHandler;

    private void Start()
    {
        plcHandler = new PlcHandler();
    }

    private void Update()
    {
        plcHandler.ReadPlcRotate();
        if (count != plcHandler.numberCurrentTool && !start && plcHandler.numberCurrentTool != 0)
        {
            count = plcHandler.numberCurrentTool;
            StartCoroutine(Rotate());
        }
    }

    private IEnumerator Rotate()
    {
        start = true;

        if (temporaryState == PlcHandler.ShopToolStates.CwRotation)
        {
            while (x < angle * count)
            {
                x += Time.deltaTime * speed;
                transform.localRotation = Quaternion.Euler(x, 0, 0);

                yield return null;
            }
        }

        if (temporaryState == PlcHandler.ShopToolStates.CcwRotation)
        {
            while (x > angle * count)
            {
                x -= Time.deltaTime * speed;
                transform.localRotation = Quaternion.Euler(x, 0, 0);

                yield return null;
            }
        }

        StopCoroutine(Rotate());

        temporaryState = plcHandler.shopToolState;
        number = count;

        if(number == plcHandler.numberTool)
        {
            plcHandler.Impulse(true);
            Invoke(nameof(Impulse), 0.5f);
        }

        start = false;
    }

    private void Impulse()
    {
        plcHandler.Impulse(false);
    }
}
