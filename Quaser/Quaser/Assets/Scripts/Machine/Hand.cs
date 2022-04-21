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
    private bool startPneumatic;

    //Сдвиг для центрирования инструмента в патроне
    private const float Z = 0.0077f;
    private const float Y = -0.85f;
    private const float X = 0.0002f;

    //Направление вращения патрона
    private bool rotateToCapture = true;

    private readonly float speed = 0.6f;
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

        if (ShopTool.plcHandler.shopToolState == PlcHandler.ShopToolStates.PneumaticCylinder && Input.GetKeyDown(KeyCode.M))
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
        ChangeTools[ShopTool.number].ToolObject.transform.localPosition = new Vector3(X, Y, Z);
        ChangeTools[ShopTool.number].ToolObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

        CurrentTool.ToolObject.transform.localPosition = new Vector3(0, 0, 0);
        CurrentTool.ToolObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

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
                    float y = 90;
                    while (y <= 180)
                    {
                        y += speed;
                        transform.localRotation = Quaternion.Euler(0, y, 0);

                        yield return new WaitForEndOfFrame();
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
                    float y = cur_Y;
                    while (y >= cur_Y - 0.074f)
                    {
                        y -= speed/150;
                        transform.position = new Vector3(transform.position.x, y, transform.position.z);

                        yield return new WaitForEndOfFrame();
                    }
                    StopCoroutine(ChangeTool());

                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.Down;
                    break;
                }
            //Проворот руки на 360 градусов
            case PlcHandler.HandOutputStates.RotateAngle_180:
                {
                    float y = 180;
                    while (y >= 0)
                    {
                        y -= speed;
                        transform.localRotation = Quaternion.Euler(0, y, 0);
                        yield return new WaitForEndOfFrame();
                    }
                    StopCoroutine(ChangeTool());

                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.RotateAngle_180;
                    break;
                }
            //Задвижение руки
            case PlcHandler.HandOutputStates.Up:
                {
                    float y = cur_Y - 0.074f;
                    while (y <= cur_Y)
                    {
                        y += speed / 150;
                        transform.position = new Vector3(transform.position.x, y, transform.position.z);

                        yield return new WaitForEndOfFrame();
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

                    float y = 0;
                    while (y >= -90)
                    {
                        y -= speed;
                        transform.localRotation = Quaternion.Euler(0, y, 0);

                        yield return new WaitForEndOfFrame();
                    }
                    StopCoroutine(ChangeTool());

                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.Return;
                    startPneumatic = false;
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
            while (PatronObjects[ShopTool.number].transform.rotation.eulerAngles.z >= 1f)
            {
                PatronObjects[ShopTool.number].transform.RotateAround(CreateTools.tools[ShopTool.number].PositionPoint,
                    Vector3.back,
                    Mathf.Lerp(0, 90, Time.deltaTime));

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
                    Mathf.Lerp(0, -90, Time.deltaTime));

                yield return  null;
            }
            ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.None;
            rotateToCapture = true;
        }

        WritePlc();
        StopCoroutine(Rotate());
    }
    #endregion
}
