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
	private int count = 0;
	//Тееущий угол поворота
	private float _x = 0;
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

		if ((plcHandler.shopToolState == PlcHandler.ShopToolStates.CwRotation 
			|| plcHandler.shopToolState == PlcHandler.ShopToolStates.CcwRotation)
			&& !start)
        {
            temporaryState = plcHandler.shopToolState;
            StartCoroutine(Rotate());
        }
    }

    private IEnumerator Rotate()
    {
        start = true;

		if (temporaryState == PlcHandler.ShopToolStates.CwRotation)
		{
			float x = 0;
			while (x <= angle)
			{
				x += Time.deltaTime * speed;
				_x += Time.deltaTime * speed;
				transform.localRotation = Quaternion.Euler(_x, 0, 0);

				yield return null;
			}

			plcHandler.shopToolInputState = PlcHandler.ShopToolInputStates.RRR;
			plcHandler.WritePlcRotate();

			yield return new WaitForSecondsRealtime(0.2f);

			plcHandler.shopToolInputState = PlcHandler.ShopToolInputStates.None;
			plcHandler.WritePlcRotate();
		}

		if (temporaryState == PlcHandler.ShopToolStates.CcwRotation)
		{
			float x = 0;
			while (x <= angle)
			{
				x += Time.deltaTime * speed;
				_x -= Time.deltaTime * speed;
				transform.localRotation = Quaternion.Euler(_x, 0, 0);

				yield return null;
			}

			plcHandler.shopToolInputState = PlcHandler.ShopToolInputStates.LRR;
			plcHandler.WritePlcRotate();

			yield return new WaitForSecondsRealtime(0.2f);

			plcHandler.shopToolInputState = PlcHandler.ShopToolInputStates.None;
			plcHandler.WritePlcRotate();

		}

		StopCoroutine(Rotate());

        temporaryState = plcHandler.shopToolState;

		if(plcHandler.numberCurrentTool == plcHandler.numberTool)
		{
			number = plcHandler.numberTool;
			plcHandler.Impulse(true);
		}

        start = false;
    }
}
