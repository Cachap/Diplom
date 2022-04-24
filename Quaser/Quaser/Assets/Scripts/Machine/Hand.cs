using Assets.Scripts.Machine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    //��������� ���� �� ���������� Y
    private float cur_Y;

    //�������� �� ��������
    private bool start;
    private bool startPneumatic;

    //����� ��� ������������� ����������� � �������
    private const float Z = 0.0077f;
    private const float Y = -0.85f;
    private const float X = 0.0002f;

    //����������� �������� �������
    private bool rotateToCapture = true;

    private readonly float speed = 0.6f;
    private bool change;
    private bool down;
    private bool up = true;

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

        if (ShopTool.plcHandler.shopToolState == PlcHandler.ShopToolStates.C1 && !startPneumatic && up)
        {
            StartCoroutine(Rotate());
        }

        if (ShopTool.plcHandler.handOutupState == PlcHandler.HandOutputStates.None && down)
        {
            StartCoroutine(Rotate());
        }
    }

    private void WritePlc()
    {
        ShopTool.plcHandler.WritePlc();
        //start = false;
    }

    private void ChangeParent()
    {
        //������������ ���������
        ChangeTools[ShopTool.number].ToolObject.transform.parent = SpindleObject.transform;
        CurrentTool.ToolObject.transform.parent = PatronObjects[ShopTool.number].transform;

        //������ ������� �������
        (CurrentTool, ChangeTools[ShopTool.number]) = (ChangeTools[ShopTool.number], CurrentTool);

        CurrentTool.ToolObject.tag = "CurrentTool";
        ChangeTools[ShopTool.number].ToolObject.tag = "Tool";

        //����� � ������ ������� � ��������
        ChangeTools[ShopTool.number].ToolObject.transform.localPosition = new Vector3(X, Y, Z);
        ChangeTools[ShopTool.number].ToolObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

        CurrentTool.ToolObject.transform.localPosition = new Vector3(0, 0, 0);
        CurrentTool.ToolObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

        TextUpdate.Change(CurrentTool);
        change = true;
    }

    #region �������� ����� �����������
    public IEnumerator ChangeTool()
    {
        start = true;
        switch (ShopTool.plcHandler.handOutupState)
        {
            //������� ���� �� 180 ��������
            case PlcHandler.HandOutputStates._90_tool:
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

                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates._90_tool;

                    break;
                }
            //������
            case PlcHandler.HandOutputStates.Lock_tool:
                {
                    StopCoroutine(ChangeTool());
                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.Lock_tool;
                    break;
                }
            //���������� ����
            case PlcHandler.HandOutputStates.Hand_down:
                {
                    float y = cur_Y;
                    while (y >= cur_Y - 0.074f)
                    {
                        y -= speed/150;
                        transform.position = new Vector3(transform.position.x, y, transform.position.z);

                        yield return new WaitForEndOfFrame();
                    }
                    StopCoroutine(ChangeTool());

                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.Hand_D;
                    break;
                }
            //�������� ���� �� 360 ��������
            case PlcHandler.HandOutputStates._180an:
                {
                    float y = 180;
                    while (y >= 0)
                    {
                        y -= speed;
                        transform.localRotation = Quaternion.Euler(0, y, 0);
                        yield return new WaitForEndOfFrame();
                    }
                    StopCoroutine(ChangeTool());

                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates._180an;
                    break;
                }
            //���������� ����
            case PlcHandler.HandOutputStates.Hand_up:
                {
                    float y = cur_Y - 0.074f;
                    while (y <= cur_Y)
                    {
                        y += speed / 150;
                        transform.position = new Vector3(transform.position.x, y, transform.position.z);

                        yield return new WaitForEndOfFrame();
                    }
                    StopCoroutine(ChangeTool());

                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.Hand_U;
                    break;
                }
            //�������� ���� �� 90 ��������
            case PlcHandler.HandOutputStates._90_default:
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

                    ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates._90_Def;
                    ShopTool.plcHandler.Impulse(false);
                    down = true;

                    startPneumatic = false;
                    break;
                }
        }

        WritePlc();
        yield return new WaitForSecondsRealtime(1f);
        start = false;
    }
    #endregion

    #region ������� �������
    public IEnumerator Rotate()
    {
        //start = true;
        startPneumatic = true;
        if (rotateToCapture)
        {
            while (PatronObjects[ShopTool.number].transform.rotation.eulerAngles.z >= 1f)
            {
                PatronObjects[ShopTool.number].transform.RotateAround(CreateTools.tools[ShopTool.number].PositionPoint,
                    Vector3.back,
                    Mathf.Lerp(0, 90, Time.deltaTime));

                yield return null;
            }
            ShopTool.plcHandler.handInputState = PlcHandler.HandInputStates.C1;
            up = false;
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
            up = true;
            down = false;
        }

        startPneumatic = false;
        ShopTool.plcHandler.WritePlc();
        StopCoroutine(Rotate());
    }
    #endregion
}
