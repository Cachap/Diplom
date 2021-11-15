using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    // Update is called once per frame

    private float cur_Y;

    private int changePos = 1;
    private int changePos_Y = 1;
    private int temp_toolPos = 0;

    private GameObject Hand_obj;
    private GameObject temp;

    [SerializeField]
    private GameObject ShopTool_obj;

    [SerializeField]
    private GameObject Spindle_obj;

    [SerializeField]
    private GameObject ToolCurrent_obj;

    private GameObject[] ToolChange_obj;
    private GameObject[] Patron_obj;

    private void Start()
	{
        Patron_obj = GameObject.FindGameObjectsWithTag("Patron");
        ToolChange_obj = GameObject.FindGameObjectsWithTag("Tool");

        ToolCurrent_obj.tag = "CurrentTool";

        Hand_obj = gameObject;
        cur_Y = transform.position.y;
	}

	void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.Euler(transform.rotation.x, 180*changePos, transform.rotation.z), 
                Time.deltaTime*30f);
        }

		if (Input.GetKeyUp(KeyCode.E))
		{
            if (Patron_obj[ShopTool.number] != null)
			{
                for(int i = 0; i < ToolChange_obj.Length; i++)
				{
                    if(Patron_obj[ShopTool.number].transform.Find("÷анговый патрон").transform.position == ToolChange_obj[i].transform.position)
					{
                        temp_toolPos = i;
					}
                }
                Patron_obj[ShopTool.number].transform.Find("÷анговый патрон").transform.parent = Hand_obj.transform;
            }

           // ToolCurrent_obj.transform.parent = null;
            ToolCurrent_obj.transform.parent = Hand_obj.transform;
		}

		if (Input.GetKey(KeyCode.W))
        {
            
            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(transform.position.x, 
                cur_Y - 0.074f, 
                transform.position.z), 
                Time.deltaTime*15f);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(transform.rotation.x, 360/changePos, transform.rotation.z),
               Time.deltaTime * 30f);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(transform.position.x,
                cur_Y, transform.position.z), 
                Time.deltaTime*15f);
        }

        if (Input.GetKey(KeyCode.Y))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(transform.rotation.x, 90*changePos_Y, transform.rotation.z),
               Time.deltaTime * 30f);
        }

        if (Input.GetKeyUp(KeyCode.R))
        {

            //ToolChange_obj[temp_toolPos].transform.position =
            //    new Vector3(ToolChange_obj[temp_toolPos].transform.position.x,
            //    ToolChange_obj[temp_toolPos].transform.position.y,
            //    -0.059f);
            //ToolCurrent_obj.transform.position =
            //    new Vector3(ToolChange_obj[temp_toolPos].transform.position.x,
            //    ToolChange_obj[temp_toolPos].transform.position.y,
            //    -0.0605f);


            changePos_Y *= -1;

            if(changePos == 1)
			{
                changePos = 2;
			}
			else
			{
                changePos = 1;
			}

            temp = ToolCurrent_obj; 
  
               
            ToolChange_obj[temp_toolPos].transform.parent = null;
               
            ToolChange_obj[temp_toolPos].transform.parent = Spindle_obj.transform;
            ToolCurrent_obj = ToolChange_obj[temp_toolPos];

            ToolChange_obj[temp_toolPos] = temp;
            ToolChange_obj[temp_toolPos].transform.parent = null;
            ToolChange_obj[temp_toolPos].transform.parent = Patron_obj[ShopTool.number].transform;

            ToolCurrent_obj.tag = "CurrentTool";
            ToolChange_obj[temp_toolPos].tag = "Tool";
        }
	}
}
