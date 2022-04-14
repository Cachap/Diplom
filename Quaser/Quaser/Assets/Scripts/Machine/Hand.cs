using Assets.Scripts.Machine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    //Положение руки по координате Y
    private float cur_Y;

    //Запущена ли корутина
    private bool start;

    //Сдвиг для центрирования инструмента в патроне
    private const float Z = 0.0077f;
    private const float Y = -0.85f;

    //Направление вращения патрона
    private bool rotateToCapture = true;

    private readonly float speed = 30f;
    private bool change;

    private GameObject Hand_obj;

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
        ShopTool.plcHandler.ReadPlc();
        ShopTool.plcHandler.ReadPlcRotate();

        if(!start && ShopTool.plcHandler.handOutupState != PlcHandler.HandOutputStates.None)
        {
            StartCoroutine(ChangeTool());
        }

        if (ShopTool.plcHandler.handInputState == PlcHandler.HandInputStates.PneumaticCylinder && Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(Rotate());
        }

        if (ShopTool.plcHandler.handOutupState == PlcHandler.HandOutputStates.PneumaticCylinder)
        {
            StartCoroutine(Rotate());
        }
    }

    private void WritePlc()
    {
        ShopTool.plcHandler.WritePlc();
        start = false;
    }

    private void ChangeParent()
    {
        //Перепривязка родителей
        ChangeTools[ShopTool.number].ToolObject.transform.parent = SpindleObject.transform;
        CurrentTool.ToolObject.transform.parent = PatronObjects[ShopTool.number].transform;

        //Меняем объекты местами
        (CurrentTool, ChangeTools[ShopTool.number]) = (ChangeTools[ShopTool.number], CurrentTool);

        CurrentTool.ToolObject.tag = "CurrentTool";
        ChangeTools[ShopTool.number].ToolObject.tag = "Tool";

        //Сдвиг к центру патрона и шпинделя
        ChangeTools[ShopTool.number].ToolObject.transform.localPosition = new Vector3(0, Y, Z);
        CurrentTool.ToolObject.transform.localPosition = Vector3.zero;

        TextUpdate.Change(CurrentTool);
        change = true;
    }

    #region Алгоритм смены инструмента
    public IEnumerator ChangeTool()
    {
        start = true;

        switch (ShopTool.plcHandler.handOutupState)
        {
            //Поворот руки на 180 градусов
            case PlcHandler.HandOutputStates.RotateAngle_90:
                {
                    while(transform.rotation.eulerAngles.y != 180)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation,
                        Quaternion.Euler(transform.rotation.x,
                            180,
                            transform.rotation.z),
                        Time.deltaTime* speed);

                        yield return null;
                    }
                    StopCoroutine(ChangeTool());

                    ChangeTools[ShopTool.number].ToolObject.transform.parent = Hand_obj.transform;
                    CurrentTool.ToolObject.transform.parent = Hand_obj.transform;
                    change = false;

                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.RotateAngle_90;

                    break;
                }
            //Выдвижение руки
            case PlcHandler.HandOutputStates.Down:
                {
                    while(transform.position.y >= cur_Y - 0.074f + 0.000003f)
                    {
                        transform.position = Vector3.Lerp(transform.position,
                           new Vector3(transform.position.x,
                               cur_Y - 0.074f,
                               transform.position.z),
                           Time.deltaTime * speed);

                        yield return null;
                    }
                    StopCoroutine(ChangeTool());

                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.Down;
                    break;
                }
            //Проворот руки на 360 градусов
            case PlcHandler.HandOutputStates.RotateAngle_180:
                {
                    while (transform.rotation.eulerAngles.y >= 0.01)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation,
                        Quaternion.Euler(transform.rotation.x,
                            360,
                            transform.rotation.z),
                        Time.deltaTime * speed);

                        yield return null;
                    }
                    StopCoroutine(ChangeTool());

                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.RotateAngle_180;
                    break;
                }
            //Задвижение руки
            case PlcHandler.HandOutputStates.Up:
                {
                    while (transform.position.y <= 0.7127)
                    {
                        transform.position = Vector3.Lerp(transform.position,
                        new Vector3(transform.position.x,
                            cur_Y,
                            transform.position.z),
                        Time.deltaTime * speed);
                        yield return null;
                    }
                    StopCoroutine(ChangeTool());

                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.Up;
                    break;
                }
            //Проворот руки на 90 градусов
            case PlcHandler.HandOutputStates.Return:
                {
                    if (!change)
                        ChangeParent();

                    while (transform.rotation.eulerAngles.y != 270)
                    {
                        transform.rotation = Quaternion.LerpUnclamped(transform.rotation,
                        Quaternion.Euler(transform.rotation.x,
                           -90,
                           transform.rotation.z),
                        Time.deltaTime * speed*5);

                        yield return null;
                    }
                    StopCoroutine(ChangeTool());

                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.Return;
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);

                    break;
                }
        }

        WritePlc();
    }
    #endregion

    #region Поворот патрона
    public IEnumerator Rotate()
    {
        start = true;
        if (rotateToCapture)
        {
            while (PatronObjects[ShopTool.number].transform.rotation.eulerAngles.z >= 1)
            {
                PatronObjects[ShopTool.number].transform.RotateAround(CreateTools.tools[ShopTool.number].PositionPoint,
                    Vector3.back,
                    Mathf.SmoothStep(0, 90, Time.deltaTime * 10));

                yield return null;
            }
            ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.PneumaticCylinder;
            rotateToCapture = false;
        }
        else
        {
            while (PatronObjects[ShopTool.number].transform.rotation.eulerAngles.z <= 90)
            {
                PatronObjects[ShopTool.number].transform.RotateAround(CreateTools.tools[ShopTool.number].PositionPoint,
                    Vector3.back,
                    Mathf.SmoothStep(0, -90, Time.deltaTime * 10));

                yield return null;
            }
            ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.None;
            rotateToCapture = true;
        }
        WritePlc();
        StopCoroutine(Rotate());
    }
    #endregion
}
