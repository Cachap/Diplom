using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private float cur_Y;

    private int changePos = 1;
    private int changePos_Y = 1;
    private const float Z = 0.0077f;
    private const float Y = -0.85f;
    private GameObject Hand_obj;
    private GameObject temp;

    [SerializeField]
    private GameObject ShopTool_obj;

    [SerializeField]
    private GameObject Spindle_obj;

    [SerializeField]
    private GameObject ToolCurrent_obj;

    public static List<GameObject> ToolChange_obj;
    public static List<GameObject> Patron_obj;

    private void Start()
	{
        Patron_obj = new List<GameObject>();
        ToolChange_obj = new List<GameObject>();
        //ToolCurrent_obj.tag = "CurrentTool";

        Hand_obj = gameObject;
        cur_Y = transform.position.y;
	}

	void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.Euler(transform.rotation.x, 180*changePos, transform.rotation.z), 
                15f);
        }

		if (Input.GetKeyUp(KeyCode.E))
		{
            //         if (Patron_obj[ShopTool.number] != null)
            //{
            //             for (int i = 0; i < ToolChange_obj.Count; i++)
            //	{
            //                 if (Patron_obj[ShopTool.number].transform.Find("Цанговый патрон").transform.position == ToolChange_obj[i].transform.position)
            //		{
            //                     temp_toolPos = i;
            //		}
            //           }

            //             Patron_obj[ShopTool.number].transform.Find("Цанговый патрон").transform.parent = Hand_obj.transform;
            //         }

            //ToolCurrent_obj.transform.parent = null;
            //ToolCurrent_obj.transform.parent = Hand_obj.transform;
            ToolChange_obj[ShopTool.number].transform.parent = Hand_obj.transform;
            ToolCurrent_obj.transform.parent = Hand_obj.transform;
		}

        //Выдвижение
		if (Input.GetKey(KeyCode.W))
        {  
            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(transform.position.x, 
                    cur_Y - 0.074f, 
                    transform.position.z), 
                Time.deltaTime*15f);
        }

        //Проворот на 360 градусов
        if (Input.GetKey(KeyCode.Q))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(transform.rotation.x, 
                    360/changePos, 
                    transform.rotation.z),
               Time.deltaTime * 30f);
        }

        //Задвижение
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(transform.position.x,
                    cur_Y, 
                    transform.position.z), 
                Time.deltaTime*15f);
        }

        //Проворот на 90 градусов
        if (Input.GetKey(KeyCode.Y))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(transform.rotation.x, 
                    90*changePos_Y, 
                    transform.rotation.z),
               Time.deltaTime * 30f);
        }

        //Сброс "Родителей" инструментов и привязка новых
        if (Input.GetKeyUp(KeyCode.R))
        {
            changePos_Y *= -1;

            if (changePos == 1)	
                changePos = 2;
			else 
                changePos = 1;

            temp = ToolCurrent_obj; 
              
            //ToolChange_obj[temp_toolPos].transform.parent = null;
              
            //Перепривязка родителей
            ToolChange_obj[ShopTool.number].transform.parent = Spindle_obj.transform;
            ToolCurrent_obj.transform.parent = Patron_obj[ShopTool.number].transform;

            ToolCurrent_obj = ToolChange_obj[ShopTool.number];

            ToolChange_obj[ShopTool.number] = temp;

            //Сдвиг к центру патрона и шпинделя
            ToolChange_obj[ShopTool.number].transform.localPosition = new Vector3(0,Y,Z);
            ToolCurrent_obj.transform.localPosition = Vector3.zero;

            //ToolChange_obj[temp_toolPos].transform.parent = null;
            //ToolChange_obj[temp_toolPos].transform.parent = Patron_obj[ShopTool.number].transform;

            //ToolCurrent_obj.tag = "CurrentTool";
            //ToolChange_obj[temp_toolPos].tag = "Tool";
        }
	}
}
