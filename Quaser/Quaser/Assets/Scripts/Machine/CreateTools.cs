using Assets.Scripts.Machine;
using AxiOMADataTest;
using System.Collections.Generic;
using UnityEngine;

public class CreateTools : MonoBehaviour
{
    //Лист инструментов
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && tools.Count < 30)
        {
            tools.Add(new Tool
            {
                Name = $"Tool_change_{ShopTool.number}",
                Position = currentlyPositionTool,
                Rotation = currentlyRotationTool,
                ParentTransform = gameObject.transform,
                Number = ShopTool.number
            });
            tools[ShopTool.number].AddToolInShopTool();
        }

        if (Input.GetKeyDown(KeyCode.Y) && tools.Count == 0)
        {
            Hand.CurrentTool = new Tool
            {
                Name = $"Tool_main",
                Position = new Vector3(0,0,0),
                Rotation = Quaternion.Euler(0, 0, 0),
                ParentTransform = spindle.transform,
                Number = 1000 //Поправить
            };
            Hand.CurrentTool.AddToolInSpindle();
        }

        if (Input.GetKeyUp(KeyCode.M))
            tools[ShopTool.number].RotateToolToCapture();

        if (Input.GetKeyUp(KeyCode.N))
            tools[ShopTool.number].ReturnTool();
    }
}
