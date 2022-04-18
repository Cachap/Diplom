using Assets.Scripts.Machine;
using AxiOMADataTest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTools : MonoBehaviour
{
    //Список инструментов
    public static List<Tool> tools;

    //Координаты для появления патрона
    private const float X = 0.4665f;
    private const float Y = 0.9028f;
    private const float Z = -0.0695f;

    //Позиция и вращение патрона
    private Vector3 currentlyPositionTool;
    private Quaternion currentlyRotationTool;

    [SerializeField]
    private GameObject spindle;

    private void Start()
    {
        tools = new List<Tool>();
        currentlyPositionTool = new Vector3(X, Y, Z);
        currentlyRotationTool = Quaternion.Euler(0, 0, 90);

        #region Заполнение магазина инструментов
        for (int i = 0; i < 30; i++)
        {
            transform.rotation = Quaternion.Euler(i * 12f, 0, 0);

            tools.Add(new Tool
            {
                Name = $"Change_Tool_{i}",
                Position = currentlyPositionTool,
                Rotation = currentlyRotationTool,
                ParentTransform = gameObject.transform,
                Number = i
            });
            tools[i].AddToolInShopTool();
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
        #endregion
    }

    private void Update()
    {
        //Создание инструмента в шпинделе(рабочего инструмента)
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Hand.CurrentTool = new Tool
            {
                Name = $"{Form1.toolParam_1.ToolName}",
                Position = new Vector3(0, 0, 0),
                Rotation = Quaternion.Euler(0, 0, 0),
                ParentTransform = spindle.transform,
                Number = 1000,
                Length = Form1.toolParam_1.cutting_edge[0].lenght1,
                Radius = Form1.toolParam_1.cutting_edge[0].radius
            };
            Hand.CurrentTool.AddToolInSpindle();
        }
    }

    public void UpdateTools()
    {
        for (int i = 0; i < 30; i++)
        {
            tools[i].UpdateTool();
        }
    }
}
