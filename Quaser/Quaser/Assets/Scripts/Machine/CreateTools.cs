using Assets.Scripts.Machine;
using System.Collections.Generic;
using UnityEngine;

public class CreateTools : MonoBehaviour
{
    public static List<Tool> tools;

    private Vector3 currentlyPositionTool;
    private Quaternion currentlyRotationTool;

    //Смещения
    //private const float X = 0.05f; 
    //private const float Y = -0.5f; 
    //private const float Z = -0.007f; 

    private const float X = 0.4665f; 
    private const float Y = 0.9028f; 
    private const float Z = -0.0695f;

    //private const float ROTATE_X = -12f;

    //Длина половины одной ячейки для патрона
    //private const float HALF_CELL_LENGTH = 0.05f;

    //Количество инструментов для заполнения первой четверти магазина
    //private int count = 7;

    private void Start()
    {
        tools = new List<Tool>();
        currentlyPositionTool = new Vector3(X, Y, Z);
        currentlyRotationTool = Quaternion.Euler(0, 0, 90);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.T) && tools.Count != 0 && tools.Count <= 29)
        //{
        //    if (tools.Count > 7 && tools.Count <= 29)
        //        CreationFullCircle();

        //    if (tools.Count <= 7)
        //        CreationQuarterCircle();
        //}

        if (Input.GetKeyDown(KeyCode.T) && tools.Count < 30)
        {
            tools.Add(new Tool
            {
                Name = "Tool",
                Position = currentlyPositionTool,
                Rotation = currentlyRotationTool,
                ShopToolTransform = gameObject.transform,
                Number = tools.Count
            });
            tools[tools.Count-1].AddToolInScene();
            //tools[tools.Count-1].SetPositionPoint();
        }

        if(Input.GetKeyUp(KeyCode.M))
            tools[ShopTool.number].RotateTool();
    }

    //private void CreationQuarterCircle()
    //{
    //    float y,z;

    //    currentlyRotationTool = Quaternion.Euler(ROTATE_X * tools.Count,
    //           0,
    //           90);

    //    y = currentlyPositionTool.y
    //        - HALF_CELL_LENGTH * Mathf.Sin(currentlyRotationTool.eulerAngles.x * Mathf.PI / 180)
    //        - HALF_CELL_LENGTH * Mathf.Sin((currentlyRotationTool.eulerAngles.x + 12f) * Mathf.PI / 180);

    //    z = Mathf.Sqrt(0.25f - y * y);

    //    currentlyPositionTool = new Vector3(currentlyPositionTool.x,
    //        y,
    //        z);

    //    tools.Add(new Tool
    //    {
    //        Name = "Tool",
    //        Position = currentlyPositionTool,
    //        Rotation = currentlyRotationTool,
    //        ShopToolTransform = gameObject.transform,
    //        Number = tools.Count
    //    });
    //    tools[tools.Count - 1].AddToolInScene();
    //    tools[tools.Count - 1].SetPositionPoint();
    //}

    //private void CreationFullCircle()
    //{
    //    tools.Add(new Tool
    //    {
    //        Name = "Tool",
    //        ShopToolTransform = gameObject.transform,
    //        Number = tools.Count
    //    });

    //    if (tools.Count >= 8 && tools.Count <= 16)
    //    {
    //        tools[tools.Count - 1].Position = new Vector3(tools[count].Position.x,
    //            tools[count].Position.y * -1,
    //            tools[count].Position.z);

    //        tools[tools.Count - 1].Rotation = Quaternion.Euler(tools[count].Rotation.eulerAngles.x * -1,
    //          0,
    //          90);
    //    }

    //    if (tools.Count > 16 && tools.Count <= 30)
    //    {
    //        tools[tools.Count - 1].Position = new Vector3(tools[count - 1].Position.x,
    //           tools[count - 1].Position.y,
    //           tools[count - 1].Position.z * -1);

    //        tools[tools.Count - 1].Rotation = Quaternion.Euler(tools[count].Rotation.eulerAngles.x * -1,
    //           0,
    //           90);
    //    }

    //    if (count == 0)
    //        count = tools.Count;

    //    count--;
    //    tools[tools.Count - 1].AddToolInScene();
    //    tools[tools.Count - 1].SetPositionPoint();
    //}
}
