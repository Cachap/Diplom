using Assets.Scripts.Machine;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private float cur_Y;

    private int changePos = 1;
    private int changePos_Y = 1;

    //Сдвиг для центрирования инструмента в патроне
    private const float Z = 0.0077f;
    private const float Y = -0.85f;

    private GameObject Hand_obj;
    private Tool temp_tool;

    [SerializeField]
    private GameObject ShopToolObject;

    [SerializeField]
    private GameObject SpindleObject;

    public static Tool CurrentTool;

    public static List<Tool> ChangeTools;
    public static List<GameObject> PatronObjects;

    private void Start()
	{
        PatronObjects = new List<GameObject>();
        ChangeTools = new List<Tool>();

        Hand_obj = gameObject;
        cur_Y = transform.position.y;
    }

	void Update()
    {
        //Поворот руки на 180 градусов
        if (Input.GetKey(KeyCode.Alpha1))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.Euler(transform.rotation.x, 180*changePos, transform.rotation.z), 
                15f);
        }

        //Захват инструментов
		if (Input.GetKeyUp(KeyCode.Alpha1))
		{
            ChangeTools[ShopTool.number].ToolObject.transform.parent = Hand_obj.transform;
            CurrentTool.ToolObject.transform.parent = Hand_obj.transform;
		}

        //Выдвижение руки
		if (Input.GetKey(KeyCode.Alpha2))
        {  
            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(transform.position.x, 
                    cur_Y - 0.074f, 
                    transform.position.z), 
                15f);
        }

        //Проворот руки на 360 градусов
        if (Input.GetKey(KeyCode.Alpha3))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(transform.rotation.x, 
                    360/changePos, 
                    transform.rotation.z),
               30f);
        }

        //Задвижение руки
        if (Input.GetKey(KeyCode.Alpha4))
        {
            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(transform.position.x,
                    cur_Y, 
                    transform.position.z), 
                15f);
        }

        //Проворот руки на 90 градусов
        if (Input.GetKey(KeyCode.Alpha5))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(transform.rotation.x, 
                    90*changePos_Y, 
                    transform.rotation.z),
               30f);
        }

        //Перепривязка родителей
        if (Input.GetKeyUp(KeyCode.R))
        {
            changePos_Y *= -1;

            //Нужно для корректоного поворота руки в исходное положение
            if (changePos == 1)	
                changePos = 2;
			else 
                changePos = 1;

            temp_tool = CurrentTool;

            //Перепривязка родителей
            ChangeTools[ShopTool.number].ToolObject.transform.parent = SpindleObject.transform;
            CurrentTool.ToolObject.transform.parent = PatronObjects[ShopTool.number].transform;

            CurrentTool = ChangeTools[ShopTool.number];         
            CurrentTool.ToolObject.tag = "CurrentTool";

            ChangeTools[ShopTool.number] = temp_tool;
            ChangeTools[ShopTool.number].ToolObject.tag = "Tool";

            //Сдвиг к центру патрона и шпинделя
            ChangeTools[ShopTool.number].ToolObject.transform.localPosition = new Vector3(0,Y,Z);
            CurrentTool.ToolObject.transform.localPosition = Vector3.zero;

            TextUpdate.Change(CurrentTool);
        }
	}
}
