using Assets.Scripts.Machine;
using AxiOMADataTest;
using System.Collections;
using System.Threading;
using UnityEngine;

public class Client : MonoBehaviour
{
    public Form1 form;
    private Thread thread;
    private bool isZero;

	public bool isRun;
	public bool isChangeTool;
	public bool permissionChange;
	public bool start;
	public bool isAccident;

	public float x;
	public float y;
	public float z;
	public PlcHandler plcHandler;

	public void RunClient()
	{
		form = Program.GetForm();
		plcHandler = new PlcHandler(form);
		isRun = true;
	}

	private void Start()
	{
		thread = new Thread(() => Initialization());
		thread.Start();
	}

	private void Update()
    {
		if (isRun && !isChangeTool && !isAccident)
		{
			if (form.x != 0)
				x = (float)form.x / 1000;

			if (form.z != 0 || !isZero)
			{
				y = -0.610f + ((float)form.z / 1000)
				+ Hand.CurrentTool.CutterObject.transform.localScale.y * 2
				+ 0.03f //�������� ������
				- 0.07f; //�����
				isZero = true;
			}
			
			
			if (form.y != 0)
				z = (float)form.y / 1000;
		}
	}

	public void ChangeTool()
	{
		StartCoroutine(ChangePositionForChangeTool());
	}

	private void Initialization() => Program.CreateWindow();

	public IEnumerator ChangePositionForChangeTool()
	{
		start = true;
		if(!isChangeTool)
		{
			isChangeTool = true;
			while (y <= -0.002)
			{
				y += 0.002f;
				yield return null;
			}
			permissionChange = true;
			StopCoroutine(ChangePositionForChangeTool());
		}
		else
		{
			permissionChange = false;
			while (y > -0.608f + Hand.CurrentTool.CutterObject.transform.localScale.y * 2 + 0.03f - 0.07f)
			{
				y -= 0.002f;
				yield return null;
			}
			isChangeTool = false;
			start = false;
			StopCoroutine(ChangePositionForChangeTool());
		}
	}
}
